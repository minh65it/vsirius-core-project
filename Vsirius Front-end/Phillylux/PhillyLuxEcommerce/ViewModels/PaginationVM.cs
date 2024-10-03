namespace PhillyLuxEcommerce.ViewModels
{
    public class PaginationVM
    {
        public int totalCount { get; set; }
        public int totalPage { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int skip { get; set; }
    }
}
