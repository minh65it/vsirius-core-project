using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;

namespace AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor
{

    public class QgoldOrderDto
    {
        public string? OrderQgoldId { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderDate { get; set; }
        public string? BuyerName { get; set; }
        public string? Address1 { get; set; }
        public string? ZipCode { get; set; }
        public string? Tracking { get; set; }
        public string? ShipVia { get; set; }
        public List<QgoldInvoiceItem>? Items { get; set; }
        public string? Total { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketOrderId { get; set; }
        public bool? IsSendMailLostPackage { get; set; }
    }

    public class QgoldOrderDetailDto
    {
        public string? OrderQgoldId { get; set; }
        public string? POQgold { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public string? Date { get; set; }
        public string? Total { get; set; }
        public string? ShipToText { get; set; }
        public string? ShipToAddress1 { get; set; }
        public string? ZipCode { get; set; }
        public string? SoldToText { get; set; }
        public string? SoldToAddress1 { get; set; }
        public string? SoldToAddress2 { get; set; }
        public string? DocumentType { get; set; }
        public string? PlacedBy { get; set; }
        public string? PlacedVia { get; set; }
        public string? ShipVia { get; set; }
        public string? PackagingOption { get; set; }
        public string? Tracking { get; set; }
        public string? DueDate { get; set; }
        public string? Term { get; set; }
        public string? ShipViaCode { get; set; }
        public string? ShipOptionCode { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketOrderId { get; set; }
        public string? ShipToPhone { get; set; }
        public List<QgoldInvoiceItem>? Items { get; set; }
        public bool? IsSendMailLostPackage { get; set; }
    }

    public class SearchQgoldProcessParam
    {
        public string? OrderQgoldId { get; set; }
        /// <summary>
        /// 0:Invoiced,1:Cancelled,2:Backordered,3:Hold,4:Pending,5:Other
        /// </summary>
        public OrderQgoldStatus? OrderStatus { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? FromDate { get; set; }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string? ToDate { get; set; }
        public string? BuyerName { get; set; }
        public string? Style { get; set; }
        public string? Marketplace { get; set; }
        public string? MarketOrderId { get; set; }
    }

    public enum OrderQgoldStatus
    {
        Invoiced,
        Cancelled,
        Backordered,
        Hold,
        Pending,
        Other
    }
}
