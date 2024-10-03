namespace PhillyLuxEcommerce.ViewModels
{
    public class SearchVM
    {
        public List<SummaryProductVM>? lstSearchProduct { get; set; }
        public List<FilterVM>? lstFilter { get; set; }
        public PaginationVM? pagination { get; set; }
    }

}
