namespace QgoldIntegrationDataApi.Dtos.Order
{
    public class OrderWalmartDto
    {
        public string purchaseOrderId { get; set; }
        public string customerOrderId { get; set; }
        public object qgoldOrderId { get; set; }
        public string trackingNumber { get; set; }
        public string carrierName { get; set; }
        public string customerEmailId { get; set; }
        public bool isSubmitted { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime estimatedDeliveryDate { get; set; }
        public DateTime estimatedShipDate { get; set; }
        public string methodShipping { get; set; }
        public string buyerName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string phone { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public List<ItemOrder> items { get; set; }
    }

    public class ItemOrder
    {
        public string sku { get; set; }
        public string productName { get; set; }
        public double price { get; set; }
        public double? tax { get; set; }
        public string quantity { get; set; }
        public string orderStatus { get; set; }
    }
}
