using AmazonIntegrationDataApi.Data;
using ExcludedProductAPI.Data;
using Microsoft.EntityFrameworkCore;
using SkuMappingAPI.Data;
using VsiriusDataModel.DataContext;
using VsiriusSystem.Data;
using WalmartIntegrationDataApi.Data;

namespace VsiriusApi.Configurations
{
    public static class MigrationsConfigure
    {
        public static void ApplyMigrationWalmart(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<WalmartDbContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }
        public static void ApplyMigrationQgoldApi(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<QgoldIntegrationDataApi.Data.DBContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }

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

        public static void ApplyMigrationSystem(this IServiceCollection services)
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

        public static void ApplyMigrationExcluded(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ExcludedProductAPI.Data.ModifiedProductContext>();

                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
        }

        public static void ApplyMigrationProductAI(this IServiceCollection services)
        {
            var svProvider = services.BuildServiceProvider();
            using (var scope = svProvider.CreateScope())
            {
                var _db = scope.ServiceProvider.GetRequiredService<ProductAiApi.DataContext.ModifiedProductContext>();

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
