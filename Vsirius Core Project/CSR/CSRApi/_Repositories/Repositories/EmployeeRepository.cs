using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
