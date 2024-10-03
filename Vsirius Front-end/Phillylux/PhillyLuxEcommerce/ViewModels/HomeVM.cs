namespace PhillyLuxEcommerce.ViewModels
{
    public class HomeVM
    {
        public string BaseURLImage { get; set; }
        public List<HomeVMFilter> VsiriusFilterDtos { get; set; }
        public List<HomeVMProduct> VsiriusProductLatest { get; set; }
        public List<HomeVMProduct> VsiriusProductPopular { get; set; }

    }
}
