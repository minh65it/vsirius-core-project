namespace WalmartIntegrationDataApi.Dtos.Order
{
    public class UnshippedOrderIdsParam
    {
        public string? Provider { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
