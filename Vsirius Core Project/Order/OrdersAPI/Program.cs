using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI._Repositories.Repositories;
using OrdersAPI._Services.Interfaces;
using OrdersAPI._Services.Services;
using OrdersAPI.Configurations;
using OrdersAPI.Helpers.AutoMapper;
using OrdersAPI.Helpers.ConfigSignalR;
using OrdersAPI.Helpers.Utilities;
using System.Reflection;
using VsiriusDataModel.DataContext;
using VSiriusSystemLog.CorrelationId;
using VSiriusSystemLog.WriteLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor ();
//DBContext
builder.Services.AddDbContext<VsiriusContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity
builder.Services.AddConfigurationIdentityOrder(builder.Configuration);

//Repository and Service
builder.Services.AddTransient<IVsiriusOrderRepository, VsiriusOrderRepository>();
builder.Services.AddTransient<IVsiriusOrderDetailRepository, VsiriusOrderDetailRepository>();
builder.Services.AddTransient<IVsiriusCartRepository, VsiriusCartRepository>();
builder.Services.AddTransient<IVsiriusCartDetailsRepository, VsiriusCartDetailsRepository>();
builder.Services.AddTransient<IVsiriusProductRepository, VsiriusProductRepository>();
builder.Services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IConfigJsonRepository, ConfigJsonRepository>();

builder.Services.AddScoped<ISendMailService, SendMailService>();
builder.Services.AddScoped<IVsiriusOrderService, VsiriusOrderService>();
builder.Services.AddScoped<IOderStatusService, OderStatusService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IVsiriusCartService, VsiriusCartService>();
builder.Services.AddScoped<IHttpHeaderService, HttpHeaderService>();
builder.Services.AddScoped<IKeyService, KeyService>();
builder.Services.AddScoped<IFunctionUtilities, FunctionUtilities>();
builder.Services.AddCorrelationIdGeneratorService();

builder.Services.AddWriteLog();
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

builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddSignalR();

var app = builder.Build();
await SeedData.Run(app, builder.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseRouting();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.UseStaticFiles();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin())
    .UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<OrderHub>("/orderHub");
    });
app.AddCorrelationIdMiddleware();
ApplyMigration();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<VsiriusContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
