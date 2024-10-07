using MongoDB.Bson.Serialization.Attributes;
using OrderProcessDomain.Walmart;

namespace OrderProcessApi.DTOs
{
    public class OrderWalmartSellerDto
    {
        public List? list { get; set; }
    }

    public class List
    {
        public Meta? meta { get; set; }
        public Elements? elements { get; set; }
    }
    public class Meta
    {
        public int totalCount { get; set; }
        public int limit { get; set; }
        public string? nextCursor { get; set; }
    }

    public class Elements
    {
        public List<WalmartOrder>? order { get; set; }
    }
}
