namespace PimApi.Models
{
    public class Order
    {
        public string OrderId { get; set; }
        public string ItemSku { get; set; }
        public string? ShipToName { get; set; }
        public string? ShipToPhone { get; set; }
        public string? ShipToAddress { get; set; }
        public string? ShipToZip { get; set; }
        public string? MethodCode { get; set; }
        public string? TrackingId { get; set; }
        public int OrderQuantity { get; set; }
        public int ReturnQuantity { get; set; }
        public string Marketplace { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
