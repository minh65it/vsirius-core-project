using Microsoft.EntityFrameworkCore;
using VsiriusSystem.Models;

namespace VsiriusSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(entity =>
            {
                entity.HasKey(e => e.LogId);
            });
        }
        //entities
        public DbSet<Log> Logs { get; set; }
        public DbSet<JobInfo> JobInfos { get; set; }
    }
}
