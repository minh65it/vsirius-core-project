using IdentityApi.Dtos.User;
using IdentityApi.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityApi._Services.Interfaces
{
    public interface IUserService
    {
        //User
        Task<UserForLoggedDto> Login(UserForLoginParam param);
        Task<OperationResult> LogOut();
        Task<OperationResult> CreateUser(UserDto Dto, IUrlHelper urlHelper, string originUrl);
        Task ConfirmEmail(string email, IUrlHelper urlHelper, string originUrl);
        Task SendSMS(string email);
        Task<OperationResult> VerifySMS(VerifySMS dto);
        Task<PaginationUtility<UserDto>> GetAllUser(PaginationParam pagination, string inputSearch);
        Task<UserDto> GetUserDetail(string mail);
        Task<OperationResult> UpdateUser(UserDto Dto);
        Task<OperationResult> UpdatePassword(UpdatePasswordDto Dto);
        Task<OperationResult> UpdatePasswordByAdmin(UpdatePasswordDto Dto);
        Task<OperationResult> DeleteUser(string id);
        string GetState();
    }
}
