using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using QgoldIntegrationDataApi._JobServices.Interfaces;
using QgoldIntegrationDataApi._JobServices.JobServices;
using QgoldIntegrationDataApi._Repositories;
using QgoldIntegrationDataApi._Repositories.Interfaces;
using QgoldIntegrationDataApi._Repositories.Repositories;
using QgoldIntegrationDataApi._Services.Interfaces;
using QgoldIntegrationDataApi._Services.Services;
using QgoldIntegrationDataApi.Controllers;
using QgoldIntegrationDataApi.Data;
using QgoldIntegrationDataApi.Helpers.AutoMapper;
using QgoldIntegrationDataApi.Helpers.Ultilities;
using QgoldIntegrationDataApi.Jobs;
using QuartzInfrastructure;
using System.Reflection;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "V1",
        Title = "Qgold Api",
        Description = "Qgold Integration Data Api"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<DBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), providerOptions =>
{
    providerOptions.CommandTimeout(6000000);
    //providerOptions.EnableRetryOnFailure(100000, TimeSpan.FromDays(1), null);
}));
builder.Services.AddSingleton<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();

builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());
//Update Jobs
DependencyInjection.AddUpdateJobs(builder.Services);
//Job PullQgold
builder.Services.AddTransient<QgoldProcessTriggerController>();
DependencyInjection.AddJob<Qgold_PullProduct>(builder.Services);
DependencyInjection.AddJob<Qgold_MappingQgoldToPIM>(builder.Services);


builder.Services.AddTransient<IRepositoryAccessor, RepositoryAccessor>();
builder.Services.AddScoped<IQgoldCsvDataService, QgoldCsvDataService>();
builder.Services.AddScoped<IMappingQgoldToVsiriusService, MappingQgoldToVsiriusService>();
builder.Services.AddScoped<IKeyService, KeyService>();
builder.Services.AddScoped<IAzureStorage, AzureStorage>();
builder.Services.AddScoped<IPullQgoldTriggerService, PullQgoldTriggerService>();
//builder.Services.AddSignalR();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    });
}
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
});

// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting()
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        //endpoints.MapHub<NotificationSignalRHub>("/notificationHub");
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
