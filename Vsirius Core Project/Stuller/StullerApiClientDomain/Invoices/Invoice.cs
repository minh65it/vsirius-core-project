namespace StullerApiClientDomain.Invoices
{
    public class InvoiceResponse
    {
        public List<Invoice> Invoices { get; set; }
    }
    public class Invoice
    {
        public string OrderNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string ShipToAccountNumber { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToProvince { get; set; }
        public string ShipToCountry { get; set; }
        public string Status { get; set; }
        public double OrderTotal { get; set; }
        public double InvoiceTotal { get; set; }
        public double TotalDue { get; set; }
        public double SalesTax { get; set; }
        public double PostageAndHandling { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShipMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingLink { get; set; }
        public double GoldMarketRate { get; set; }
        public double GoldMarketBase { get; set; }
        public string GoldMarketAdder { get; set; }
        public double PlatinumMarketRate { get; set; }
        public double SilverMarketRate { get; set; }
        public double PlatinumMarketBase { get; set; }
        public double SilverMarketBase { get; set; }
        public string PlatinumMarketAdder { get; set; }
        public string SilverMarketAdder { get; set; }
        public List<InvoiceDetails> InvoiceDetails { get; set; }
    }
}
