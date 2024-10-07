using MongoDB.Bson.Serialization.Attributes;

namespace WalmartIntegrationDataApi.Models
{
    public class SubmittedWalmartOrder
    {
        [BsonId]
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string WalmartOrderId { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }
    }
}
