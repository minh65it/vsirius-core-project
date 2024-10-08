using EbayCoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EbayCoreApi.DataContext
{
    public class EbayContext : DbContext
    {
        public EbayContext(DbContextOptions<EbayContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
            .HasKey(p => p.Sku);

            modelBuilder.Entity<Order>()
           .HasKey(o => o.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => od.OrderDetailId);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.Price)
                      .HasColumnType("decimal(18,2)"); // Chỉ định kiểu lưu trữ
            });
        }
    }
}