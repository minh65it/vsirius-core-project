namespace QgoldIntegrationDataApi.Dtos.Order
{
    public class OrderAmazonDto
    {
        public string amazonOrderId { get; set; }
        public string qgoldOrderId { get; set; }
        public bool isSubmitted { get; set; }
        public string orderStatus { get; set; }
        public string fulfillmentChannel { get; set; }
        public string paymentMethod { get; set; }
        public string orderType { get; set; }
        public string salesChannel { get; set; }
        public string shipServiceLevel { get; set; }
        public string total { get; set; }
        public object marketplaceId { get; set; }
        public object shipmentServiceLevelCategory { get; set; }
        public DateTime purchaseDate { get; set; }
        public DateTime? earliestShipDate { get; set; }
        public DateTime? latestShipDate { get; set; }
        public DateTime? earliestDeliveryDate { get; set; }
        public DateTime? latestDeliveryDate { get; set; }
        public string buyerName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string city { get; set; }
        public string stateOrRegion { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }
        public string phone { get; set; }
        public string buyerEmail { get; set; }
        public List<OrderItemList> orderItemList { get; set; }
    }

    public class OrderItemList
    {
        public string asin { get; set; }
        public string sellerSKU { get; set; }
        public string orderItemId { get; set; }
        public string title { get; set; }
        public int quantityOrdered { get; set; }
        public string itemPrice { get; set; }
        public string shippingPrice { get; set; }
    }
}
