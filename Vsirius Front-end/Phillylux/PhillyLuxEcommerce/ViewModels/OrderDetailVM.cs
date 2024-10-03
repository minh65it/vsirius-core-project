namespace PhillyLuxEcommerce.ViewModels
{
    public class OrderDetailVM
    {
        public OrderInformationVM OrderInformation { get; set; }
        public List<OrderProductVM> ListProductByOrderId { get; set; }
        public List<OrderStatusVM> ListOrderStatus { get; set; }
    }
}
