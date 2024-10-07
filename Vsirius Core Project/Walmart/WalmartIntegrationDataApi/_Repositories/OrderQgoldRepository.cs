using WalmartIntegrationDataApi._Repositories.Interfaces;

namespace WalmartIntegrationDataApi._Repositories
{
    public class OrderQgoldRepository : MongoBaseRepository, IOrderQgoldRepository
    {
        public OrderQgoldRepository(IConfiguration configuration,string dbName = "Qgold") : base(dbName, configuration)
        {
        }
    }
}
