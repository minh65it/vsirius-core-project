using System.ComponentModel.DataAnnotations;

namespace SkuMappingAPI.Models
{
    public class SKUInfo
    {
        [Key]
        public Guid Id { get; set; }
        public string? SKUQgold { get; set; }
        public string? SKUStuller { get; set; }
        public string? SKUWalmart { get; set; }
        public string? SKUAmazon { get; set; }
        public string? SKUVsirius { get; set; }
    }
}
