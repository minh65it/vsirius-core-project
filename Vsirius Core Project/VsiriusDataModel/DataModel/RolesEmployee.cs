using System.ComponentModel.DataAnnotations;

namespace VsiriusDataModel.DataModel
{
    public interface IRolesEmployee
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
    }
    public class RolesEmployee : IRolesEmployee
    {
        [Key]
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
    }
}
