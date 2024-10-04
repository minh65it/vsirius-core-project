namespace AmazonIntegrationDataApi.Dtos.MongoDB
{
    public class ReturnFBMOrderRowDto
    {
        public string OrderID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ReturnRequestDate { get; set; }
        public string ReturnRequestStatus { get; set; }
        public string AmazonRMAID { get; set; }
        public string MerchantRMAID { get; set; }
        public string LabelType { get; set; }
        public string LabelCost { get; set; }
        public string CurrencyCode { get; set; }
        public string ReturnCarrier { get; set; }
        public string TrackingID { get; set; }
        public string LabelToBePaidBy { get; set; }
        public bool AToZClaim { get; set; }
        public bool IsPrime { get; set; }
        public string ASIN { get; set; }
        public string MerchantSKU { get; set; }
        public string ItemName { get; set; }
        public int? ReturnQuantity { get; set; }
        public string ReturnReason { get; set; }

        public bool InPolicy { get; set; }
        public string ReturnType { get; set; }
        public string Resolution { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? ReturnDeliveryDate { get; set; }
        public decimal? OrderAmount { get; set; }
        public int? OrderQuantity { get; set; }
        public string SafeTActionReason { get; set; }
        public string SafeTClaimId { get; set; }
        public string SafeTClaimState { get; set; }
        public string SafeTClaimCreationTime { get; set; }
        public string SafeTClaimReimbursementAmount { get; set; }
        public decimal? RefundedAmount { get; set; }
        public string Category { get; set; }
        public string refNumber { get; set; }
        //======================================claim=====================================//
        public string DocumentType { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketplaceOrderId { get; set; }
        public string? RMA { get; set; }
        public List<string> Notes { get; set; }
        public List<ImageInfo> Images { get; set; }
        public DateTime? ScanDate { get; set; }
    }
    public class ReturnFBMOrderParams
    {
        public string? OrderID { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? OrderDateFrom { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? OrderDateTo { get; set; }
        /// <summary>
        /// AmazonRMAID
        /// </summary>
        public string? AmazonRMAID { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? ReturnRequestDateFrom { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? ReturnRequestDateTo { get; set; }
        public string? TrackingID { get; set; }
        public string? ReturnRequestStatus { get; set; }
        /// <summary>
        /// Product name
        /// </summary>
        public string? ItemName { get; set; }
        public bool? IsClaim { get; set; }
    }

}
