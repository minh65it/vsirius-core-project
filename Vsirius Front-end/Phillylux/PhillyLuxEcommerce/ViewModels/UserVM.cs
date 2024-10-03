namespace PhillyLuxEcommerce.ViewModels
{
    public class UserVM
    {
        public string? Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? PasswordHash { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public bool? IsActive { get; set; }
        public bool? EmailConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public IFormFile? UploadImg { get; set; }
    }
}
