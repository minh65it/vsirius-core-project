namespace AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor
{
    public class UnshipOrderDto
    {
        public string? CustomerOrderId { get; set; }
        public List<ItemUnShip> ItemUnShip { get; set; } = new();
    }

    public class ItemUnShip
    {
        public string? Sku { get; set; }
        public string? Quantity { get; set; }
    }
}
