
using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusCartDetailsRepository : Repository<VsiriusCartDetails>, IVsiriusCartDetailsRepository
    {
        public VsiriusCartDetailsRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
