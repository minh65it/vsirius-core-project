namespace PhillyLuxEcommerce.Dtos.Params
{
    public class SearchTopProductParam
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Keyword { get; set; }
        public string? Filter { get; set; }
        public string? feed_product_type { get; set; }
    }
}
