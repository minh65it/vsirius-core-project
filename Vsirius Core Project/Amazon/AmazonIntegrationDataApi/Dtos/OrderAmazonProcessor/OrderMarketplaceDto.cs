using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor
{
    /// <summary>
    /// Using for Qgold.OrderMarketpalce, Qgold.ReSubmit
    /// </summary>
    public class OrderMarketplaceDto
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string OrderId { get; set; }
        public string PO { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState_Province { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountryCode { get; set; }
        public string ShipToPhone { get; set; }
        public List<OrderMarketplaceItem> Item { get; set; }
        public string Description { get; set; }
        public string Ship_Method_Code { get; set; }
        public string Ship_Option_Code { get; set; }
        public string Cust_Order_Number { get; set; }
        public string Seller_Order_ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Status { get; set; }
        public string Marketplace { get; set; }
    }
    public class OrderMarketplaceItem
    {
        public string Item { get; set; }
        public string Qty { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
    }
    public class OrderMarketplaceParams
    {
        public string OrderId { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToZip { get; set; }
        public List<string> Styles { get; set; }
        public string Seller_Order_ID { get; set; }
        public string Marketplace { get; set; }
    }
}
