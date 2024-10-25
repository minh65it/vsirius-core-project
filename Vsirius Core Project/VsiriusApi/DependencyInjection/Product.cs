using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProductApi._Repositories.Interfaces;
using ProductApi._Repositories.Repositories;
using ProductApi._Services.Interfaces;
using ProductApi._Services.Services;
using ProductApi.Controllers;
using ProductApi.Helpers.httpClient;
using ProductApi.Helpers.Utilities;
using ProductApi.Jobs;
using QuartzInfrastructure;
using VsiriusDataModel.DataContext;

namespace VsiriusApi.DependencyInjection
{
    public static class Product
    {
        public static void AddProductServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IVsiriusProductRepository, VsiriusProductRepository>();
            services.AddTransient<IVsiriusFilterRepository, VsiriusFilterRepository>();
            services.AddTransient<IVsiriusValidProductRepository, VsiriusValidProductRepository>();
            services.AddTransient<IVsiriusHomeProductRepository, VsiriusHomeProductRepository>();

            services.AddScoped<IElasticSearchProductService, ElasticSearchProductService>();
            services.AddScoped<IMapVSiriusDBToVSiriusWebDB, MapVSiriusDBToVSiriusWebDB>();
            services.AddScoped<IVsiriusHomeProductService, VsiriusHomeProductService>();
            services.AddScoped<IVsiriusTopProductService, VsiriusTopProductService>();
            services.AddScoped<IVSiriusProductApiClient, VSiriusProductApiClient>();
            services.AddScoped<IVsiriusProductService, VsiriusProductService>();
            services.AddScoped<IVsiriusFilterService, VsiriusFilterService>();
            services.AddScoped<IProductAIClient, ProductAIClient>();
            services.AddScoped<IAzureStorage, AzureStorage>();

            services.AddScoped<ILogService, LogService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped<ValidationVsirius>();
            /*
                        //Update Jobs
                        services.AddUpdateJobs();
                        //Job PullQgold
                        services.AddTransient<TriggerController>();
                        services.AddJob<VSiriusWeb_MapPIMToVSiriusWebDB>();
                        services.AddJob<VSiriusWeb_UpdateFilter>();
            */
            //Excel
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
        }
    }
}
