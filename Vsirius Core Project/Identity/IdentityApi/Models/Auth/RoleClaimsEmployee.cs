
namespace IdentityApi.Models
{
    public class RoleClaimsEmployee : VsiriusDataModel.DataModel.IRoleClaimsEmployee
    {
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public int Id { get; set; }
    }
}
