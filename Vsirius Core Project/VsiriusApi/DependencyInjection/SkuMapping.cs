using Microsoft.EntityFrameworkCore;
using SkuMappingAPI._Repositories.Interfaces;
using SkuMappingAPI._Repositories.Repositories;
using SkuMappingAPI._Services.Services;
using SkuMappingAPI.Common;
using SkuMappingAPI.Data;
using SkuMappingAPI.Services.Interfaces;

namespace VsiriusApi.DependencyInjection
{
    public static class SkuMapping
    {
        public static void AddSKUServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddTransient<Result>();
            services.AddTransient<IMappingSkuRepository, MappingSkuRepository>();
            services.AddScoped<IMappingSkuService, MappingSkuService>();

            ApplyMigration(services.BuildServiceProvider());
        }

        static void ApplyMigration(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<MappingSkuDbContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }
    }
}
