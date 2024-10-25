using ExcludedProductAPI._Repositories.Interfaces;
using ExcludedProductAPI._Repositories.Repositories;
using ExcludedProductAPI._Services.Interfaces;
using ExcludedProductAPI._Services.Repositories;

namespace VsiriusApi.DependencyInjection
{
    public static class Excluded
    {
        public static void AddExcludedServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IExcludedProductRepository, ExcludedProductRepository>();
            services.AddScoped<IExcludedProductService, ExcludedProductService>();
        }
    }
}
