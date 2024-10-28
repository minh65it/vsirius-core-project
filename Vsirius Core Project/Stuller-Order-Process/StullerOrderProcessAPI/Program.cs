using Microsoft.OpenApi.Models;
using QuartzInfrastructure;
using StullerOrderProcessAPI._JobServices.Interfaces;
using StullerOrderProcessAPI._JobServices.JobService;
using StullerOrderProcessAPI._Repositories.Interfaces;
using StullerOrderProcessAPI._Repositories.Repositories;
using StullerOrderProcessAPI._Services.Interfaces;
using StullerOrderProcessAPI._Services.Services;
using StullerOrderProcessAPI.Common;
using StullerOrderProcessAPI.Controllers;
using StullerOrderProcessAPI.Jobs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<IStullerOrderRepository, StullerOrderRepository>();
builder.Services.AddScoped<IOrderStullerService, OrderStullerService>();

// Job services
builder.Services.AddScoped<IGetStullerOrderService, GetStullerOrderService>();
builder.Services.AddTransient<TriggerController>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Stuller API",
        Description = "",
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
        "\r\n\r\nExample: \"Bearer qazwsx\"",
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
            Array.Empty<string>()
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
//Update Jobs
DependencyInjection.AddUpdateJobs(builder.Services);
DependencyInjection.AddJob<Stuller_GetOrder>(builder.Services);

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();