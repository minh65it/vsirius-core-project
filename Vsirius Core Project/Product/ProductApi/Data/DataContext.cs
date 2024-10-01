using Microsoft.EntityFrameworkCore;
using ProductApi.Models;

namespace ProductApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VsiriusProduct>(entity =>
            {
                entity.HasKey(e => e.item_sku);
            });
            modelBuilder.Entity<VsiriusFilter>(entity =>
            {
                entity.HasKey(e => e.FilterId);
            });
            modelBuilder.Entity<VsiriusHomeProduct>(entity =>
            {
                entity.HasKey(e => e.item_sku);
            });
            modelBuilder.Entity<VsiriusValidProduct>(entity =>
            {
                entity.HasKey(e => e.FilterId);
            });
        }
        //entities
        public DbSet<VsiriusProduct> VsiriusProduct { get; set; }
        public DbSet<VsiriusHomeProduct> VsiriusHomeProduct { get; set; }
        public DbSet<VsiriusFilter> VsiriusFilter { get; set; }
        public DbSet<VsiriusValidProduct> VsiriusValidProduct { get; set; }
    }
}
