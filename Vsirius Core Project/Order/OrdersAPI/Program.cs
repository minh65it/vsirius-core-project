using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI._Repositories.Repositories;
using OrdersAPI._Services.Interfaces;
using OrdersAPI._Services.Services;
using OrdersAPI.Configurations;
using OrdersAPI.Data;
using OrdersAPI.Helpers.AutoMapper;
using OrdersAPI.Helpers.ConfigSignalR;
using OrdersAPI.Helpers.Utilities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor ();
//DBContext
builder.Services.AddDbContext<DataContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity
builder.Services.AddConfigurationIdentity(builder.Configuration);

//Repository and Service
builder.Services.AddScoped<IVsiriusOrderRepository, VsiriusOrderRepository>();
builder.Services.AddScoped<IVsiriusOrderDetailRepository, VsiriusOrderDetailRepository>();
builder.Services.AddScoped<IVsiriusCartRepository, VsiriusCartRepository>();
builder.Services.AddScoped<IVsiriusCartDetailsRepository, VsiriusCartDetailsRepository>();
builder.Services.AddScoped<IVsiriusProductRepository, VsiriusProductRepository>();
builder.Services.AddScoped<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IVsiriusOrderService, VsiriusOrderService>();
builder.Services.AddScoped<IOderStatusService, OderStatusService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IVsiriusCartService, VsiriusCartService>();
builder.Services.AddScoped<IHttpHeaderService, HttpHeaderService>();
builder.Services.AddScoped<IKeyService, KeyService>();
builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//Add Swagger
builder.Services.AddConfigurationAddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddSignalR();

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
app.UseRouting();
//app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<OrderHub>("/orderHub");
});
app.Run();
