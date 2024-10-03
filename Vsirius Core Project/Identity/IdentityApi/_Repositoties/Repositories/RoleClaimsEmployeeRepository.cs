using IdentityApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace IdentityApi._Repositories.Repositories
{
    public class RoleClaimsEmployeeRepository : Repository<RoleClaimsEmployee>, IRoleClaimsEmployeeRepository
    {
        public RoleClaimsEmployeeRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
