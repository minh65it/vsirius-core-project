using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusProductRepository : Repository<VsiriusProduct>, IVsiriusProductRepository
    {
        public VsiriusProductRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
