using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataModel;

namespace VsiriusDataModel.DataContext
{
    public class VsiriusContext : IdentityDbContext<VsiriusUser>
    {
        public VsiriusContext(DbContextOptions<VsiriusContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            //Entities Identity
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = item.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    item.SetTableName(tableName.Substring(6));
                }
            }


            modelBuilder.Entity<VsiriusProduct>(entity =>
            {
                entity.HasKey(e => e.item_sku);
                entity.HasIndex(r => r.IsDelete).HasFilter("IsDelete = 0");
                entity.Property(e => e.standard_price).HasColumnType("decimal(18,2)");
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
            modelBuilder.Entity<VsiriusCart>(entity =>
            {
                entity.HasKey(e => e.CartId);
                entity.Property(c => c.TotalPrice).HasColumnType("decimal(18, 2)");
                entity.Property(c => c.Tax).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<VsiriusCartDetails>(entity =>
            {
                entity.HasKey(e => e.CartDetailId);
                entity.HasOne(e => e.VsiriusCart)
                     .WithMany(d => d.VsiriusCartDetails)
                     .HasForeignKey(e => e.CartId);
                entity.Property(c => c.Price).HasColumnType("decimal(18, 2)");
                entity.Property(c => c.PriceSale).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<VsiriusOrders>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(c => c.TotalPrice).HasColumnType("decimal(18, 2)");
                entity.Property(c => c.Tax).HasColumnType("decimal(18, 2)");


            });
            modelBuilder.Entity<VsiriusOrderDetails>(entity =>
            {
                entity.HasKey(e => e.OrderDetailId);
                entity.Property(c => c.Price).HasColumnType("decimal(18, 2)");

            });

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
                entity.HasKey(e => e.Id);
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.HasKey(e => e.IdGuild);
            });

            modelBuilder.Entity<Favourites>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<DrivenHtmlPages>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<Notifications>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<NotificationDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<NotificationMain>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
        //Entities Product
        public DbSet<VsiriusProduct> VsiriusProduct { get; set; }
        public DbSet<VsiriusHomeProduct> VsiriusHomeProduct { get; set; }
        public DbSet<VsiriusFilter> VsiriusFilter { get; set; }
        public DbSet<VsiriusValidProduct> VsiriusValidProduct { get; set; }
        public DbSet<VsiriusCart> VsiriusCart { get; set; }
        public DbSet<VsiriusCartDetails> VsiriusCartDetails { get; set; }
        public DbSet<VsiriusOrders> VsiriusOrders { get; set; }
        public DbSet<VsiriusOrderDetails> VsiriusOrderDetails { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        //Entities
        //Entities Employee
        public DbSet<Employee> Employee { get; set; }
        public DbSet<RolesEmployee> RolesEmployee { get; set; }
        public DbSet<RoleClaimsEmployee> RoleClaimsEmployee { get; set; }
        public DbSet<UserRolesEmployee> UserRolesEmployee { get; set; }

        //CSR 
        public DbSet<Favourites> Favourites { get; set; }
        public DbSet<DrivenHtmlPages> DrivenHtmlPages { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<NotificationDetail> NotificationDetails { get; set; }
        public DbSet<NotificationMain> NotificationMains { get; set; }
        public DbSet<SecretIdentityUserInfo> SecretIdentityUserInfo { get; set; }
        public DbSet<PageView> PageView { get; set; }
        public DbSet<ConfigJson> ConfigJson { get; set; }

    }
}
