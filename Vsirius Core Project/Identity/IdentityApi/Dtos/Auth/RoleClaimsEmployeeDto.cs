using VsiriusDataModel.DataModel;

namespace IdentityApi.Dtos.Auth
{
    public class RoleClaimsEmployeeDto : VsiriusDataModel.DataModel.IRoleClaimsEmployee
    {
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public int Id { get; set; }
    }
}
