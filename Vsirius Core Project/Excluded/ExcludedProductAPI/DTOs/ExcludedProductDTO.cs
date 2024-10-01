using ExcludedProductAPI.Models;

namespace ExcludedProductAPI.DTOs
{
    public class ExcludedProductDTO
    {
        public string MarketplaceSKU { get; set; }
        public string Marketplace { get; set; }
        public string SupplierSKU { get; set; }
        public string ExclusionType { get; set; }
        public string ExclusionNote { get; set; }
    }
    public class ExcludedProductParam
    {
        public string? SupplierSKU { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketplaceSKU { get; set; }
        /// <summary>
        /// 0: NoStock, 1: Deleted
        /// </summary>
        public ExclusionType? ExclusionType { get; set; }
    }
}
