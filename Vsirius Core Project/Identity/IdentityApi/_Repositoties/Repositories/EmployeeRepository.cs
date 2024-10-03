using IdentityApi._Repositories.Interfaces;
using IdentityApi.Data;
using IdentityApi.Models;

namespace IdentityApi._Repositories.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IdentityContext context) : base(context)
        {
        }
    }
}
