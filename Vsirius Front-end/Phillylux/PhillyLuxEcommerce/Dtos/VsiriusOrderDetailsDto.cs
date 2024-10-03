namespace PhillyLuxEcommerce.Dtos
{
    public class VsiriusOrderDetailsDto
    {
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public string? ProductId { get; set; }
        public int? QuantityItem { get; set; }
        public decimal? Price { get; set; }
    }
}
