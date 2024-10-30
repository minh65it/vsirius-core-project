

using AutoMapper;
using CSRApi.Helpers.ConfigSignalR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using OrdersAPI.Configurations;
using OrdersAPI.Helpers.ConfigSignalR;
using Serilog;
using SkuMappingAPI.Data;
using System.Reflection;
using VsiriusApi.Configurations;
using VsiriusApi.DependencyInjection;
using VsiriusApi.Helper.AutoMapper;
using VsiriusApi.Helpers.Utilities;
using VsiriusApi.SwaggerConfiguration;
using VsiriusDataModel.DataContext;
using VsiriusSystem.Data;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

// Add more doc API
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

// Add Confiruration
//Add ConfigurationIdentity
builder.Services.AddConfigurationIdentity(builder.Configuration);

// Add Context
builder.Services.AddContext(builder.Configuration);

//Add DI CSR
builder.Services.AddCSRServices(builder.Configuration);
//Add DI Identity
builder.Services.AddIdentityServices(builder.Configuration);
//Add DI SKU
builder.Services.AddSKUServices(builder.Configuration);
//Add DI Product
builder.Services.AddProductServices(builder.Configuration);
//Add DI Order
builder.Services.AddOrderServices(builder.Configuration);
//Add DI System
builder.Services.AddSystemServices(builder.Configuration);
//Add DI Excluded
builder.Services.AddExcludedServices(builder.Configuration);
//Add DI Amazon
builder.Services.AddAmazonServices(builder.Configuration);
//Add DI ProductAI
builder.Services.AddProductAIServices(builder.Configuration);
//Add DI Qgold
builder.Services.AddQgoldServices(builder.Configuration);
//Add DI StullerOrder
builder.Services.AddStullerOrderServices(builder.Configuration);
//Add DI Stuller
builder.Services.AddStullerServices(builder.Configuration);
//Add DI Package
builder.Services.AddPackageServices(builder.Configuration);
//Add DI Walmart
builder.Services.AddWalmartServices(builder.Configuration);
//Add DI QgoldOrder
builder.Services.AddQgoldOrderServices(builder.Configuration);

builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();



//Mapper
builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());


builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddWriteLog();

builder.Services.AddRouting();
builder.Services.AddMvc(options => options.EnableEndpointRouting = false);
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddSignalR();
builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.ApplyMigrationQgoldApi();
builder.Services.ApplyMigrationVsiriusContext();
builder.Services.ApplyMigrationSKU();
builder.Services.ApplyMigrationSystem();
builder.Services.ApplyMigrationExcluded();
builder.Services.ApplyMigrationProductAI();
builder.Services.ApplyMigrationAmazon();
builder.Services.ApplyMigrationWalmart();

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Swaager for Production
app.UseCustomSwagger();
app.UseSession();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")).UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
    endpoints.MapHub<OrderHub>("/orderHub");
});
app.UseSerilogRequestLogging();
// Identity Seed data
await SeedDataConfig.Run(app);
// Order Seed data
await SeedData.Run(app, builder.Configuration);
app.AddCorrelationIdMiddleware();
app.Run();





