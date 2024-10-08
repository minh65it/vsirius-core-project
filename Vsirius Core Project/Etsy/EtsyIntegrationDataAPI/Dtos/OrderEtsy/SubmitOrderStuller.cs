namespace EtsyIntegrationDataAPI.Dtos.OrderEtsy
{
    public class SubmitOrderStuller
    {
        public string? ShipToName { get; set; }
        public string? ShipToAddress1 { get; set; }
        public string? ShipToAddress2 { get; set; }
        public string? ShipToAddress3 { get; set; }
        public string? ShipToCity { get; set; }
        public string? ShipToState { get; set; }
        public string? ShipToZip { get; set; }
        public string? ShipToCountryCode { get; set; }
        public string? ShipToProvince { get; set; }
        public string? ShipToPhone { get; set; }
        public List<ItemSubmit> Item { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// UPS_NEXT_DAY, UPS_NEXT_DAY_SAVR, UPS_2DAY, UPS_GROUND, FED_PRIORITY, FED_STD_OVERNIGHT, FED_2DAY, FED_GROUND, USPS_FIRST_CLASS,
        /// FED_INT_PRIORITY, UPS_WW_EXPR, UPS_STD_CANADA, FED_INT_ECONOMY, USPS_PRIORITY, FEDEX_EXPRESS_SAVER, USPS_EXPRESS, FEDEX_HOME_DELIVERY
        /// </summary>
        public string? ShipMethodCode { get; set; }
        public string? MarketplaceOrderId { get; set; }
        public string? Marketplace { get; set; }
    }
    public class ItemSubmit
    {
        public string? Item { get; set; }
        public string? Qty { get; set; }
        public string? Size { get; set; }
        public string? Price { get; set; }
    }
    //Detail
    public class StullerOrderDTO
    {
        public string? OrderID { get; set; }
        public string? Confirmation { get; set; }
        public string? PO { get; set; }
        public string? OrderDate { get; set; }
        public string? Status { get; set; }
        public string? Account { get; set; }
        public string? EstTotalPrice { get; set; }
        public string? UrlDetail { get; set; }
        public string? Invoice { get; set; }
        public string? ShipDate { get; set; }
        public string? DeliveryDate { get; set; }
        public string? Carrier { get; set; }
        public string? Tracking { get; set; }
        public string? ShippingUpdates { get; set; }
        public string? InvoiceAmount { get; set; }
        public List<ItemStuller> Items { get; set; }
        public string? Shipping { get; set; }
        public string? Billing { get; set; }
        public string? ZipCode { get; set; }
        public string? Address1 { get; set; }
        public string? BuyerName { get; set; }
        public string? ShippingMethod { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketplaceOrderId { get; set; }
    }
    public class ItemStuller
    {
        public string ProductSku { get; set; }
        public string Ordered { get; set; }
        public string Shipped { get; set; }
        public string EstShipDate { get; set; }
        public string UnitPrice { get; set; }
        public string EstPrice { get; set; }
    }
    public class StullerSearchParam
    {
        //public string? AmazonOrderId { get; set; }
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
}
