namespace AmazonIntegrationDataApi.Dtos.MongoDB
{
    public class ClaimDto
    {
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
}
