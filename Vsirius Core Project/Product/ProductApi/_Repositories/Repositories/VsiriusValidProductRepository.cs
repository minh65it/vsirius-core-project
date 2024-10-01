using ProductApi._Repositories.Interfaces;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusValidProductRepository : Repository<VsiriusValidProduct>, IVsiriusValidProductRepository
    {
        public VsiriusValidProductRepository(DataContext context) : base(context)
        {
        }
    }
}
