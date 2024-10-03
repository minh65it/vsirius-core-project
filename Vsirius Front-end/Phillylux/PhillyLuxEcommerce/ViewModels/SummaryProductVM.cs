namespace PhillyLuxEcommerce.ViewModels
{
    public class SummaryProductVM
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Price { get; set; }
        public string SalePrice { get; set; }
        public string MainImageUrl { get; set; }
        public string SubImageUrl { get; set; }
        public string image_standard_1 { get; set; }
        public string image_standard_2 { get; set; }
        public string image_standard_3 { get; set; }
        public string image_standard_4 { get; set; }
        public string image_standard_5 { get; set; }
        public bool? CheckWishlist { get; set; }
        public Guid? WishListId { get; set; }
        public bool? CheckCart { get; set; }
    }
}
