using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByEmail();
        Task<bool> SendOTP();
        Task<(bool, string?)> UpdateUser(UserVM user);
        Task<bool> VerifyEmail(string email);
        Task<bool> VerifyPhone(string code);

    }
}
