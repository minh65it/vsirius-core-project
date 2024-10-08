using eBay.Service.Core.Soap;
using EbayApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;


namespace EbayApi.DataContext
{
    public class EbayContext : DbContext
    {
        public EbayContext() : base("name=EbayContext")
        {           
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>()
            .HasKey(p => p.Sku);

            modelBuilder.Entity<Order>()
           .HasKey(o => o.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => od.OrderDetailId);

            modelBuilder.Entity<OrderDetail>()
                .HasRequired(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId);
        }   

        public void UpdateDatabase()
        {
            var migrator = new DbMigrator(new Migrations.MigraConfig());
            foreach (var migration in migrator.GetPendingMigrations())
            {
                migrator.Update(migration);
            }
            if (ConfigurationManager.AppSettings["Enviroment"] == "Development")
            {
                SeedData(this);
            }           
        }

        private void SeedData(EbayContext context)
        {
            if (!context.Products.Any())
            {
                context.Products.AddOrUpdate(
                    new Product
                    {
                        Sku = "Q-QBR057-20",
                        Tittle = "VSirius Sterling Silver 3.2mm Box Chain QQBR057-20",
                        Price = 85.45m,
                        Brand = "VSirius",
                        CreatedBy = "admin",
                        CreatedDate = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedDate = DateTime.Now,
                        Description =
                        "Material: Primary - Purity:925<br />" +
                        "Finish:Polished<br />" +
                        "Length of Item:20 in<br />" +
                        "Chain Length:20 in<br />" +
                        "Chain Type:Box<br />" +
                        "Chain Width:3.2 mm<br />" +
                        "Clasp /Connector:Lobster<br />" +
                        "Coating:Anti-tarnish<br />" +
                        "Feature:Solid<br />" +
                        "Manufacturing Process:Casted<br />" +
                        "Material: Primary:Sterling Silver<br />" +
                        "Item Weight U/M:gm<br />" +
                        "Width of Item:3.2 mm<br />" +
                        "Product Type:Jewelry<br />" +
                        "Jewelry Type:Necklaces<br />" +
                        "Sold By Unit:Each<br />" +
                        "Necklace Type:Chain Styles<br />" +
                        "Gender:Women's<br />" +
                        "Texture:Diamond-cut<br />" +
                        "Material: Primary - Color:White",
                        MainImageUrl = "https://stvsiriuseastusdev1.blob.core.windows.net/public/Q-QBR057-20%5C1.jpg",
                        Image1Url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/Q-QBR057-20%5C2.jpg",
                        Image2Url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/Q-QBR057-20%5C3.jpg",
                        Image3Url = "https://stvsiriuseastusdev1.blob.core.windows.net/public/Q-QBR057-20%5C4.jpg",
                        IsDeleted = false,
                        Category = "Necklaces & Pendants",
                        Origin = "ITALY",
                        ChainType = "Box",
                        ItemColor = "White",
                        Material = "Sterling Silver",
                        ItemLength = "20 in",
                        ProductType = "Jewelry",
                        ItemStyle = "Necklaces",
                        PrimaryStoneShape = "",
                        ItemSize = "",
                        ItemWeight = "",
                        PrimaryStoneSize = "",
                        PrimaryStoneType = "",
                        Quantity = 1,

                    }
                );

                context.Orders.AddOrUpdate(
                    new Order()
                    {
                        Address1 = "630 Church Ln",
                        Address2 = "",
                        City = "Yeadon",
                        State = "PA",
                        Country = "US",
                        ZipCode = "19050",
                        CustomerEmail = "admin@vsirius.com",
                        CustomerId = "CTEST",
                        CustomerName = "Vsirius Company",
                        CustomerPhone = "215-500-2667",
                        OrderDate = DateTime.Now,
                        OrderId = "OTEST1",
                        StatusOrder = OrderStatusCodeType.Shipped.ToString(),
                        TotalPrice = 100,
                        TrackingNo = "9123141412",
                        OrderDetails = new[] { 
                        new OrderDetail()
                        {
                            OrderId = "OTEST1",
                            OrderDetailId = Guid.NewGuid().ToString(),
                            Price = 20,
                            Quantity = 2,
                            Sku = "Q-1E"
                        },
                        new OrderDetail()
                        {
                            OrderId = "OTEST2",
                            OrderDetailId = Guid.NewGuid().ToString(),
                            Price = 60,
                            Quantity = 1,
                            Sku = "Q-8U"
                        }
                        },
                        IsSubmit = true,
                    }
                    );
                context.SaveChanges();
            }
        }
    }
}