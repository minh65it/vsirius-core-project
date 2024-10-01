using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
