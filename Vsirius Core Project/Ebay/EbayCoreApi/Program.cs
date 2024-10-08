using EbayApi.JobInfratructure.Jobs;
using EbayCoreApi.Common.Configurations;
using EbayCoreApi.Common.Middlewares;
using EbayCoreApi.DataContext;
using EbayCoreApi.JobServices.Jobs;
using Microsoft.EntityFrameworkCore;
using QuartzInfrastructure;
using VSiriusSystemLog.CorrelationId;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddConfigurationAddSwaggerGen();
builder.Services.AddDependencyInjectionConfiguration();

builder.Services.AddHttpContextAccessor();
builder.Services.AddCorrelationIdGeneratorService();

// Add services to the container.
builder.Services.AddDbContext<EbayContext>(
                 options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

QuartzInfrastructure.DependencyInjection.AddUpdateJobs(builder.Services);
QuartzInfrastructure.DependencyInjection.AddJob<Ebay_GetOrderFromEbay>(builder.Services);
QuartzInfrastructure.DependencyInjection.AddJob<Ebay_MapperPimProductToEbayProduct>(builder.Services);
QuartzInfrastructure.DependencyInjection.AddJob<Ebay_SubmitEbayOrder>(builder.Services);
QuartzInfrastructure.DependencyInjection.AddJob<Ebay_UpdatePriceQuantityMarketplace>(builder.Services);
QuartzInfrastructure.DependencyInjection.AddJob<Ebay_UpdateTrackingOrder>(builder.Services);
QuartzInfrastructure.DependencyInjection.AddJob<Ebay_UploadProductToSeller>(builder.Services);

ConfigHolder.Configuration = builder.Configuration;
var app = builder.Build();

app.UseCors();

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

ApplyMigration();
app.AddCorrelationIdMiddleware();
app.Run();


void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<EbayContext>();

        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
