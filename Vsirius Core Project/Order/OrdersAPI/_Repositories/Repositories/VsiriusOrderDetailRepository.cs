using OrdersAPI._Repositories.Interfaces;
using OrdersAPI.Data;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusOrderDetailRepository : Repository<VsiriusOrderDetails>, IVsiriusOrderDetailRepository
    {
        public VsiriusOrderDetailRepository(DataContext context) : base(context)
        {
        }
    }
}
