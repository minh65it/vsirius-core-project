namespace EbayCoreApi.Dtos
{
    public class PriceQuantitySkuDto
    {
        public string Sku { get; set; }
        public string ItemID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}