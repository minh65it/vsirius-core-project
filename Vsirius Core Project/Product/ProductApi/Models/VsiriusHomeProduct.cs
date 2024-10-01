namespace ProductApi.Models
{
    public class VsiriusHomeProduct : VsiriusDataModel.DataModel.IVsiriusHomeProduct
    {
        public string item_sku { get; set; }
        public bool? IsPopular { get; set; }
        public bool? IsLatest { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
