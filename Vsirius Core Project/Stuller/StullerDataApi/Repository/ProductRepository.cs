using StullerDataApi.Data;
using StullerDataApi.IRepository;
using StullerApiClient.Models;

namespace StullerDataApi.Repository
{
    public class ProductRepository : MongoDBRepository<Product>, IProductRepository
    {
        public ProductRepository(IMongoContext context) : base(context)
        {
        }
    }
}
