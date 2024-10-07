namespace WalmartIntegrationDataApi.Dtos.Return
{
    public class SearchReturnParam
    {
        /// <summary>
        /// 1: INITIATED, 2: DELIVERED, 3: COMPLETED
        /// </summary>
        public StatusReturn? StatusReturn { get; set; }
        public string? CustomerName { get; set; }
        public string? PurchaseOrderID { get; set; }
        public string? ReturnOrderID { get; set; }
        public string? SKU { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? FromDate { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? ToDate { get; set; }

    }

    public enum StatusReturn
    {
        INITIATED,
        DELIVERED,
        COMPLETED
    }
}
