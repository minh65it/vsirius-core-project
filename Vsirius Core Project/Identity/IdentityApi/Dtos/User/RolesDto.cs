using IdentityApi.Models.User;

namespace IdentityApi.Dtos.User
{
    public class RolesDto : Roles
    {
        public List<string> RolesClaims { get; set;}
    }
}
