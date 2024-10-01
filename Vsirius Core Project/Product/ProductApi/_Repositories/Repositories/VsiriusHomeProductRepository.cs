using ProductApi._Repositories.Interfaces;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusHomeProductRepository : Repository<VsiriusHomeProduct>, IVsiriusHomeProductRepository
    {
        public VsiriusHomeProductRepository(DataContext context) : base(context)
        {
        }
    }
}
