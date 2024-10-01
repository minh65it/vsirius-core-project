
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI.Data;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusCartRepository : Repository<VsiriusCart>, IVsiriusCartRepository
    {
        public VsiriusCartRepository(DataContext context) : base(context)
        {
        }
    }
}
