using ProductAiApi._JobServices.Interfaces;
using ProductAiApi._JobServices.JobServices;
using ProductAiApi._Repositories.Interfaces;
using ProductAiApi._Repositories.Repositories;
using ProductAiApi._Services.Interfaces;
using ProductAiApi._Services.Services;
using ProductAiApi.Controllers;
using ProductAiApi.Jobs;
using QuartzInfrastructure;

namespace VsiriusApi.DependencyInjection
{
    public static class ProductAI
    {
        public static void AddProductAIServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddTransient<TriggerController>();
            services.AddTransient<IProductAIRepository, ProductAIRepository>();
            services.AddScoped<ICreateProductAiFromPimService, CreateProductAiFromPimService>();
            services.AddScoped<IProductAIService, ProductAIService>();

            services.AddJob<ProductAI_CreateProductAiFromPim>();
        }
    }
}
