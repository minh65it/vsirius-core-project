
using AmazonIntegrationDataApi.Data;
using EbayCoreApi.DataContext;
using ExcludedProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkuMappingAPI.Data;
using StullerDataApi.Data;
using VsiriusDataModel.DataContext;
using VsiriusSystem.Data;
using WalmartIntegrationDataApi.Data;

namespace VsiriusApi.DependencyInjection
{
    public static class ContextConfigure
    {
        public static void AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            //  For System
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SystemConnection")));

            services.AddDbContext<VsiriusContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("VsiriusWebConnection")));

            services.AddDbContext<MappingSkuDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SKUConnection")));

            // For Excluded
            services.AddDbContext<ExcludedProductAPI.Data.ModifiedProductContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("PIMConnection")));
            // For ProductAI
            services.AddDbContext<ProductAiApi.DataContext.ModifiedProductContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("PIMConnection")));
            // For Amazon
            services.AddDbContext<DBContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("AmazonConnection")));

            services.AddDbContext<QgoldIntegrationDataApi.Data.DBContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("PIMConnection"), providerOptions =>
                {
                    providerOptions.CommandTimeout(6000000);
                }));

            // For Stuller
            //var mongoDbSettings = configuration.GetSection("ConnectionStrings").Get<MongoDbSettings>();
            services.Configure<StullerDataApi.Data.MongoDbSettings>(
               configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<IMongoDbSettings>(p =>
                p.GetRequiredService<IOptions<MongoDbSettings>>().Value);

            // For Walmart
            services.AddDbContext<WalmartDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("WalmartConnection"),
                sqlServerOptions => sqlServerOptions.CommandTimeout(60000)));

            // For Qgold Order
            //var mongoDbSettings2 = configuration.GetSection("ConnectionStrings").Get<OrderProcessApi.Data.MongoDbSettings>();
            services.Configure<OrderProcessApi.Data.MongoDbSettings>(
                configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<OrderProcessApi.Data.IMongoDbSettings>(p =>
                    p.GetRequiredService<IOptions<OrderProcessApi.Data.MongoDbSettings>>().Value);

            // For Pim
            services.AddDbContext<PimApi.Context.DataContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("PIMConnection")));

            services.AddDbContext<EbayContext>(
                 options => options.UseSqlServer(configuration.GetConnectionString("EbayConnection")));
        }
    }
}
