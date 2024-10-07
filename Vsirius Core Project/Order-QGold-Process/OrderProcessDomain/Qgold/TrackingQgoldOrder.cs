using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderProcessDomain.Qgold
{
    public class TrackingQgoldOrder
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string OrderQgoldId { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
