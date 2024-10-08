using EtsyIntegrationDataAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EtsyIntegrationDataAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Product
            modelBuilder.Entity<ImageProduct>().HasKey(s => new { s.item_sku, s.listing_image_id });
            modelBuilder.Entity<TagProduct>().HasKey(s => new { s.item_sku, s.tag });
            modelBuilder.Entity<MaterialProduct>().HasKey(s => new { s.item_sku, s.material });
            modelBuilder.Entity<StyleProduct>().HasKey(s => new { s.item_sku, s.style });
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
        }

        //Product
        public DbSet<AuthenEtsy> AuthenEtsy { get; set; }
        public DbSet<ProductEtsy> ProductEtsy { get; set; }
        public DbSet<ImageProduct> ImageProduct { get; set; }
        public DbSet<TagProduct> TagProduct { get; set; }
        public DbSet<MaterialProduct> MaterialProduct { get; set; }
        public DbSet<StyleProduct> StyleProduct { get; set; }

        //Order
        public DbSet<Order> Order { get; set; }
        public DbSet<Shipment> Shipment { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Refund> Refund { get; set; }
        public DbSet<Variation> Variation { get; set; }
        public DbSet<ProductDatum> ProductDatum { get; set; }
        public DbSet<Taxonomy> Taxonomy { get; set; }
        public DbSet<PropertiesByTaxonomy> PropertiesByTaxonomy { get; set; }
        public DbSet<PropertiesProduct> PropertiesProduct { get; set; }
    }
}
