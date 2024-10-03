using IdentityApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace IdentityApi._Repositories.Repositories
{
    public class UserRolesEmployeeRepository : Repository<UserRolesEmployee>, IUserRolesEmployeeRepository
    {
        public UserRolesEmployeeRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
