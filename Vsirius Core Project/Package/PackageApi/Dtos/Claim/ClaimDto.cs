using PackageApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PackageApi.Dtos.Claim
{
    public class ClaimDTO
    {
        public string DocumentType { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketplaceOrderId { get; set; }
        public string? RMA { get; set; }
        public List<string> Notes { get; set; }
        public List<ImageInfo> Images { get; set; }
        public DateTime ScanDate { get; set; }
    }
}
