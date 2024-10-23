using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VsiriusDataModel.DataModel
{
    public interface IRoleClaimsEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? RoleId { get; set; }
        public string? ClaimType { get; set; }
    }
    public class RoleClaimsEmployee : IRoleClaimsEmployee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? RoleId { get; set; }
        public string? ClaimType { get; set; }
    }
}
