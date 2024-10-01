using ProductApi._Repositories.Interfaces;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusProductRepository : Repository<VsiriusProduct>, IVsiriusProductRepository
    {
        public VsiriusProductRepository(DataContext context) : base(context)
        {
        }
    }
}
