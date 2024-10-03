using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class UserRepository : Repository<VsiriusUser>, IUserRepository
    {
        public UserRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
