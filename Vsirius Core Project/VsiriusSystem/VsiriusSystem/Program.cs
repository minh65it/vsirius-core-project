using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using System.Text.Json;
using VsiriusSystem._Repositories.Interfaces;
using VsiriusSystem._Repositories.Repositories;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem._Services.Services;
using VsiriusSystem.Data;
using VsiriusSystem.Helpers.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<ILogRepository, LogRepository>();
builder.Services.AddTransient<IJobSchedulerRepository, JobSchedulerRepository>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IJobSchedulerService, JobSchedulerService>();
builder.Services.AddScoped<IApiKeysService, ApiKeysService>();

builder.Services.AddScoped<ITextractAwsService, TextractAWSService>();
builder.Services.AddSingleton<IKeyService, KeyService>();

builder.Services.AddScoped<IChatGPTService, ChatGPTService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});


builder.Services.AddScoped<IMapper>(sp =>
{
    return new Mapper(AutoMapperConfig.RegisterMappings());
});
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());

builder.Services.AddHttpContextAccessor();
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.Production.json").Build();
builder.Host.UseSerilog((context, configuration) => 
        configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

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
app.UseSession();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

ApplyMigration();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<DataContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
