using PimApi._Repositories.Interfaces;

namespace PimApi._Repositories.Repositories
{
    public class WalmartMongoRepository : MongoBaseRepository, IWalmartMongoRepository
    {
        public WalmartMongoRepository(IConfiguration configuration, string dbName = "Walmart") : base(dbName, configuration)
        {
        }
    }
}
