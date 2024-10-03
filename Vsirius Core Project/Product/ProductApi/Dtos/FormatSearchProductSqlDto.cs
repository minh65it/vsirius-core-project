namespace ProductApi.Dtos
{
    public class FormatSearchProductSqlDto
    {
        public string? feed_product_type { get; set; }
        public string? item_type { get; set; }
        public string? metal_type { get; set; }
        public string? gem_type1 { get; set; }
        public string? color_map { get; set; }
        //result
        public string? item_sku { get; set; }
        public string? item_name { get; set; }
        public string? bullet_point1 { get; set; }
        public decimal standard_price { get; set; }
        public string? sale_price { get; set; }
        public string? color_name { get; set; }
        public string? size_name { get; set; }
        public string? main_image_url { get; set; }
        public string? other_image_url1 { get; set; }
        public string? image_standard_1 { get; set; }
        public string? image_standard_2 { get; set; }
        public string? image_standard_3 { get; set; }
        public string? image_standard_4 { get; set; }
        public string? image_standard_5 { get; set; }
        public string? quantity { get; set; }
    }
}
