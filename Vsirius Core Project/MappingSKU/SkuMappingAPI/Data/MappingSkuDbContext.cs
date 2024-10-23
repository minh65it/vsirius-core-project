using Microsoft.EntityFrameworkCore;
using SkuMappingAPI.Models;

namespace SkuMappingAPI.Data
{
    public class MappingSkuDbContext : DbContext
    {
        public MappingSkuDbContext(DbContextOptions<MappingSkuDbContext> context) : base(context)
        {

        }
        public DbSet<SKUInfo> SKUInfos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
