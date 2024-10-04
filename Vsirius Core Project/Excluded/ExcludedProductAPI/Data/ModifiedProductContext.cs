using ExcludedProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcludedProductAPI.Data
{
    public class ModifiedProductContext: DbContext
    {
        public ModifiedProductContext(DbContextOptions<ModifiedProductContext> options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExcludedProduct>()
                .HasKey(e => new { e.MarketplaceSKU, e.Marketplace });
        }

        public DbSet<ExcludedProduct> ExcludedProducts { get; set; }
    }
}
