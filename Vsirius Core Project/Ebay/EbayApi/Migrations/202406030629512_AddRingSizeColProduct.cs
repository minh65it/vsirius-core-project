namespace EbayApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRingSizeColProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "RingSize", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "RingSize");
        }
    }
}
