namespace WalmartIntegrationDataApi.Dtos.SellerItem
{
    public class TemplateMapShipping
    {
        public ItemFeedHeader ItemFeedHeader { get; set; }
        public List<ItemAdd> Item { get; set; }
    }
    public class ItemAdd
    {
        public PreciseDelivery PreciseDelivery { get; set; }
    }

    public class ItemFeedHeader
    {
        public string sellingChannel { get; set; }
        public string locale { get; set; }
        public string version { get; set; }
    }

    public class PreciseDelivery
    {
        public string sku { get; set; }
        public string actionType { get; set; }
        public string shippingTemplateId { get; set; }
        public string fulfillmentCenterId { get; set; }
    }
}
