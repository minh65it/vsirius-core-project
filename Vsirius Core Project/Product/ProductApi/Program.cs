using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProductApi._Repositories.Interfaces;
using ProductApi._Repositories.Repositories;
using ProductApi._Services.Interfaces;
using ProductApi._Services.Services;
using ProductApi.Configurations;
using ProductApi.Controllers;
using ProductApi.Helpers.AutoMapper;
using ProductApi.Helpers.httpClient;
using ProductApi.Helpers.Utilities;
using ProductApi.Jobs;
using QuartzInfrastructure;
using System.Reflection;
using VsiriusDataModel.DataContext;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
//Add Swagger
builder.Services.AddConfigurationAddSwaggerGen();
//DBContext
builder.Services.AddDbContext<VsiriusContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Identity
builder.Services.AddConfigurationIdentity(builder.Configuration);
builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();

//Repository and Service
builder.Services.AddTransient<IVsiriusProductRepository, VsiriusProductRepository>();
builder.Services.AddTransient<IVsiriusFilterRepository, VsiriusFilterRepository>();
builder.Services.AddTransient<IVsiriusValidProductRepository, VsiriusValidProductRepository>();
builder.Services.AddTransient<IVsiriusHomeProductRepository, VsiriusHomeProductRepository>();

builder.Services.AddScoped<IElasticSearchProductService, ElasticSearchProductService>();
builder.Services.AddScoped<IMapVSiriusDBToVSiriusWebDB, MapVSiriusDBToVSiriusWebDB>();
builder.Services.AddScoped<IVsiriusHomeProductService, VsiriusHomeProductService>();
builder.Services.AddScoped<IVsiriusTopProductService, VsiriusTopProductService>();
builder.Services.AddScoped<IVSiriusProductApiClient, VSiriusProductApiClient>();
builder.Services.AddScoped<IVsiriusProductService, VsiriusProductService>();
builder.Services.AddScoped<IVsiriusFilterService, VsiriusFilterService>();
builder.Services.AddScoped<IProductAIClient, ProductAIClient>();
builder.Services.AddScoped<IAzureStorage, AzureStorage>();

//Mapper
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddScoped<ValidationVsirius>();
//Update Jobs
DependencyInjection.AddUpdateJobs(builder.Services);
//Job PullQgold
builder.Services.AddTransient<TriggerController>();
DependencyInjection.AddJob<VSiriusWeb_MapPIMToVSiriusWebDB>(builder.Services);
DependencyInjection.AddJob<VSiriusWeb_UpdateFilter>(builder.Services);

builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());
//Excel
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Deploy desktop server
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

ApplyMigration();
app.AddCorrelationIdMiddleware();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<VsiriusContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}