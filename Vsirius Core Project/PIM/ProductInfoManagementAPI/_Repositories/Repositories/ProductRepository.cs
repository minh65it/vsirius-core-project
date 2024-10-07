using PimApi._Repositories.Interfaces;
using PimApi.Context;
using PimApi.Models;

namespace PimApi._Repositories.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context) : base(context)
        {
        }
    }
}
