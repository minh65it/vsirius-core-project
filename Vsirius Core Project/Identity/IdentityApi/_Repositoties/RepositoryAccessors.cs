using IdentityApi._Repositories.Interfaces;
using IdentityApi._Repositories.Repositories;
using VsiriusDataModel.DataContext;

namespace IdentityApi._Repositoties
{
    public class RepositoryAccessor : IRepositoryAccessor
    {
        private VsiriusContext _dentityContext;
        public RepositoryAccessor(VsiriusContext dentityContext)
        {
            _dentityContext = dentityContext;
            employee = new EmployeeRepository(_dentityContext);
            roleClaimsEmployee = new RoleClaimsEmployeeRepository(_dentityContext);
            userRolesEmployee = new UserRolesEmployeeRepository(_dentityContext);
            rolesEmployee = new RolesEmployeeRepository(_dentityContext);
            user = new UserRepository(_dentityContext);   
        }

        public IEmployeeRepository employee { get; private set; }

        public IRoleClaimsEmployeeRepository roleClaimsEmployee { get; private set; }

        public IRolesEmployeeRepository rolesEmployee { get; private set; }

        public IUserRolesEmployeeRepository userRolesEmployee { get; private set; }

        public IUserRepository user { get; private set; }

        public async Task<bool> Save()
        {
            return await _dentityContext.SaveChangesAsync() > 0;
        }
    }
}
