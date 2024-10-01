
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI.Data;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusOrderRepository : Repository<VsiriusOrders>, IVsiriusOrderRepository
    {
        public VsiriusOrderRepository(DataContext context) : base(context)
        {
        }
    }
}
