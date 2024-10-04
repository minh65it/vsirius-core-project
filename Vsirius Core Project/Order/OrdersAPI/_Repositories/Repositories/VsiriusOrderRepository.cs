
using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusOrderRepository : Repository<VsiriusOrders>, IVsiriusOrderRepository
    {
        public VsiriusOrderRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
