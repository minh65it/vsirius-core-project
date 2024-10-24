using System.ComponentModel.DataAnnotations;

namespace VsiriusDataModel.DataModel
{
    public interface IUserRolesEmployee
    {
        [Key]
        public string Id { get; set; }
        public string? Email { get; set; }
        public Guid? RoleId { get; set; }
    }
    public class UserRolesEmployee : IUserRolesEmployee
    {
        [Key]
        public string Id { get; set; }
        public string? Email { get; set; }
        public Guid? RoleId { get; set; }
    }
}
