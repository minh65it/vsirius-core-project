
using Microsoft.EntityFrameworkCore;
using SkuMappingAPI.Data;
using VsiriusDataModel.DataContext;

namespace VsiriusApi.DependencyInjection
{
    public static class ContextConfigure
    {
        public static void AddContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<VsiriusContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("VsiriusWebConnection")));

            services.AddDbContext<MappingSkuDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SKUConnection")));

        }
    }
}
