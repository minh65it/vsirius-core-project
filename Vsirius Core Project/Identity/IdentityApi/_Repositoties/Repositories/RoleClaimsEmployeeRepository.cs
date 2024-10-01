using IdentityApi._Repositories.Interfaces;
using IdentityApi.Data;
using IdentityApi.Models;

namespace IdentityApi._Repositories.Repositories
{
    public class RoleClaimsEmployeeRepository : Repository<RoleClaimsEmployee>, IRoleClaimsEmployeeRepository
    {
        public RoleClaimsEmployeeRepository(IdentityContext context) : base(context)
        {
        }
    }
}
