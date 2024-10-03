using IdentityApi._Repositories.Interfaces;
using IdentityApi.Data;
using IdentityApi.Models;

namespace IdentityApi._Repositories.Repositories
{
    public class UserRolesEmployeeRepository : Repository<UserRolesEmployee>, IUserRolesEmployeeRepository
    {
        public UserRolesEmployeeRepository(IdentityContext context) : base(context)
        {
        }
    }
}
