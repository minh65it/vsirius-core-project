namespace EbayApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        OrderDetailId = c.String(nullable: false, maxLength: 128),
                        OrderId = c.String(nullable: false, maxLength: 128),
                        Sku = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OrderDetailId)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.String(nullable: false, maxLength: 128),
                        CustomerId = c.String(),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        CustomerEmail = c.String(),
                        Address1 = c.String(),
                        Address2 = c.String(),
                        TrackingNo = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        ZipCode = c.String(),
                        TotalPrice = c.Double(nullable: false),
                        StatusOrder = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        IsSubmit = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Sku = c.String(nullable: false, maxLength: 50),
                        Tittle = c.String(nullable: false, maxLength: 80),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(nullable: false, maxLength: 800),
                        Brand = c.String(nullable: false, maxLength: 100),
                        Category = c.String(nullable: false, maxLength: 100),
                        ProductType = c.String(nullable: false, maxLength: 100),
                        ItemStyle = c.String(nullable: false, maxLength: 100),
                        Material = c.String(nullable: false, maxLength: 100),
                        MainImageUrl = c.String(nullable: false, maxLength: 4000),
                        Image1Url = c.String(maxLength: 4000),
                        Image2Url = c.String(maxLength: 4000),
                        Image3Url = c.String(maxLength: 4000),
                        Image4Url = c.String(maxLength: 4000),
                        Image5Url = c.String(maxLength: 4000),
                        Image6Url = c.String(maxLength: 4000),
                        Image7Url = c.String(maxLength: 4000),
                        Image8Url = c.String(maxLength: 4000),
                        Image9Url = c.String(maxLength: 4000),
                        ItemLength = c.String(maxLength: 100),
                        Origin = c.String(maxLength: 100),
                        ItemSize = c.String(maxLength: 100),
                        ItemColor = c.String(maxLength: 100),
                        PrimaryStoneType = c.String(maxLength: 100),
                        PrimaryStoneSize = c.String(maxLength: 100),
                        PrimaryStoneShape = c.String(maxLength: 100),
                        ItemWeight = c.String(maxLength: 100),
                        ChainType = c.String(maxLength: 100),
                        Note = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedBy = c.String(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedBy = c.String(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Sku)
                .Index(t => t.IsDeleted);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropIndex("dbo.Products", new[] { "IsDeleted" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
        }
    }
}
