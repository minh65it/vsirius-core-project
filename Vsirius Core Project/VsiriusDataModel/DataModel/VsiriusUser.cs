using Microsoft.AspNetCore.Identity;

namespace VsiriusDataModel.DataModel
{
    public class VsiriusUser : IdentityUser
    {
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Avatar { get; set; }
        public string? FullName { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public bool? IsActive { get; set; }
    }
}
