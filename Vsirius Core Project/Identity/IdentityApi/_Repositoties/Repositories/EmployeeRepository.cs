using IdentityApi._Repositories.Interfaces;
using VsiriusDataModel.DataModel;
using VsiriusDataModel.DataContext;

namespace IdentityApi._Repositories.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
