using VsiriusDataModel.DataModel;
namespace IdentityApi.Dtos.Auth
{
    public class EmployeeDto : IEmployee
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public bool? IsActive { get; set; }
        public IList<string> Roles { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string? PostCode { get; set; }
        public string Country { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
    }

    public class EmployeeForLoggedDto
    {
        public bool isLogin { get; set; }
        public EmployeeDto user { get; set; }
        public string token { get; set; }
        public string Message { get; set; }
    }
    public class EmployeeForLoginParam
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAuthen { get; set; }
    }
    public class RoleClaimEmployeeDto
    {
        public string RoleId { get; set; }
        public List<string> ClaimType{ get; set; }
    }
    public enum Permissions
    {
        Admin,
        Employee,
        Product,
        Order,
        Contact,
        Cart,
        System,
        Notification,
        DrivenHTML
    }
}
