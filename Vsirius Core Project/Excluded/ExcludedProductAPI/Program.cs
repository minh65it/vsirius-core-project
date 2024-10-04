using ExcludedProductAPI._Repositories.Interfaces;
using ExcludedProductAPI._Repositories.Repositories;
using ExcludedProductAPI._Services.Interfaces;
using ExcludedProductAPI._Services.Repositories;
using ExcludedProductAPI.Configs;
using ExcludedProductAPI.Data;
using ExcludedProductAPI.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();

builder.Services.AddConfigurationIdentity(builder.Configuration);

builder.Services.AddDbContext<ModifiedProductContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddConfigurationAddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddTransient<IExcludedProductRepository, ExcludedProductRepository>();
builder.Services.AddScoped<IExcludedProductService, ExcludedProductService>();

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