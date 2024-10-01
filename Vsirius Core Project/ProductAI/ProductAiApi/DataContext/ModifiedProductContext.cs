using Microsoft.EntityFrameworkCore;
using ProductAiApi.Models;

namespace ProductAiApi.DataContext
{
    public class ModifiedProductContext : DbContext
    {
        public ModifiedProductContext(DbContextOptions<ModifiedProductContext> options) : base(options)
        {
        }

        public DbSet<ProductAI> ProductAIs { get; set; }
    }
}
