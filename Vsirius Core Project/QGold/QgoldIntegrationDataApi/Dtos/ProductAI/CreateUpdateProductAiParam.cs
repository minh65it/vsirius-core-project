namespace QgoldIntegrationDataApi.Dtos.ProductAI
{
    public class CreateUpdateProductAiParam
    {
        public required string SKU { get; set; }
        public required string Description { get; set; }
        public required string Brand { get; set; }
        public required string ImgUrl { get; set; }
    }
}
