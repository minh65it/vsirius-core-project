namespace WalmartIntegrationDataApi.DtosOrderStuller
{
    public class SearchOrderStullerParam
    {
        public string? OrderID { get; set; }
        /// <summary>
        /// format: MM/dd/yyyy
        /// </summary>
        public string? FromDate { get; set; }
        /// <summary>
        /// format: MM/dd/yyyy
        /// </summary>
        public string? ToDate { get; set;}
        public string? SKU { get; set; }
        public string? Status { get; set; }
        public string? BuyerName { get; set; }
        public string? Marketplace { get; set; }    
        public string? MarketplaceOrderId { get; set; }    
    }
}
