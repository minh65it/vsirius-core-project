namespace EbayApi.Migrations
{
    using EbayApi.DataContext;
    using System.Data.Entity.Migrations;

    internal sealed class MigraConfig : DbMigrationsConfiguration<EbayApi.DataContext.EbayContext>
    {
        public MigraConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            CommandTimeout = 60000;
        }

        protected override void Seed(EbayContext context)
        {
        }
    }
}
