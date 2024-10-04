using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusOrderDetailRepository : Repository<VsiriusOrderDetails>, IVsiriusOrderDetailRepository
    {
        public VsiriusOrderDetailRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
