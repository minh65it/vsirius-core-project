using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductAiApi._JobServices.Interfaces;
using ProductAiApi._JobServices.JobServices;
using ProductAiApi._Repositories.Interfaces;
using ProductAiApi._Repositories.Repositories;
using ProductAiApi._Services.Interfaces;
using ProductAiApi._Services.Services;
using ProductAiApi.Common;
using ProductAiApi.Controllers;
using ProductAiApi.DataContext;
using ProductAiApi.Jobs;
using QuartzInfrastructure;
using System.Reflection;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProductAI_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. " +
        "\r\n\r\n Enter 'Bearer' [space] and then your token in the text input below." +
        "\r\n\r\nExample: \"Bearer qazwsx\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }});
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<ModifiedProductContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<TriggerController>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<IProductAIRepository, ProductAIRepository>();
builder.Services.AddScoped<ICreateProductAiFromPimService, CreateProductAiFromPimService>();
builder.Services.AddScoped<IProductAIService, ProductAIService>();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();

builder.Services.AddCorrelationIdGeneratorService();
DependencyInjection.AddJob<ProductAI_CreateProductAiFromPim>(builder.Services);
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
ApplyMigration();

app.Run();

void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<ModifiedProductContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}