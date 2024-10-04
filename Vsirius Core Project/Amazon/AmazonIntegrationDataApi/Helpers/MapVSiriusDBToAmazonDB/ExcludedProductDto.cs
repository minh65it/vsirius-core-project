namespace AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB
{
    public class ExcludedProductDto
    {
        public string MarketplaceSKU { get; set; }
        public string Marketplace { get; set; }
        public string SupplierSKU { get; set; }
        public string ExclusionType { get; set; }
        public string ExclusionNote { get; set; }
    }

    public enum ExclusionType
    {
        NoStock,
        Deleted
    }
}
