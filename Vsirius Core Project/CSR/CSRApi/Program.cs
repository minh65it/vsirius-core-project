using AutoMapper;
using CSRApi._Repositories.Interfaces;
using CSRApi._Repositories.Repositories;
using CSRApi._Services.Interfaces;
using CSRApi._Services.Services;
using CSRApi.Configurations;
using CSRApi.Data;
using CSRApi.Helpers.AutoMapper;
using CSRApi.Helpers.ConfigSignalR;
using CSRApi.Helpers.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddConfigurationAddSwaggerGen();

//Identity
builder.Services.AddConfigurationIdentity(builder.Configuration);

builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddDbContext<CSRDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Repositories, Services
builder.Services.AddScoped<IFavouritesRepository, FavouritesRepository>();
builder.Services.AddScoped<IVsiriusProductRepository, VsiriusProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationDetailRepository, NotificationDetailRepository>();
builder.Services.AddScoped<INotificationMainRepository, NotificationMainRepository>();
builder.Services.AddScoped<IDrivenHtmlPagesRepository, DrivenHtmlPagesRepository>();
builder.Services.AddScoped<IPageViewRepository, PageViewRepository>();
builder.Services.AddScoped<ISecretUserRepository, SecretUserRepository>();

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IFavouritesService, FavouritesService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IPageViewService, PageViewService>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddScoped<IDrivenHtmlPagesService, DrivenHtmlPagesService>();

builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());

builder.Services.AddHttpContextAccessor();

builder.Services.AddSignalR(options =>
{
    options.ClientTimeoutInterval = TimeSpan.FromSeconds(30); // Thời gian chờ tối đa của máy khách
    options.KeepAliveInterval = TimeSpan.FromSeconds(30); // Thời gian giữa các request
});

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
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<NotificationHub>("/notificationHub");
});

app.Run();
