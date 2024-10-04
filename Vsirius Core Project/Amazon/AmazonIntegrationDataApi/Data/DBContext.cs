using AmazonIntegrationDataApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AmazonIntegrationDataApi.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            this.Database.SetCommandTimeout(TimeSpan.FromHours(3));
        }
        //public DbSet<AmazonJewelryDataFeedItem> AmazonJewelryDataFeedItems { get; set; }
        public DbSet<AmazonJewelryDataFeedItemV3> AmazonJewelryDataFeedItems { get; set; }
        public DbSet<ValidValueVsiriusModel> ValidValueVsiriusModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AmazonJewelryDataFeedItemV3>()
                .HasKey(u => u.item_sku);

            modelBuilder.Entity<ValidValueVsiriusModel>()
                .HasKey(u => u.Id);
        }

    }

}

