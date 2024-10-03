namespace IdentityApi.Models
{
    public class UserRolesEmployee : VsiriusDataModel.DataModel.IUserRolesEmployee
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
    }
}
