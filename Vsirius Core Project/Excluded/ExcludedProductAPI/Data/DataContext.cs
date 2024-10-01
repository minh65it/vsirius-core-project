using ExcludedProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcludedProductAPI.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }

        public DbSet<ExcludedProduct> ExcludedProducts { get; set; }
    }
}
