namespace StullerApiClientDomain.Invoices
{
    public class Shipment
    {
        public int ShipmentHeaderId { get; set; }
        public string ShipToAccountNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalPrice { get; set; }
        public double PostageAndHandling { get; set; }
        public string ShipmentMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingLink { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToProvince { get; set; }
        public string ShipToCountry { get; set; }
        public string ShipToZip { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
