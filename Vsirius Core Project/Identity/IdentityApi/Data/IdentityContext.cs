using IdentityApi.Models;
using IdentityApi.Models.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityApi.Data
{
    public class IdentityContext : IdentityDbContext<VsiriusUser>
    {
        public IdentityContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //substring ASPNET
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = item.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    item.SetTableName(tableName.Substring(6));
                }
            }
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<RolesEmployee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<RoleClaimsEmployee>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<State>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<UserRolesEmployee>(entity =>
            {
                entity.HasKey(e => e.Email);
            });
            modelBuilder.Entity<VsiriusUser>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
        //Entities Employee
        public DbSet<Employee> Employee { get; set; }
        public DbSet<RolesEmployee> RolesEmployee { get; set; }
        public DbSet<RoleClaimsEmployee> RoleClaimsEmployee { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<UserRolesEmployee> UserRolesEmployee { get; set; }
        public DbSet<VsiriusUser> User { get; set; }
    }
}
