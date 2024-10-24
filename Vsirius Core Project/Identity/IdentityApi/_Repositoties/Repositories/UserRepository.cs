using IdentityApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace IdentityApi._Repositories.Repositories
{
    public class UserRepository : Repository<VsiriusUser>, IUserRepository
    {
        public UserRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
