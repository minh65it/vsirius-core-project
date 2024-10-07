namespace WalmartIntegrationDataApi.Dtos.SellerItem
{
    public class WalmartInventoryUpload
    {
        public InventoryHeader? InventoryHeader { get; set; }
        public List<Inventory>? Inventory { get; set; }


    }
    public class InventoryHeader
    {
        public string? version { get; set; } = "1.4";

    }

    public class Inventory
    {
        public string? sku { get; set; }
        public quantity? quantity { get; set; }

    }
    public class quantity
    {
        public string? unit { get; set; } = "EACH";
        public int? amount { get; set; }

    }
}
