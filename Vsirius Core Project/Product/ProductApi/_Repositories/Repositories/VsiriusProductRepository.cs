using ProductApi._Repositories.Interfaces;
using VsiriusDataModel.DataModel;
using VsiriusDataModel.DataContext;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusProductRepository : Repository<VsiriusProduct>, IVsiriusProductRepository
    {
        public VsiriusProductRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
