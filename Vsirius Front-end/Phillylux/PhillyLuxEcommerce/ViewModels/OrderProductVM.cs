namespace PhillyLuxEcommerce.ViewModels
{
    public class OrderProductVM
    {
        public string? SKU { get; set; }
        public string? ProductName { get; set; }
        public string? Price { get; set; }
        public string? PriceSale { get; set; }
        public string? main_image_url { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }
        public decimal? QuantityItem { get; set; }
    }
}
