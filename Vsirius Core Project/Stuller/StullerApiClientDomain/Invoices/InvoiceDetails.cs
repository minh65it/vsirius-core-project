namespace StullerApiClientDomain.Invoices
{
    public class InvoiceDetails
    {
        public int LineNumber { get; set; }
        public string CustomerLineReference { get; set; }
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public double ShipQuantity { get; set; }
        public int BackOrderedQuantity { get; set; }
        public string LaborOtherCharges { get; set; }
        public string TotalWeight { get; set; }
        public double UnitPrice { get; set; }
        public double LineTotal { get; set; }
        public string CustomerNotes { get; set; }
        public string UnitPriceUnitOfMeasure { get; set; }
    }
}
