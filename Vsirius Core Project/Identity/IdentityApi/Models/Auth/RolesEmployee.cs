namespace IdentityApi.Models
{
    public class RolesEmployee : VsiriusDataModel.DataModel.IRolesEmployee
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
    }
}
