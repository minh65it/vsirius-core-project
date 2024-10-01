using IdentityApi._Repositories.Interfaces;
using IdentityApi.Data;
using IdentityApi.Models;

namespace IdentityApi._Repositories.Repositories
{
    public class RolesEmployeeRepository : Repository<RolesEmployee>, IRolesEmployeeRepository
    {
        public RolesEmployeeRepository(IdentityContext context) : base(context)
        {
        }
    }
}
