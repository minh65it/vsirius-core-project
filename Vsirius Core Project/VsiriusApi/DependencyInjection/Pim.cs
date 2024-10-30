using OfficeOpenXml;
using PimApi._Repositories.Interfaces;
using PimApi._Repositories.Repositories;
using PimApi._Services.Interfaces;
using PimApi._Services.Services;
using PimApi.Controllers;
using PimApi.Jobs;
using QuartzInfrastructure;

namespace VsiriusApi.DependencyInjection
{
    public static class Pim
    {
        public static void AddPimServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IVariationService, VariationService>();

            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IAmazonMongoRepository, AmazonMongoRepository>();
            services.AddTransient<IWalmartMongoRepository, WalmartMongoRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IETLOrderService, ETLOrderService>();
            services.AddScoped<IOrderService, OrderService>();
            /*
            services.AddUpdateJobs();
            //Jobs
            services.AddTransient<ETLOrderController>();
            services.AddTransient<VariationController>();
            services.AddJob<PIM_ETLOrder>();
            services.AddJob<PIM_MapVariationProduct>();
            */
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
    }
}
