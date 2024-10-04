using AmazonIntegrationDataApi._Repositories.Interfaces;

namespace AmazonIntegrationDataApi._Repositories.Repositories
{
    public class AmazonMongoRepository : CoreMongoDB, IAmazonMongoRepository
    {
        public AmazonMongoRepository(string dbName = "Amazon", IConfiguration configuration = null) : base(dbName, configuration)
        {
        }
    }
}
