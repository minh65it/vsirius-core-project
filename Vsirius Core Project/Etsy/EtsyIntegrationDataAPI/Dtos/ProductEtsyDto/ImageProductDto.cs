namespace EtsyIntegrationDataAPI.Dtos.ProductEtsyDto
{
    public class ImageProductDto
    {
        public string item_sku { get; set; }
        public byte[] image { get; set; }
        public long listing_image_id { get; set; }
        public long? listing_id { get; set; }
        public int rank { get; set; } = 1;
        public bool overwrite { get; set; } = false;
        public bool is_watermarked { get; set; } = false;
        public string alt_text { get; set; } = "";
    }
}
