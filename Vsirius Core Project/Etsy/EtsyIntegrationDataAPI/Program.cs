using AutoMapper;
using EtsyIntegrationDataAPI._Etsy.Etsy;
using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Repositories.Repositories;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI._Services.Services;
using EtsyIntegrationDataAPI.Controllers;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Helpers;
using EtsyIntegrationDataAPI.Helpers.AutoMapper;
using EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Helpers.OrderEtsy;
using EtsyIntegrationDataAPI.Jobs;
using Microsoft.EntityFrameworkCore;
using QuartzInfrastructure;
using System.Reflection;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    sqlServerOptions => sqlServerOptions.CommandTimeout(60000))
);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();
builder.Services.AddTransient<IAuthenEtsyRepository, AuthenEtsyRepository>();
builder.Services.AddTransient<IProductEtsyRepository, ProductEtsyRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<IPropertiesByTaxonomyRepository, PropertiesByTaxonomyRepository>();
builder.Services.AddTransient<IPropertiesProductRepository, PropertiesProductRepository>();
builder.Services.AddScoped<IAuthenticationEtsyService, AuthenticationEtsyService>();
builder.Services.AddScoped<IListingImageService, ListingImageService>();
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IProductEtsyService, ProductEtsyService>();
builder.Services.AddScoped<IMapPIMToEtsyDBService, MapPIMToEtsyDBService>();
builder.Services.AddScoped<IPIMApiClient, PIMApiClient>();
builder.Services.AddScoped<IExcludedProduct, ExcludedProduct>();
builder.Services.AddScoped<IInsertProduct, InsertProduct>();
builder.Services.AddScoped<IListingInventoryService, ListingInventoryService>();
builder.Services.AddScoped<IInsertOrder, InsertOrder>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISubmitOrder, SubmitOrder>();
builder.Services.AddScoped<IQgoldApiClient, QgoldApiClient>();
builder.Services.AddScoped<IStullerApiClient, StullerApiClient>();
builder.Services.AddScoped<ITopOrderService, TopOrderService>();
builder.Services.AddScoped<IPropertiesProductService, PropertiesProductService>();

//Jobs
builder.Services.AddTransient<MapPIMToEtsyDBController>();
DependencyInjection.AddJob<Etsy_MapPIMToEtsyDB>(builder.Services);
builder.Services.AddTransient<ListingController>();
DependencyInjection.AddJob<Etsy_CreateListDraftAndPublishListing>(builder.Services);
builder.Services.AddTransient<ListingInventoryController>();
DependencyInjection.AddJob<Etsy_UpdateAllListingInventory>(builder.Services);
// update order
builder.Services.AddTransient<OrderController>();
DependencyInjection.AddJob<Etsy_GetNewOrderEtsy>(builder.Services);
DependencyInjection.AddJob<Etsy_SubmitOrderQgold>(builder.Services);
DependencyInjection.AddJob<Etsy_UpdateTrackingOrder>(builder.Services);
// update Properties
builder.Services.AddTransient<PropertiesProductController>();
DependencyInjection.AddJob<Etsy_UpdateProperties>(builder.Services);

//Update Jobs
DependencyInjection.AddUpdateJobs(builder.Services);

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
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.AddCorrelationIdMiddleware();

/*ApplyMigration();*/
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