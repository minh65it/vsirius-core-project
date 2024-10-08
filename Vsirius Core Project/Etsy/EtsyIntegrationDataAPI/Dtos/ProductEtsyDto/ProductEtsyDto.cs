namespace EtsyIntegrationDataAPI.Dtos.ProductEtsyDto
{
    public class ProductEtsyDto
    {
        public string user_id { get; set; }
        public string shop_id { get; set; }
        public string item_sku { get; set; }
        public int quantity { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public PriceDto price { get; set; }
        public string who_made { get; set; }
        public string when_made { get; set; }
        public long taxonomy_id { get; set; }
        public long listing_id { get; set; }
        public long? shipping_profile_id { get; set; }
        public long? return_policy_id { get; set; }
        public string[]? materials { get; set; }
        public long? shop_section_id { get; set; }
        public decimal? processing_min { get; set; }
        public decimal? processing_max { get; set; }
        public string[]? tags { get; set; }
        public string[]? styles { get; set; }
        public float? item_weight { get; set; }
        public float? item_length { get; set; }
        public float? item_width { get; set; }
        public float? item_height { get; set; }
        public string item_weight_unit { get; set; }
        public string item_dimensions_unit { get; set; }
        public bool? is_personalizable { get; set; }
        public bool? personalization_is_required { get; set; }
        public int? personalization_char_count_max { get; set; }
        public int? production_partner_ids { get; set; }
        public long[]? image_ids { get; set; }
        public bool is_supply { get; set; }
        public bool is_customizable { get; set; }
        public bool should_auto_renew { get; set; }
        public bool is_taxable { get; set; }
        public string type { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? updatedDate { get; set; }
    }
    public class PriceDto
    {
        public decimal amount { get; set; }
        public decimal? divisor { get; set; }
        public string? currency_code { get; set; }
    }
    public class AllListings 
    {
        public int count { get; set; }
        public List<ProductEtsyDto> results { get; set; }
    }

}
