using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI._Repositories.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DataContext context) : base(context)
        {
        }
    }
}
