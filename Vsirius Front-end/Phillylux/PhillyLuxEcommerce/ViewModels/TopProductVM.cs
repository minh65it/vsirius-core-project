namespace PhillyLuxEcommerce.ViewModels
{
    public class TopProductVM
    {
        public bool IsLatest { get; set; } = false;
        public List<SummaryProductVM>? lstSearchProduct { get; set; }
        public PaginationVM? pagination { get; set; }
    }
}
