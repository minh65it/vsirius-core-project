namespace AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor
{
    public class AmazonOrderDto
    {
        public string? AmazonOrderId { get; set; }
        public bool? IsSubmitted { get; set; }
        public string? OrderStatus { get; set; }
        public string? FulfillmentChannel { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OrderType { get; set; }
        public string? SalesChannel { get; set; }
        public string? ShipServiceLevel { get; set; }
        public string? Total { get; set; }
        public string? MarketplaceId { get; set; }
        public string? ShipmentServiceLevelCategory { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? EarliestShipDate { get; set; }
        public DateTime? LatestShipDate { get; set; }
        public DateTime? EarliestDeliveryDate { get; set; }
        public DateTime? LatestDeliveryDate { get; set; }
        public string? BuyerName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? City { get; set; }
        public string? StateOrRegion { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? BuyerEmail { get; set; }
        public List<ItemAmazon>? OrderItemList { get; set; }
        public string? SupplierOrderId { get; set; }
        public string? Supplier { get; set; }
    }
    public class ItemAmazon
    {
        public string? ASIN { get; set; }
        public string? SellerSKU { get; set; }
        public string? OrderItemId { get; set; }
        public string? Title { get; set; }
        public int? QuantityOrdered { get; set; }
        public string? ItemPrice { get; set; }
        public string? ShippingPrice { get; set; }
    }
    public class AmazonOrderDetailDTO
    {
        public string? AmazonOrderId { get; set; }
        public bool? IsSubmitted { get; set; }
        public string? OrderStatus { get; set; }
        public string? FulfillmentChannel { get; set; }
        public string? PaymentMethod { get; set; }
        public string? OrderType { get; set; }
        public string? SalesChannel { get; set; }
        public string? ShipServiceLevel { get; set; }
        public string? Total { get; set; }
        public string? MarketplaceId { get; set; }
        public string? ShipmentServiceLevelCategory { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? EarliestShipDate { get; set; }
        public DateTime? LatestShipDate { get; set; }
        public DateTime? EarliestDeliveryDate { get; set; }
        public DateTime? LatestDeliveryDate { get; set; }
        public string? BuyerName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? City { get; set; }
        public string? StateOrRegion { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? BuyerEmail { get; set; }
        public List<ItemAmazon>? OrderItemList { get; set; }
        public string? SupplierOrderId { get; set; }
        public string? Supplier { get; set; }
    }
    public class AmazonProcessSearchParam
    {
        public string? AmazonOrderId { get; set; }
        public string? Supplier { get; set; }
        public string? SKU { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? FromDate { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? ToDate { get; set; }
        public string? AddressLine1 { get; set; }
        public string? BuyerName { get; set; }
        public bool? IsSubmitted { get; set; }
    }

    public class UnshippedOrderIdsParam
    {
        public string? Provider { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
