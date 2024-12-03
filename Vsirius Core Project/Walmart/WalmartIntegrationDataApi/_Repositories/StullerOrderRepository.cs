

using WalmartIntegrationDataApi._Repositories.Interfaces;

namespace WalmartIntegrationDataApi._Repositories.Repositories
{
    public class StullerOrderRepository : MongoBaseRepository, IStullerOrderRepository
    {

        public StullerOrderRepository(IConfiguration configuration, string dbName = "Stuller") : base(dbName, configuration)
        {
        }
    }
}
