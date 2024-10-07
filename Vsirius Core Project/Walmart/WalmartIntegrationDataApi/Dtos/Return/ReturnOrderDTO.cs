using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi.Dtos.Return
{
    public class ReturnOrderDTO
    {
        public string returnOrderId { get; set; }
        public string customerEmailId { get; set; }
        public string returnType { get; set; }
        public string replacementCustomerOrderId { get; set; }
        public CustomerName customerName { get; set; }
        public string customerOrderId { get; set; }
        public DateTime returnOrderDate { get; set; }
        public DateTime returnByDate { get; set; }
        public string refundMode { get; set; }
        public TotalRefundAmount totalRefundAmount { get; set; }
        public List<ReturnLineGroup> returnLineGroups { get; set; }
        public List<ReturnOrderLine> returnOrderLines { get; set; }
        public ReturnChannel returnChannel { get; set; }
        public ClaimDTO? claim { get; set; }
    }

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

    public class ImageInfo
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
