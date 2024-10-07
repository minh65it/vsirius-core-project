using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderProcessDomain.AmazonSeller
{
    public class OrderAmazonTracking
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string AmazonOrderID { get; set; }
        public object MerchantOrderID { get; set; }
        public object MerchantFulfillmentID { get; set; }
        public string FulfillmentDate { get; set; }
        public FulfillmentData FulfillmentData { get; set; }
        public object CODCollectionMethod { get; set; }
        public object Item { get; set; }
        public object ShipFromAddress { get; set; }
    }

    public class FulfillmentData
    {
        public object CarrierCode { get; set; }
        public string CarrierName { get; set; }
        public string ShippingMethod { get; set; }
        public string ShipperTrackingNumber { get; set; }
    }
}
