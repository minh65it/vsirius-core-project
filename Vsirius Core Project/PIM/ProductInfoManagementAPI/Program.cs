using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PimApi._Repositories.Interfaces;
using PimApi._Repositories.Repositories;
using PimApi._Services.Interfaces;
using PimApi._Services.Services;
using PimApi.Configurations;
using PimApi.Context;
using PimApi.Controllers;
using PimApi.Helpers.AutoMapper;
using PimApi.Helpers.Utilities;
using PimApi.Jobs;
using QuartzInfrastructure;
using System.Reflection;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//Add Swagger
builder.Services.AddConfigurationAddSwaggerGen();

//Identity
builder.Services.AddConfigurationIdentity(builder.Configuration);
//DBContext
builder.Services.AddDbContext<DataContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();
builder.Services.AddScoped<IVariationService, VariationService>();
//Repositories
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IAmazonMongoRepository, AmazonMongoRepository>();
builder.Services.AddTransient<IWalmartMongoRepository, WalmartMongoRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

//Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IETLOrderService, ETLOrderService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//Update Jobs
//DependencyInjection.AddUpdateJobs(builder.Services);
//Jobs
builder.Services.AddTransient<ETLOrderController>();
builder.Services.AddTransient<VariationController>();
//DependencyInjection.AddJob<PIM_ETLOrder>(builder.Services);
//DependencyInjection.AddJob<PIM_MapVariationProduct>(builder.Services);


builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());
//Excel
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));
app.UseMiddleware<ExceptionHandlingMiddleware>();

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

        var _db = scope.ServiceProvider.GetRequiredService<DataContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}