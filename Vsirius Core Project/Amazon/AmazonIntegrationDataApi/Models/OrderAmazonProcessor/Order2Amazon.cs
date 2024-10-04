using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmazonIntegrationDataApi.Models.OrderAmazonProcessor
{
    public class Order2Amazon
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string OrderID { get; set; }
        public object TrackingID { get; set; }
        public string PurchaseDate { get; set; }
        public string BuyerName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ShipTo { get; set; }
        public string AddressType { get; set; }
        public string Phone { get; set; }
        public List<OrderContent> OrderContent { get; set; }
        public string OrderDate { get; set; }
        public string OrderDetails { get; set; }
        public string InstructionDetails { get; set; }
        [BsonElement("Instruction details")]
        public string Instruction_Details { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class OrderContent
    {
        public string Status { get; set; }
        public string ProductName { get; set; }
        public string ProductNameUrl { get; set; }
        public string MoreInformation { get; set; }
        public string Quantity { get; set; }
        public string UnitPrice { get; set; }
        public string Proceeds { get; set; }
    }
}
