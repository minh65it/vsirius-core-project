using QuartzInfrastructure;
using StullerOrderProcessAPI._JobServices.Interfaces;
using StullerOrderProcessAPI._JobServices.JobService;
using StullerOrderProcessAPI._Repositories.Interfaces;
using StullerOrderProcessAPI._Repositories.Repositories;
using StullerOrderProcessAPI._Services.Interfaces;
using StullerOrderProcessAPI._Services.Services;    
using StullerOrderProcessAPI.Controllers;
using StullerOrderProcessAPI.Jobs;

namespace VsiriusApi.DependencyInjection
{
    public static class StullerOrder
    {
        public static void AddStullerOrderServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IStullerOrderRepository, StullerOrderRepository>();
            services.AddScoped<IOrderStullerService, OrderStullerService>();

            services.AddScoped<IGetStullerOrderService, GetStullerOrderService>();

            services.AddTransient<TriggerController>();
            services.AddUpdateJobs();
            services.AddJob<Stuller_GetOrder>();
        }
    }
}
