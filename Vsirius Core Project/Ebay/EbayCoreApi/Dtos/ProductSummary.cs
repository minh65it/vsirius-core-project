using eBay.Service.Core.Soap;

namespace EbayCoreApi.Dtos
{
    public class ProductSummary
    {
        public string SKU { get; set; }
        public string ItemID { get; set; }
        public string Tittle { get; set; }
        public ListingStatusCodeType? Status { get; set; }
    }
}