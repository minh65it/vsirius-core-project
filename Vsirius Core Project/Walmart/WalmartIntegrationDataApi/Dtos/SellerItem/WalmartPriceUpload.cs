namespace WalmartIntegrationDataApi.Dtos.SellerItem
{
    public class WalmartPriceUpload
    {
        public PriceHeader? PriceHeader { get; set; }
        public List<PriceUpload>? Price { get; set; }
    }
    public class PriceHeader
    {
        public string? version { get; set; } = "1.7";

    }
    public class PriceUpload
    {
        public string? sku { get; set; }
        public List<pricing>? pricing { get; set; }

    }
    public class pricing
    {
        public currentPrice? currentPrice { get; set; }

    }
    public class currentPrice
    {
        public string? currency { get; set; } = "USD";
        public decimal? amount { get; set; }

    }
}
