namespace StullerOrderProcessAPI.Dtos
{
    public class EbayOrderDto
    {
        public string? OrderId { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? TrackingNo { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public double TotalPrice { get; set; }
        public string? StatusOrder { get; set; }
        public DateTime OrderDate { get; set; }
        public bool IsSubmit { get; set; }
        public string? Supplier { get; set; }
        public List<OrderEbayDetail> OrderDetails { get; set; }
    }
    public class OrderEbayDetail
    {
        public string? OrderDetailId { get; set; }
        public string? OrderId { get; set; }
        public string? Sku { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}
