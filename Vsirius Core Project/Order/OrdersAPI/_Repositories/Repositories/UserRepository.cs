using OrdersAPI._Repositories.Interfaces;
using OrdersAPI.Data;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class UserRepository : Repository<VsiriusUser>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}
