namespace OrdersAPI.Dtos
{
    public class VsiriusListCartDetailsDto
    {
        public Guid? CartDetailsId { get; set; }
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? InfoBrowser { get; set; }
        public string ProductName { get; set; }
        public string? Image { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
    }
}
