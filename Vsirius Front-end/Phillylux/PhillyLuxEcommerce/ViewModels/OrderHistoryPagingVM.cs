namespace PhillyLuxEcommerce.ViewModels
{
    public class OrderHistoryPagingVM
    {
        public List<OrderHistoryVM> ListOrderHistory { get; set; }
        public PaginationVM Pagination { get; set; }
    }
}
