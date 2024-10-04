using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers
{
    public class OrderStuller
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string OrderID { get; set; }
        public string Confirmation { get; set; }
        public string PO { get; set; }
        public string OrderDate { get; set; }
        public string Status { get; set; }
        public string Account { get; set; }
        public string EstTotalPrice { get; set; }
        public string UrlDetail { get; set; }
        public List<TrackOrder> TrackOrder { get; set; }
        public List<Item> Items { get; set; }
        public string Shipping { get; set; }
        public string Billing { get; set; }
        public string ZipCode { get; set; }
        public string Address1 { get; set; }
        public string BuyerName { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class Item
    {
        public string ProductName { get; set; }
        public string Ordered { get; set; }
        public string Shipped { get; set; }
        public string EstShipDate { get; set; }
        public string UnitPrice { get; set; }
        public string EstPrice { get; set; }
    }
    public class TrackOrder
    {
        public string Invoice { get; set; }
        public string Status { get; set; }
        public string ShipDate { get; set; }
        public string DeliveryDate { get; set; }
        public string Carrier { get; set; }
        public string Tracking { get; set; }
        public string ShippingUpdates { get; set; }
        public string InvoiceAmount { get; set; }
    }
}
