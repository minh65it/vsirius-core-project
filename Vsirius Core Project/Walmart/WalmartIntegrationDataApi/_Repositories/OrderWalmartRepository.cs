using WalmartIntegrationDataApi._Repositories.Interfaces;

namespace WalmartIntegrationDataApi._Repositories
{
    public class OrderWalmartRepository : MongoBaseRepository, IOrderWalmartRepository
    {
        public OrderWalmartRepository(IConfiguration configuration, string dbName = "Walmart") : base(dbName, configuration)
        {
        }
    }
}
