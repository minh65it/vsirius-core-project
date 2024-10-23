using SkuMappingAPI._Repositories.Interfaces;
using SkuMappingAPI.Data;
using SkuMappingAPI.Models;

namespace SkuMappingAPI._Repositories.Repositories
{
    public class MappingSkuRepository : Repository<SKUInfo>, IMappingSkuRepository
    {
        public MappingSkuRepository(MappingSkuDbContext context) : base(context)
        {
        }
    }
}
