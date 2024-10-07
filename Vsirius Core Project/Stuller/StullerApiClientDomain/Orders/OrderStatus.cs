namespace StullerApiClientDomain.Orders
{
    public class OrderStatusResponse
    {
        public List<OrderStatuses> OrderStatuses { get; set;}
    }
        public class OrderStatusDetail
    {
        public int LineNumber { get; set; }
        public string ItemNumber { get; set; }
        public int OrderQuantity { get; set; }
        public string OrderQuantityUnitOfMeasure { get; set; }
        public int ShipQuantity { get; set; }
        public string ShipQuantityUnitOfMeasure { get; set; }
        public double UnitPrice { get; set; }
        public string UnitPriceUnitOfMeasure { get; set; }
    }

    public class OrderStatuses
    {
        public string OrderNumber { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string Status { get; set; }
        public double OrderTotal { get; set; }
        public double InvoiceTotal { get; set; }
        public double SalesTax { get; set; }
        public double PostageAndHandling { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ScheduledShipDate { get; set; }
        public DateTime ActualShipDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ShipMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string TrackingLink { get; set; }
        public List<OrderStatusDetail> OrderStatusDetails { get; set; }
    }


}
