using PackageApi.Models;

namespace PackageApi.Dtos.Claim
{
    public class ClaimSearchParam
    {
        /// <summary>
        ///  0:ReturnPackage,1:OrderDocuments,2:ShippedPackage
        /// </summary>
        public DocumentType? DocumentType { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketplaceOrderId { get; set; }
        public string? RMA { get; set; }
    }
}
