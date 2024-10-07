using AutoMapper;
using PackageApi._Repositories.IRepositories;
using PackageApi._Repositories.Repositories;
using PackageApi._Services.IServices;
using PackageApi._Services.Services;
using PackageApi.Configurations;
using PackageApi.Utilities;
using PackageApi.Utilities.Mapping;
using static PackageApi.Utilities.Blob;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Identity
builder.Services.AddConfigurationIdentity(builder.Configuration);

//Add Swagger
builder.Services.AddConfigurationAddSwaggerGen();

builder.Services.AddTransient<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<IAzureStorage, AzureStorage>();

builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

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

app.Run();
