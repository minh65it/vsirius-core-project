using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI._Repositories.Repositories
{
    public class PropertiesByTaxonomyRepository : Repository<PropertiesByTaxonomy>, IPropertiesByTaxonomyRepository
    {
        public PropertiesByTaxonomyRepository(DataContext context) : base(context)
        {
        }
    }
}
