using PimApi._Repositories.Interfaces;

namespace PimApi._Repositories.Repositories
{
    public class AmazonMongoRepository : MongoBaseRepository, IAmazonMongoRepository
    {
        public AmazonMongoRepository(IConfiguration configuration, string dbName = "Amazon") : base(dbName, configuration)
        {
        }
    }
}
