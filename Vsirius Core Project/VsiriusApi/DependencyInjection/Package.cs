using PackageApi._Repositories.IRepositories;
using PackageApi._Repositories.Repositories;
using PackageApi._Services.IServices;
using PackageApi._Services.Services;
using PackageApi.Utilities;

namespace VsiriusApi.DependencyInjection
{
    public static class Package
    {
        public static void AddPackageServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IClaimRepository, ClaimRepository>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<PackageApi.Utilities.Blob.IAzureStorage, AzureStorage>();
        }
    }
}
