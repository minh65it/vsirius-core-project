
using Microsoft.EntityFrameworkCore;
using SkuMappingAPI.Data;
using VsiriusDataModel.DataContext;
using VsiriusSystem.Data;

namespace VsiriusApi.DependencyInjection
{
    public static class ContextConfigure
    {
        public static void AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SystemConnection")));

            services.AddDbContext<VsiriusContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("VsiriusWebConnection")));

            services.AddDbContext<MappingSkuDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SKUConnection")));

            

        }
    }
}
