namespace StullerDataApi.Data
{
    public class MongoDbSettings : IMongoDbSettings
    {
        public string CollectionName { get; set; }
        public string MongoDB { get; set; }
        public string StullerDatabase { get; set; }
    }

    public interface IMongoDbSettings
    {
        string CollectionName { get; set; }
        string MongoDB { get; set; }
        string StullerDatabase { get; set; }
    }

}
