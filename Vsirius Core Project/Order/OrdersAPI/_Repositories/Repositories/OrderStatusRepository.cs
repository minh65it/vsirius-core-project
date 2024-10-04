using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
