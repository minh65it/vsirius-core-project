using VsiriusDataModel.DataModel;

namespace IdentityApi.Dtos.User
{
    public class RolesDto : Roles
    {
        public List<string> RolesClaims { get; set;}
    }
}
