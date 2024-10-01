using IdentityApi._Repositories.Interfaces;

namespace IdentityApi._Repositoties
{
    public interface IRepositoryAccessor
    {
        IEmployeeRepository employee { get; }
        IRoleClaimsEmployeeRepository roleClaimsEmployee { get; }
        IRolesEmployeeRepository rolesEmployee { get; }
        IUserRolesEmployeeRepository userRolesEmployee { get; }
        IUserRepository user { get; }
        Task<bool> Save();
    }
}
