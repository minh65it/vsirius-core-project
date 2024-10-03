namespace PhillyLuxEcommerce.ViewModels
{
    public class HomeVMProduct
    {

        public string SKU { get; set; }
        public string? ProductName { get; set; }
        public string? BulletPoint1 { get; set; }
        public string? Price { get; set; }
        public string? PriceSale { get; set; }
        public string? main_image_url { get; set; }
        public string? other_image_url1 { get; set; }
        public string? Size { get; set; }
        public string? Color { get; set; }

        public string? image_standard_1 { get; set; }
        public string? image_standard_2 { get; set; }
        public string? image_standard_3 { get; set; }
        public string? image_standard_4 { get; set; }
        public string? image_standard_5 { get; set; }

        public bool? CheckWishlist { get; set; }
        public Guid? WishListId { get; set; }
        public bool? CheckCart { get; set; }
    }
}
