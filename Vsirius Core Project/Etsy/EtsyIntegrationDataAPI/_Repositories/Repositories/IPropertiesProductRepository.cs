using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI._Repositories.Repositories
{
    public class PropertiesProductRepository : Repository<PropertiesProduct>, IPropertiesProductRepository
    {
        public PropertiesProductRepository(DataContext context) : base(context)
        {
        }
    }
}
