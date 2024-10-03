using IdentityApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace IdentityApi._Repositories.Repositories
{
    public class RolesEmployeeRepository : Repository<RolesEmployee>, IRolesEmployeeRepository
    {
        public RolesEmployeeRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
