using QgoldIntegrationDataApi._JobServices.Interfaces;
using QgoldIntegrationDataApi._JobServices.JobServices;
using QgoldIntegrationDataApi._Repositories;
using QgoldIntegrationDataApi._Repositories.Interfaces;
using QgoldIntegrationDataApi._Repositories.Repositories;
using QgoldIntegrationDataApi._Services.Interfaces;
using QgoldIntegrationDataApi._Services.Services;
using QgoldIntegrationDataApi.Controllers;
using QgoldIntegrationDataApi.Jobs;
using QuartzInfrastructure;

namespace VsiriusApi.DependencyInjection
{
    public static class Qgold
    {
        public static void AddQgoldServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddUpdateJobs();
            //services.AddTransient<QgoldProcessTriggerController>();
            //services.AddJob<Qgold_PullProduct>();
            //services.AddJob<Qgold_MappingQgoldToPIM>();

            services.AddTransient<IRepositoryAccessor, RepositoryAccessor>();
            services.AddScoped<IQgoldCsvDataService, QgoldCsvDataService>();
            services.AddScoped<IMappingQgoldToVsiriusService, MappingQgoldToVsiriusService>();
            services.AddScoped<IKeyService, KeyService>();
            services.AddScoped<IAzureStorage, AzureStorage>();
            services.AddScoped<IPullQgoldTriggerService, PullQgoldTriggerService>();
        }
        
    }
}
