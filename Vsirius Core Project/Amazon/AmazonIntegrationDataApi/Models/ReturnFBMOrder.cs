using FikaAmazonAPI.ReportGeneration;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AmazonIntegrationDataApi.Models
{
    public class ReturnFBMOrder : ReturnFBMOrderRow
    {
        [BsonId]
        public ObjectId Id { get; set; }
    }
}
