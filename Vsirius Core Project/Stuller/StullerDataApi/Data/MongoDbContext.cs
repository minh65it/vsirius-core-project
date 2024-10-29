using MongoDB.Driver;

namespace StullerDataApi.Data
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; }
    }

    public class MongoContext : IMongoContext
    {
        public MongoContext(IMongoDbSettings connectionSetting)
        {
            var client = new MongoClient(connectionSetting.MongoDB);
            Database = client.GetDatabase(connectionSetting.StullerDatabase);
        }

        public IMongoDatabase Database { get; }
    }

}
