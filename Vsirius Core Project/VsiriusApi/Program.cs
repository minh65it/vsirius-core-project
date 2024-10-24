

using AutoMapper;
using CSRApi.Helpers.ConfigSignalR;
using System.Reflection;
using VsiriusApi.Configurations;
using VsiriusApi.DependencyInjection;
using VsiriusApi.Helper.AutoMapper;
using VsiriusApi.Helpers.Utilities;
using VsiriusApi.SwaggerConfiguration;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCustomSwagger();
}

// Swaager for Production
app.UseCustomSwagger();


app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")).UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
});
// Identity Seed data
await SeedDataConfig.Run(app);
app.AddCorrelationIdMiddleware();
app.Run();
