using System.ComponentModel.DataAnnotations;

namespace ProductAiApi.Models
{
    public class ProductAI
    {
        [Key][StringLength(100)]
        public string SKU { get; set; }
        [StringLength(200)] public string ProductName { get; set; }
        [StringLength(200)] public string Brand { get; set; }
        [StringLength(1000)] public string? Keywords { get; set; } = string.Empty;
        [StringLength(1000)] public string? BulletPoint1 { get; set; }
        [StringLength(1000)] public string? BulletPoint2 { get; set; }
        [StringLength(1000)] public string? BulletPoint3 { get; set; }
        [StringLength(1000)] public string? BulletPoint4 { get; set; }
        [StringLength(1000)] public string? BulletPoint5 { get; set; }
        [StringLength(1000)] public string? BulletPoint6 { get; set; }
        [StringLength(1000)] public string? BulletPoint7 { get; set; }
        [StringLength(1000)] public string? BulletPoint8 { get; set; }
        [StringLength(1000)] public string? BulletPoint9 { get; set; }
        public string? Note { get; set; }
        public bool IsReview { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
