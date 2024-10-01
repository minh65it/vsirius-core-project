using IdentityApi.Dtos.Auth;
using IdentityApi.Helpers.Utilities;
using static IdentityApi.Dtos.Auth.AuthDto;

namespace IdentityApi._Services.Interfaces
{
    public interface IAuthService
    {
        Task<EmployeeDto> Login(AuthForLoginParam param);
        Task<OperationResult> LogOut();
    }
}
