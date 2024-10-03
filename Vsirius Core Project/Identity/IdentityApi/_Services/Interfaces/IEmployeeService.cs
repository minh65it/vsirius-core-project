using Google.Apis.Auth;
using IdentityApi.Dtos.Auth;
using IdentityApi.Helpers.Utilities;
using static IdentityApi.Dtos.Auth.AuthDto;

namespace IdentityApi._Services.Interfaces
{
    public interface IEmployeeService
    {
        //Employee
        Task<OperationResult> CreateEmployee(EmployeeDto Dto);
        Task<PaginationUtility<EmployeeDto>> GetAllEmployee(PaginationParam pagination, string inputSearch);
        Task<EmployeeDto> GetEmployeeDetail(string mail);
        Task<OperationResult> UpdateEmployee(EmployeeDto Dto);
        Task<OperationResult> DeleteEmployee(string id);
        //Roles
        Task<List<RolesEmployeeDto>> GetAllRole();
        Task<OperationResult> CreateRole(RolesEmployeeDto roles);
        Task<OperationResult> CreateAllRole();
        Task<OperationResult> DeleteRole(string id);
        Task<OperationResult> UpdateRole(RolesEmployeeDto roles);
        Task<RolesEmployeeDto> GetDetailRole(string id);
    }
}
