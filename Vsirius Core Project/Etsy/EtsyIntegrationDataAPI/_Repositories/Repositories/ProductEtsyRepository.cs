using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI._Repositories.Repositories
{
    public class ProductEtsyRepository : Repository<ProductEtsy>, IProductEtsyRepository
    {
        public ProductEtsyRepository(DataContext context) : base(context)
        {
        }
    }
}
