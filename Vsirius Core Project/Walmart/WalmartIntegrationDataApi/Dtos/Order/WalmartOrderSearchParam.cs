namespace WalmartIntegrationDataApi.Dtos.Order
{
    public class WalmartOrderSearchParam
    {
        public string? purchaseOrderId { get; set; }
        public string? customerOrderId { get; set; }
        public string? SupplierOrderId { get; set; }
        public string? Supplier { get; set; }
        public string? TrackingNumber { get; set; }
        public string? BuyerName { get; set; }
        public string? Address1 { get; set; }
        /// <summary>
        /// Created == 0, Acknowledged = 1, Shipped = 2, Delivered = 3, Cancelled = 4.
        /// </summary>
        public WalmartOrderType? OrderStatus { get; set; }
        public string? SKU { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? FromDate { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? ToDate { get; set; }
        public bool? IsSubmitted { get; set; }
    }

    public enum WalmartOrderType
    {
        Created,
        Acknowledged,
        Shipped,
        Delivered,
        Cancelled
    }
}
