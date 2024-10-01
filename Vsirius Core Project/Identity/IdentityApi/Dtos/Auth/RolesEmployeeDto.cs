namespace IdentityApi.Dtos.Auth
{
    public class RolesEmployeeDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public List<string>? ClaimType { get; set; }
    }
}
