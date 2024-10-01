using AutoMapper;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProductApi._Repositories.Interfaces;
using ProductApi._Repositories.Repositories;
using ProductApi._Services.Interfaces;
using ProductApi._Services.Services;
using ProductApi.Configurations;
using ProductApi.Data;
using ProductApi.Helpers.AutoMapper;
using ProductApi.Helpers.Utilities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
//Add Swagger
builder.Services.AddConfigurationAddSwaggerGen();
//DBContext
builder.Services.AddDbContext<DataContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//Identity
builder.Services.AddConfigurationIdentity(builder.Configuration);

//Repository and Service
builder.Services.AddScoped<IVsiriusProductRepository, VsiriusProductRepository>();
builder.Services.AddScoped<IVsiriusFilterRepository, VsiriusFilterRepository>();
builder.Services.AddScoped<IVsiriusValidProductRepository, VsiriusValidProductRepository>();
builder.Services.AddScoped<IVsiriusHomeProductRepository, VsiriusHomeProductRepository>();

//AddMemoryCache
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IVsiriusProductService, VsiriusProductService>();
builder.Services.AddScoped<IVsiriusHomeProductService, VsiriusHomeProductService>();
//Mapper
builder.Services.AddScoped<IVsiriusFilterService, VsiriusFilterService>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());
//Excel
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
builder.Services.AddScoped<ValidationVsirius>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
});
//builder.Services.AddTransient<ExceptionHandlingMiddleware>();
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//Deploy desktop server
app.UseSwagger();
app.UseSwaggerUI();
//
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();
app.UseRouting();
//app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
app.Run();
