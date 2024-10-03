namespace PhillyLuxEcommerce.ViewModels
{
    public class WishlistVM
    {
        public Guid? Id { get; set; }

        public string? UserId { get; set; }

        public string? ProductId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? item_sku { get; set; }
        public string? item_name { get; set; }

        public string? main_image_url { get; set; }
        public string? quantity { get; set; }
        public string? standard_price { get; set; }
        public string? price_seo { get; set; }
        public string? bullet_point { get; set; }
    }
}
