namespace ProductApi.Dtos
{
    public class VsiriusHomeProductDto
    {
        public string item_sku { get; set; }
        public string? item_name { get; set; }
        public bool? IsPopular { get; set; }
        public bool? IsLatest { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDelete { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }

    public class VsiriusHomeProductParams
    {
        public string Filter { get; set; }
        public int TakeProduct { get; set; } = 20;
    }
    public class VsiriusTopParams
    {
        public string? Filter { get; set; }
        /// <summary>
        /// Multiple values, separated by |,no spaces , '|'
        /// </summary>
        public string? feed_product_type { get; set; }
        public string? keyword { get; set; }
    }

    public static class VsiriusHomeProductFilterParams
    {
        public static string FilterPopular = "popular";
        public static string FilterLatest = "latest";
    }
}
