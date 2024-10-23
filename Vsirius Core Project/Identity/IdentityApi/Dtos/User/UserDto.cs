namespace IdentityApi.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        //public IList<string> Roles { get; set; }
        public string? Address1 { get; set; }
        public string? OldPassword { get; set; }
        public string? Address2 { get; set; }
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public bool? IsActive { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        //public bool? TwoFactorEnabled { get; set; }
        public virtual bool? TwoFactorEnabled { get; set; }
    }
    public class UpdatePasswordDto
    {
        public string Id { get; set; }
        public string? Password { get; set; }
        public string? OldPassword { get; set; }
    }
    public class UserForLoginParam
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAuthen { get; set; }
        public string Provider { get; set; }
    }
    public class UserForLoggedDto
    {
        public bool isLogin { get; set; }
        public UserDto user { get; set; }
        public string token { get; set; }
        public string Message { get; set; }
    }
    public class VerifySMS
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
