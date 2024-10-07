using Microsoft.Extensions.Options;
using OfficeOpenXml;
using QuartzInfrastructure;
using StullerDataApi.Controllers;
using StullerDataApi.Data;
using StullerDataApi.IRepository;
using StullerDataApi.Jobs;
using StullerDataApi.Repository;
using StullerDataApi.Services.IServices;
using StullerDataApi.Services.Services;
using StullerDataApi.Utilities;
using StullerDataApi.Utilities.Mapping.MapStullerDBToVsiriusDB;
using StullerDataApi.Utilities.PullStuller;
using System.Reflection;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

//Add database
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("Database"));
builder.Services.AddCors();

builder.Services.AddSingleton<IMongoDbSettings>(p =>
        p.GetRequiredService<IOptions<MongoDbSettings>>().Value);
// using System.Reflection;
builder.Services.AddSwaggerGen(c => {
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();
builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IKeyService, KeyService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPullStuller, PullStuller>();
builder.Services.AddScoped<IMapStullerDBToVsiriusDB, MapStullerDBToVsiriusDB>();
builder.Services.AddScoped<IProductAI, ProductAI>();

// Add services to the Utilities.
builder.Services.AddScoped<IExportDataVsirius, ExportDataVsirius>();
builder.Services.AddScoped<ICallAPIVSiriusWeb, CallAPIVSiriusWeb>();
builder.Services.AddScoped<IUtilitiesStorageBlobs, UtilitiesStorageBlobs>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Update Jobs
DependencyInjection.AddUpdateJobs(builder.Services);
DependencyInjection.AddJob<Stuller_PullDataStuller>(builder.Services);
builder.Services.AddTransient<PullStullerController>();
DependencyInjection.AddJob<Stuller_MapStullerToPIM>(builder.Services);
builder.Services.AddTransient<MapStullerDBToVsiriusDBController>();

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.AddCorrelationIdMiddleware();

app.Run();
