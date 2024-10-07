namespace WalmartIntegrationDataApi.Dtos.Order
{
    public class OrderDetailDTO
    {
        public string? purchaseOrderId { get; set; }
        public string? customerOrderId { get; set; }
        public string? SupplierOrderId { get; set; }
        public string? Supplier {  get; set; }
        public string? TrackingNumber { get; set; }
        public string? CarrierName { get; set; }
        public string? customerEmailId { get; set; }
        public bool? IsSubmitted { get; set; }
        public DateTime? orderDate { get; set; }
        public DateTime? estimatedDeliveryDate { get; set; }
        public DateTime? estimatedShipDate { get; set; }
        public string? methodShipping { get; set; }
        public string? BuyerName { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Phone { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        public List<WalmartItemDTO>? Items { get; set; }
    }
    public class WalmartItemDTO
    {
        public string? Sku { get; set; }
        public string? ProductName { get; set; }
        public double? Price { get; set; }
        public double? Tax { get; set; }
        public string? quantity { get; set; }
        public string? OrderStatus { get; set; }
    }
}
