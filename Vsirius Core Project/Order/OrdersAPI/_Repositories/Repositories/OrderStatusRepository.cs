using OrdersAPI._Repositories.Interfaces;
using OrdersAPI.Data;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(DataContext context) : base(context)
        {
        }
    }
}
