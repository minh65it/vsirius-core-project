using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace OrderProcessDomain.Qgold
{
    public class QgoldQA
    {
        [BsonId]
        public string _id { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerName { get; set; }
        public string? OrderQgoldId { get; set; }
        public string? SKU { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public string? DateOrder { get; set; }
        public string? CustomerAddress1 { get; set; }
        public string? ZipCode { get; set; }
        public string? ShipVia { get; set; }
        public string? Tracking { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketOrderId { get; set; }
        public bool? IsSent { get; set; }
        public string? CCEmail1 { get; set; }
        public string? CCEmail2 { get; set; }
        public List<FilesInfo>? Files { get; set; }
        public DateTime? Date { get; set; }
    }
    public class FilesInfo
    {
        public string? Text { get; set; }
        public string? Url { get; set; }
    }
}
