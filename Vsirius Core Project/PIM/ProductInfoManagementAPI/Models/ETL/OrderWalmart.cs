using MongoDB.Bson.Serialization.Attributes;

namespace PimApi.Models.ETL
{
    public class OrderWalmart
    {
        [BsonId]
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string purchaseOrderId { get; set; }
        public string customerOrderId { get; set; }
        public string customerEmailId { get; set; }
        public string orderType { get; set; }
        public string originalCustomerOrderID { get; set; }
        public long orderDate { get; set; }
        public ShippingInfo shippingInfo { get; set; }
        public OrderLines orderLines { get; set; }
        public DateTime? PullUpdateDate { get; set; }
    }
    public class OrderWalmartSellerDto
    {
        public OrderList list { get; set; }
    }
    public class OrderLine
    {
        public string lineNumber { get; set; }
        public OrderItem item { get; set; }
        public Charges charges { get; set; }
        public OrderLineQuantity orderLineQuantity { get; set; }
        public object statusDate { get; set; }
        public OrderLineStatuses orderLineStatuses { get; set; }
        public Fulfillment fulfillment { get; set; }
    }

    public class OrderLineQuantity
    {
        public string unitOfMeasurement { get; set; }
        public string amount { get; set; }
    }

    public class OrderLines
    {
        public List<OrderLine> orderLine { get; set; }
    }

    public class OrderLineStatus
    {
        public string status { get; set; }
        public StatusQuantity statusQuantity { get; set; }
        public TrackingInfo trackingInfo { get; set; }
    }

    public class OrderLineStatuses
    {
        public List<OrderLineStatus> orderLineStatus { get; set; }
    }

    public class PostalAddress
    {
        public string name { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public string addressType { get; set; }
    }

    public class ShippingInfo
    {
        public string phone { get; set; }
        public long estimatedDeliveryDate { get; set; }
        public long estimatedShipDate { get; set; }
        public string methodCode { get; set; }
        public PostalAddress postalAddress { get; set; }
    }

    public class StatusQuantity
    {
        public string unitOfMeasurement { get; set; }
        public string amount { get; set; }
    }

    public class OrderTax
    {
        public string taxName { get; set; }
        public TaxAmount taxAmount { get; set; }
    }

    public class TaxAmount
    {
        public string currency { get; set; }
        public double amount { get; set; }
    }

    public class TrackingInfo
    {
        public object shipDateTime { get; set; }
        public CarrierName carrierName { get; set; }
        public string methodCode { get; set; }
        public string trackingNumber { get; set; }
        public string trackingURL { get; set; }
    }

    public class CarrierName
    {
        public string otherCarrier { get; set; }
        public string carrier { get; set; }
    }

    public class OrderCharge
    {
        public string chargeType { get; set; }
        public string chargeName { get; set; }
        public ChargeAmount chargeAmount { get; set; }
        public OrderTax tax { get; set; }
    }

    public class ChargeAmount
    {
        public string currency { get; set; }
        public double amount { get; set; }
    }

    public class Charges
    {
        public List<OrderCharge> charge { get; set; }
    }

    public class Elements
    {
        public List<Order> order { get; set; }
    }

    public class Fulfillment
    {
        public string fulfillmentOption { get; set; }
        public string shipMethod { get; set; }
        public object pickUpDateTime { get; set; }
    }

    public class OrderItem
    {
        public string productName { get; set; }
        public string sku { get; set; }
    }

    public class OrderList
    {
        public Meta meta { get; set; }
        public Elements elements { get; set; }
    }

    public class Meta
    {
        public int totalCount { get; set; }
        public int limit { get; set; }
        public string nextCursor { get; set; }
    }
}
