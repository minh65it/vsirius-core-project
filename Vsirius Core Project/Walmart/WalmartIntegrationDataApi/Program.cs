using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WalmartIntegrationDataApi.Data;
using WalmartIntegrationDataApi.Common;
using Microsoft.OpenApi.Models;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi._Services;
using WalmartIntegrationDataApi._Repositories.Interfaces;
using WalmartIntegrationDataApi._Repositories;
using QuartzInfrastructure;
using WalmartIntegrationDataApi.Job;
using WalmartIntegrationDataApi.Controllers;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._JobServices.JobService;
using System.Text;
using WalmartIntegrationDataApi.Common.Helpers;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
Util.ActiveEPPLUS();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("*");
    });
});
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Walmart API",
        Description = "Secret: fecb6608f0f3c8d512ee61adf930793435ac2926e2e5b4edc297ccbd6be5c500a7345628691eb2f7c41f8c378d14031dd772d69f32fcac50fe90897653998b39",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme." +
        "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
        "\r\n\r\n Example: \"Bearer qazwsx\"",
    });
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();
//API CRUD
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IProductWalmartService, ProductWalmartService>();
builder.Services.AddScoped<IOrderWalmartService, OrderWalmartService>();

builder.Services.AddTransient<IOrderWalmartRepository, OrderWalmartRepository>();
builder.Services.AddTransient<IOrderQgoldRepository, OrderQgoldRepository>();
builder.Services.AddTransient<OperationResult>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddSingleton<IValidationWithMasterExcel, ValidationWithMasterExcel>();
builder.Services.AddDbContext<WalmartDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"),
    sqlServerOptions => sqlServerOptions.CommandTimeout(60000)));


//Update Jobs
DependencyInjection.AddUpdateJobs(builder.Services);
//Add Job
builder.Services.AddScoped<IGetOrderWalmartService, GetOrderWalmartService>();
builder.Services.AddScoped<IGetReturnOrderService, GetReturnOrderService>();
builder.Services.AddScoped<IInteractWalmartSellerCommonService, InteractWalmartSellerCommonService>();
builder.Services.AddScoped<ITrackingWalmartOrderService, TrackingWalmartOrderService>();
builder.Services.AddScoped<ISubmitOrderToQgoldService, SubmitOrderToQgoldService>();
builder.Services.AddScoped<IApplyShippingTemplateService, ApplyShippingTemplateService>();
builder.Services.AddScoped<IUploadProductToSellerService, UploadProductToSellerService>();
builder.Services.AddScoped<IUpdateInventoryService, UpdateInventoryService>();
builder.Services.AddScoped<IUpdatePriceService, UpdatePriceService>();
builder.Services.AddScoped<IMapVsiriusToWalmartService, MapVsiriusToWalmartService>();
builder.Services.AddTransient<TriggerController>();
DependencyInjection.AddJob<Walmart_UpdateTrackingOrder>(builder.Services);
DependencyInjection.AddJob<Walmart_SubmitOrderToQgold>(builder.Services);
DependencyInjection.AddJob<Walmart_GetReturnOrder>(builder.Services);
DependencyInjection.AddJob<Walmart_GetOrder>(builder.Services);

DependencyInjection.AddJob<Walmart_ApplyShippingTemplate>(builder.Services);
DependencyInjection.AddJob<Walmart_UpdateInventory>(builder.Services);
DependencyInjection.AddJob<Walmart_UpdatePrice>(builder.Services);
DependencyInjection.AddJob<Walmart_UploadProductToSeller>(builder.Services);
DependencyInjection.AddJob<Walmart_MapVsiriusToWalmart>(builder.Services);

var app = builder.Build();
app.UseCors();

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

ApplyMigration();
app.AddCorrelationIdMiddleware();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<WalmartDbContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
