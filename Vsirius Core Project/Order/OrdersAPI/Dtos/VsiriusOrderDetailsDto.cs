namespace OrdersAPI.Dtos
{
    public class VsiriusOrderDetailsDto
    {
        public Guid OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public string? ProductId { get; set; }
        public int? QuantityItem { get; set; }
        public decimal? Price { get; set; }
    }
}
