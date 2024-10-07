using Microsoft.EntityFrameworkCore;
using QgoldIntegrationDataApi.Models;

namespace QgoldIntegrationDataApi.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }
        public DbSet<QgoldProduct> QgoldCsvDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QgoldProduct>()
                .HasKey(u => u.Item);
        }
    }
}
