using AutoMapper;
using Microsoft.Extensions.Options;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessApi.Data;
using System.Reflection;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Repositories.Repositories;
using OrderProcessApi._Services.IServices.Amazon;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi._Services.IServices.Stuller;
using OrderProcessApi._Services.IServices;
using OrderProcessApi._Services.Services.Amazon;
using OrderProcessApi._Services.Services.Qgold;
using OrderProcessApi._Services.Services.Stuller;
using OrderProcessApi._Services.Services;
using OrderProcessApi._Services.IServices.Walmart;
using OrderProcessApi._Services.Services.Walmart;
using OrderProcessApi.Utilities.Amazon;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Add services to the container.
//Add database
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("Database"));

builder.Services.AddSingleton<IMongoDbSettings>(p =>
        p.GetRequiredService<IOptions<MongoDbSettings>>().Value);
// using System.Reflection;
builder.Services.AddSwaggerGen(c => {
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddCorrelationIdGeneratorService();
builder.Services.AddScoped<ILoggerCustom, LoggerCustom>();
//Utilities
builder.Services.AddTransient<IAmazonRepository, AmazonRepository>();
builder.Services.AddTransient<IWalmartRepository, WalmartRepository>();
builder.Services.AddTransient<IQgoldRepository, QgoldRepository>();
builder.Services.AddTransient<IStullerRepository, StullerRepository>();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddScoped<ISendMailService, SendMailService>();
builder.Services.AddScoped<IQgoldShippingInstruction, QgoldShippingInstruction>();
builder.Services.AddScoped<IReOrderQgold, ReOrderQgold>();
builder.Services.AddScoped<IUpdateFullFields, UpdateFullFields>();
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddScoped<IKeyService, KeyService>();
builder.Services.AddScoped<ISubmitOrderQgoldService, SubmitOrderQgoldService>();
builder.Services.AddScoped<IQAService, QAService>();
builder.Services.AddScoped<IPackageClaimService, PackageClaimService>();
builder.Services.AddScoped<ISubmitOrderStullerService, SubmitOrderStullerService>();
builder.Services.AddScoped<IQgoldProcessService, QgoldProcessService>();
builder.Services.AddScoped<IStullerProcessService, StullerProcessService>();
builder.Services.AddScoped<IAmazonProcessService, AmazonProcessService>();
builder.Services.AddScoped<IWalmartProcessService, WalmartProcessService>();
builder.Services.AddScoped<IQgoldOrderMarketplaceService, QgoldOrderMarketplaceService>();
builder.Services.AddScoped<IReSubmitOrderService, ReSubmitOrderService>();

builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
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

app.Run();
