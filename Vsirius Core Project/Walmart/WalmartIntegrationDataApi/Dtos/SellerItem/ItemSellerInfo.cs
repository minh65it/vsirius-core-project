namespace WalmartIntegrationDataApi.Dtos.SellerItem
{
    public class ItemSellerInfo
    {
        public List<Item>? ItemResponse { get; set; }
        public int totalItems { get; set; }
        public string nextCursor { get; set; } = "*";

    }
    public class Item
    {
        public string? mart { get; set; } = string.Empty;
        public string? sku { get; set; } = string.Empty;
        public string? wpid { get; set; } = string.Empty;
        public string? upc { get; set; } = string.Empty;
        public string? gtin { get; set; } = string.Empty;
        public string? productName { get; set; } = string.Empty;
        public string? shelf { get; set; } = string.Empty;
        public string? productType { get; set; } = string.Empty;
        public Price? price { get; set; }
        public string? publishedStatus { get; set; }
        public string? lifecycleStatus { get; set; }
        public Reason? unpublishedReasons { get; set; }
    }

    public class Price
    {
        public string? currency { get; set; } = string.Empty;
        public decimal? amount { get; set; } = decimal.Zero;
    }

    public class Reason
    {
        public List<string>? reason { get; set; }
    }
}
