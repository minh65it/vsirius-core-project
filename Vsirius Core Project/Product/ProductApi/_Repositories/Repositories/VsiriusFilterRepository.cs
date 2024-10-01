using ProductApi._Repositories.Interfaces;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusFilterRepository : Repository<VsiriusFilter>, IVsiriusFilterRepository
    {
        public VsiriusFilterRepository(DataContext context) : base(context)
        {
        }
    }
}
