using eBay.Service.Core.Soap;

namespace EbayApi.Dtos
{
    public class ProductSummary
    {
        public string SKU { get; set; }
        public string ItemID { get; set; }
        public string Tittle { get; set; }
        public ListingStatusCodeType Status { get; set; }
    }
}