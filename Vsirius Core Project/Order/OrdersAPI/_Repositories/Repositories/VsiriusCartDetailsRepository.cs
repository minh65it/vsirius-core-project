
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI.Data;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusCartDetailsRepository : Repository<VsiriusCartDetails>, IVsiriusCartDetailsRepository
    {
        public VsiriusCartDetailsRepository(DataContext context) : base(context)
        {
        }
    }
}
