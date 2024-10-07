namespace OrderProcessApi.Data
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string AmazonSellerCollectionName { get; set; }
        public string AmazonSellerDatabaseName { get; set; }
        public string QgoldCollectionName { get; set; }
        public string QgoldDatabaseName { get; set; }
        public string QgoldMarketplace { get; set; }
    }

    public interface IMongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string AmazonSellerCollectionName { get; set; }
        public string AmazonSellerDatabaseName { get; set; }
        public string QgoldCollectionName { get; set; }
        public string QgoldDatabaseName { get; set; }
        public string QgoldMarketplace { get; set; }
    }
}
