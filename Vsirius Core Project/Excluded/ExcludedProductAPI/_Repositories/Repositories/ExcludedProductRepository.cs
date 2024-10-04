using ExcludedProductAPI._Repositories.Interfaces;
using ExcludedProductAPI.Data;
using ExcludedProductAPI.Models;

namespace ExcludedProductAPI._Repositories.Repositories
{
    public class ExcludedProductRepository : Repository<ExcludedProduct>, IExcludedProductRepository
    {
        public ExcludedProductRepository(ModifiedProductContext context) : base(context)
        {
        }
    }
}
