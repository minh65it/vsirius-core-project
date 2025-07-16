
using OfficeOpenXml;
using StullerDataApi.Data;
using StullerDataApi.IRepository;
using StullerDataApi.Repository;
using StullerDataApi.Services.IServices;
using StullerDataApi.Services.Services;
using StullerDataApi.Utilities.Mapping.MapStullerDBToVsiriusDB;
using StullerDataApi.Utilities;
using StullerDataApi.Utilities.PullStuller;
using StullerDataApi.Jobs;
using QuartzInfrastructure;
using StullerDataApi.Controllers;

namespace VsiriusApi.DependencyInjection
{
    public static class Stuller
    {
        public static void AddStullerServices(this IServiceCollection services, IConfiguration configuration)
        {
            

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            services.AddScoped<IMongoContext, MongoContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IKeyService, KeyService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IPullStuller, PullStuller>();
            services.AddScoped<IMapStullerDBToVsiriusDB, MapStullerDBToVsiriusDB>();
            services.AddScoped<IProductAI, StullerDataApi.Utilities.PullStuller.ProductAI>();

            services.AddScoped<IExportDataVsirius, ExportDataVsirius>();
            services.AddScoped<ICallAPIVSiriusWeb, CallAPIVSiriusWeb>();
            services.AddScoped<IUtilitiesStorageBlobs, UtilitiesStorageBlobs>();

            services.AddUpdateJobs();
            services.AddJob<Stuller_PullDataStuller>();
            services.AddTransient<PullStullerController>();
            services.AddJob<Stuller_MapStullerToPIM>();
            services.AddTransient<MapStullerDBToVsiriusDBController>();

        }
    }
}
