namespace WalmartIntegrationDataApi.Dtos.SellerItem
{
    public class DeleteSKU
    {
        public RetireItemHeader RetireItemHeader { get; set; }
        public List<RetireItem> RetireItem { get; set; }
    }

    public class RetireItem
    {
        public string sku { get; set; }
    }

    public class RetireItemHeader
    {
        public DateTime feedDate { get; set; }
        public string version { get; set; }
    }
}
