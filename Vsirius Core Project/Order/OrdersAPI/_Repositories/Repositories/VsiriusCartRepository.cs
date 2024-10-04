
using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusCartRepository : Repository<VsiriusCart>, IVsiriusCartRepository
    {
        public VsiriusCartRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
