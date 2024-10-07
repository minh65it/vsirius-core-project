namespace WalmartIntegrationDataApi.Dtos.Order
{
    public class UpdateStatusOrder
    {
        public string purchaseOrderId { get; set; }
        public OrderShipment orderShipment { get; set; }
    }
    public class UpdateStatusOrderApi
    {
        public OrderShipment orderShipment { get; set; }
    }
    public class Asn
    {
        public string packageASN { get; set; }
        public string palletASN { get; set; }
    }

    public class UCarrierName
    {
        public object otherCarrier { get; set; }
        public string carrier { get; set; }
    }

    public class UOrderLine
    {
        public string lineNumber { get; set; }
        public UOrderLineStatuses orderLineStatuses { get; set; }
    }

    public class UOrderLines
    {
        public List<UOrderLine> orderLine { get; set; }
    }

    public class UOrderLineStatus
    {
        public string status { get; set; }
        public Asn asn { get; set; }
        public UStatusQuantity statusQuantity { get; set; }
        public UTrackingInfo trackingInfo { get; set; }
    }

    public class UOrderLineStatuses
    {
        public List<UOrderLineStatus> orderLineStatus { get; set; }
    }

    public class OrderShipment
    {
        public UOrderLines orderLines { get; set; }
    }

    public class UStatusQuantity
    {
        public string unitOfMeasurement { get; set; }
        public string amount { get; set; }
    }

    public class UTrackingInfo
    {
        public long shipDateTime { get; set; }
        public UCarrierName carrierName { get; set; }
        public string methodCode { get; set; }
        public string trackingNumber { get; set; }
        public string trackingURL { get; set; }
    }
}
