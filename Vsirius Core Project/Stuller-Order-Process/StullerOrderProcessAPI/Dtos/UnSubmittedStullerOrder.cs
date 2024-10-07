namespace StullerOrderProcessAPI.Dtos
{
    public class UnSubmittedStullerOrder
    {
        public string PO { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string ShipMethod { get; set; } = string.Empty;
        public List<UnSubmitItemStuller> Items { get; set; } = new();
    }

    public class UnSubmitItemStuller
    {
        public string Sku { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
    }

    public enum ShipMethod
    {
        UPSNextDayAir,
        UPS2ndDayAir,
        UPS3rdDayAirSelect,
        UPSGround,
        FedExPriority,
        FedEx2ndBusinessDayPM,
        FedEx3DaySaver,
        FedExHomeDelivery,
        USPSExpressMailParcel,
        USPSPriorityMailParcel,
        USPS1stClassMailParcel
    }
}
