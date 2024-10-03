using ProductApi._Repositories.Interfaces;
using VsiriusDataModel.DataModel;
using VsiriusDataModel.DataContext;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusValidProductRepository : Repository<VsiriusValidProduct>, IVsiriusValidProductRepository
    {
        public VsiriusValidProductRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
