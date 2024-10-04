using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using FikaAmazonAPI.ConstructFeed.Messages;

namespace AmazonIntegrationDataApi.Models.OrderAmazonProcessor
{
    public class OrderTrackingAmazon : OrderFulfillmentMessage
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
    }
}
