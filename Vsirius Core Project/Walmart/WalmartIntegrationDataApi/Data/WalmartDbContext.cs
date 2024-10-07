using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi.Data
{
    #pragma warning disable CS1591
    public class WalmartDbContext : DbContext
    {
        public WalmartDbContext(DbContextOptions<WalmartDbContext> options) : base(options)
        {
        }
        public DbSet<Walmart> Walmarts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            //builder.Properties<DateOnly>()
            //    .HaveConversion<DateOnlyConverter>()
            //    .HaveColumnType("date");

            builder.Properties<DateOnly?>()
                .HaveConversion<NullableDateOnlyConverter>()
                .HaveColumnType("date");
        }
    }

    public class NullableDateOnlyConverter : ValueConverter<DateOnly?, DateTime?>
    {
        public NullableDateOnlyConverter() : base(
            d => d == null
                ? null
                : new DateTime?(d.Value.ToDateTime(TimeOnly.MinValue)),
            d => d == null
                ? null
                : new DateOnly?(DateOnly.FromDateTime(d.Value)))
        { }
    }
}
