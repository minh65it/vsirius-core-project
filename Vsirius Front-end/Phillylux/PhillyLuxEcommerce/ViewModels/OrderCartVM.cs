namespace PhillyLuxEcommerce.ViewModels
{
    public class OrderCartVM
    {
        public Guid CartId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool? Status { get; set; }
        public int? QuantityTotalItem { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Tax { get; set; }
        public List<OrderProductVM>? ListProductByOrderId { get; set; }
    }
}
