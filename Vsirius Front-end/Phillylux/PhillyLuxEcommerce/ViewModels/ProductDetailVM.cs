namespace PhillyLuxEcommerce.ViewModels
{
    public class ProductDetailVM
    {
        //Main
        public string? Title { get; set; }
        public string? Price { get; set; }
        public string? SKU { get; set; }
        public string? Quantity { get; set; }
        public string? Description { get; set; }
        public string? BulletPoints { get; set; }

        //Image
        public string? main_image_url { get; set; }
        public string? other_image_url1 { get; set; }
        public string? other_image_url2 { get; set; }
        public string? other_image_url3 { get; set; }
        public string? other_image_url4 { get; set; }
        public string? other_image_url5 { get; set; }
        public string? other_image_url6 { get; set; }
        public string? other_image_url7 { get; set; }
        public string? other_image_url8 { get; set; }

        //AdditionalInformation
        public string? ItemType { get; set; }
        public string? MetalType { get; set; }
        public string? Color { get; set; }

        public bool? CheckWishlist { get; set; }
        public Guid? WishListId { get; set; }


        //RelatedProduct
        public List<SummaryProductVM>? RelatedProducts { get; set; }
    }

    public class ImageGalleryVM
    {
        public string? imageUrl { get; set; }
    }

}
