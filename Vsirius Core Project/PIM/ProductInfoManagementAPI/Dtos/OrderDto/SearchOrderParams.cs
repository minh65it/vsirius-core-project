namespace PimApi.Dtos.OrderDto
{
    public class SearchOrderParams
    {
        public string? ItemSku { get; set; }
        public string? ShipToName { get; set; }
        public string? Marketplace { get; set; }
        public bool? isSortQuantity { get; set; }
        public bool? isGroupQuantity { get; set; }
        public bool? isPagging { get; set; }
    }
}
