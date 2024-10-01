using OrdersAPI._Repositories.Interfaces;
using OrdersAPI.Data;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class VsiriusProductRepository : Repository<VsiriusProduct>, IVsiriusProductRepository
    {
        public VsiriusProductRepository(DataContext context) : base(context)
        {
        }
    }
}
