using AmazonIntegrationDataApi.Data;
using ExcludedProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using SkuMappingAPI.Data;
using VsiriusDataModel.DataContext;
using VsiriusSystem.Data;

namespace VsiriusApi.Configurations
{
    public static class MigrationsConfigure
    {
        public static void ApplyMigrationVsiriusContext(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<VsiriusContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }

        public static void ApplyMigrationSKU(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<MappingSkuDbContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }

        public static void ApplyMigrationDataContext(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<DataContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }

        public static void ApplyMigrationModifiedProductContext(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ModifiedProductContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }

        public static void ApplyMigrationAmazon(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<DBContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }
    }
}
