using System.ComponentModel.DataAnnotations;

namespace PimApi.Models
{
    public class Variation
    {
        [Key] 
        public int Id { get; set; }
        [StringLength(100)]
        public required string VariationId { get; set; }
        [StringLength(100)]
        public required string VariationName { get; set; }
        [StringLength(50)]
        public required string ItemSku { get; set; }
    }
}
