using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Models
{
    public class Employee : VsiriusDataModel.DataModel.IEmployee
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string? Phone { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Avatar { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public int? PostCode { get; set; }
        public string? Country { get; set; }
        public bool? IsActive { get; set; }
        public string? FullName { get; set; }
    }
}
