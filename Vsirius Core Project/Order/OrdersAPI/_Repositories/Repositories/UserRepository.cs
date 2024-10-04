using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class UserRepository : Repository<VsiriusUser>, IUserRepository
    {
        public UserRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
