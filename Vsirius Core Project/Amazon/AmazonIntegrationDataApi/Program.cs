using AmazonIntegrationDataApi._Repositories;
using AmazonIntegrationDataApi._Repositories.Interfaces;
using AmazonIntegrationDataApi._Repositories.Repositories;
using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi._Services.Services;
using AmazonIntegrationDataApi.Controllers;
using AmazonIntegrationDataApi.Data;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.AutoMapper;
using AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetNewOrder;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.GetOrderShipped;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToStuller;
using AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.UpdateOrderFulfillment;
using AmazonIntegrationDataApi.Helpers.SignalRConfig;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.Update;
using AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon;
using AmazonIntegrationDataApi.Jobs;
using AutoMapper;
using DBDataContext._Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QuartzInfrastructure;
using System.Reflection;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddDbContext<DBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();
//Repository
builder.Services.AddTransient<IAmazonJewelryDataFeedItemRepository, AmazonJewelryDataFeedItemRepository>();
builder.Services.AddTransient<IAmazonMongoRepository, AmazonMongoRepository>();
//Add Services
builder.Services.AddScoped<IReturnOrderAmazon, ReturnOrderAmazon>();
builder.Services.AddScoped<IUploadProductToAmazonSeller, UploadProductToAmazonSeller>();
builder.Services.AddScoped<ValidationAmazon>();
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());
builder.Services.AddScoped<IValidator<AmazonJewelryDataFeedItem_Dto>, AmazonJewelryDataFeedValidator>();
builder.Services.AddTransient<IRepositoryAccessor, RepositoryAccessor>();
builder.Services.AddScoped<IAmazonJewelryDataFeedItemService, AmazonJewelryDataFeedItemService>();
builder.Services.AddTransient<IGetDataAmazonLocal, GetDataAmazonLocal>();
builder.Services.AddTransient<IGetDataAmazonLocalForUpdate, GetDataAmazonLocalForUpdate>();
//Add Services Order
builder.Services.AddTransient<ITriggerOrderAmazonProcessor, TriggerOrderAmazonProcessor>();
builder.Services.AddTransient<IOrderAmazonProcessor, OrderAmazonProcessor>();
//Qgold
builder.Services.AddTransient<IUpdateFullFields, UpdateFullFields>();
builder.Services.AddTransient<IOrderDB, OrderDB>();
builder.Services.AddTransient<IMappingOrder, MappingOrder>();
builder.Services.AddTransient<IQgoldApiClient, QgoldApiClient>();
builder.Services.AddTransient<IStullerClientApi, StullerClientApi>();
builder.Services.AddTransient<IOrderTracking, OrderTracking>();
builder.Services.AddTransient<IGetOrderDB, GetOrderDB>();
builder.Services.AddTransient<IOrderFulfillment, OrderFulfillment>();
builder.Services.AddTransient<IUpdateFullFieldsShipped, UpdateFullFieldsShipped>();
builder.Services.AddTransient<IMapVSiriusDBToAmazonDB, MapVSiriusDBToAmazonDB>();
builder.Services.AddTransient<IVSiriusProductApiClient, VSiriusProductApiClient>();
builder.Services.AddTransient<IInsertProduct, InsertProduct>();
builder.Services.AddTransient<IExcludedProduct, ExcludedProduct>();
builder.Services.AddTransient<IQgoldClientApi, QgoldClientApi>();
//Stuller
builder.Services.AddTransient<IMappingOrderStuller, MappingOrderStuller>();
builder.Services.AddTransient<IOrderDBForStuller, OrderDBForStuller>();
builder.Services.AddTransient<IStullerApiClient, StullerApiClient>();
builder.Services.AddLogging(builder => builder
    .AddConsole()
    .AddDebug()
    .AddEventSourceLogger());

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});
//Update Jobs
DependencyInjection.AddUpdateJobs(builder.Services);

//Quartz Products
builder.Services.AddTransient<UploadProductToAmazonSellerController>();
DependencyInjection.AddJob<Amazon_UploadProductToAmazon>(builder.Services);
DependencyInjection.AddJob<Amazon_UpdateInventoryToAmazon>(builder.Services);
DependencyInjection.AddJob<Amazon_UpdatePriceToAmazon>(builder.Services);
//Quartz Orders
builder.Services.AddTransient<TriggerOrderAmazonProcessorController>();
DependencyInjection.AddJob<Amazon_GetNewAmazonOrder>(builder.Services);
DependencyInjection.AddJob<Amazon_SubmitOrderAmazonToQgold>(builder.Services);
DependencyInjection.AddJob<Amazon_UpdateTrackingOrderAmazon>(builder.Services);
builder.Services.AddTransient<ReturnOrderController>();
DependencyInjection.AddJob<Amazon_ReturnAmazonOrder>(builder.Services);
builder.Services.AddTransient<MapVSiriusDBToAmazonDBController>();
DependencyInjection.AddJob<Amazon_MapPIMToAmazonDB>(builder.Services);

builder.Services.AddSignalR();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<NotificationSignalRHub>("/notificationHub");
    });
ApplyMigration();

app.AddCorrelationIdMiddleware();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<DBContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}