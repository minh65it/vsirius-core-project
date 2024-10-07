using MongoDB.Bson.Serialization.Attributes;

namespace OrderProcessDomain.Walmart
{
    public class SubmittedWalmartOrder
    {
        [BsonId]
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string WalmartOrderId { get; set; }
        public DateTime? SubmittedDate { get; set; }
    }
}
