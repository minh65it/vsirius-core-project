using PimApi._Repositories.Interfaces;
using PimApi.Context;
using PimApi.Models;

namespace PimApi._Repositories.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context)
        {
        }
    }
}
