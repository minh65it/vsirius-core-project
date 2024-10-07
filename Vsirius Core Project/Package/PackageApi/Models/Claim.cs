using System.ComponentModel.DataAnnotations;

namespace PackageApi.Models
{
    public class Claim
    {
        [Key]
        public string Id { get; set; }
        public string DocumentType { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketplaceOrderId { get; set; }
        public string? RMA { get; set; }
        public List<string> Notes { get; set; }
        public List<ImageInfo> Images { get; set; }
        public DateTime ScanDate { get; set; }
    }
    public class ImageInfo
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }

    public enum DocumentType
    {
        /// <summary>
        /// 0: ReturnPackage
        /// </summary>
        ReturnPackage,
        /// <summary>
        /// 1: OrderDocuments
        /// </summary>
        OrderDocuments,
        /// <summary>
        /// 3: ShippedPackage
        /// </summary>
        ShippedPackage,
    }
}
