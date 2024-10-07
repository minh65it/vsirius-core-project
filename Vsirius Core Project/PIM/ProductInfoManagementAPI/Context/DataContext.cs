using Microsoft.EntityFrameworkCore;
using PimApi.Models;

namespace PimApi.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.SetCommandTimeout(TimeSpan.FromHours(3));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
                {
                    entity.HasKey(e => e.item_sku);
                });
            modelBuilder.Entity<Order>()
                .HasKey(u => new { u.OrderId, u.ItemSku, u.Marketplace });
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Variation> Variations { get; set; }
    }
}
