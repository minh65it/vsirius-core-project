namespace WalmartIntegrationDataApi.Models
{
    public class ReturnOrder
    {
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string returnOrderId { get; set; }
        public string customerEmailId { get; set; }
        public string returnType { get; set; }
        public string replacementCustomerOrderId { get; set; }
        public CustomerName customerName { get; set; }
        public string customerOrderId { get; set; }
        public DateTime returnOrderDate { get; set; }
        public DateTime returnByDate { get; set; }
        public string refundMode { get; set; }
        public TotalRefundAmount totalRefundAmount { get; set; }
        public List<ReturnLineGroup> returnLineGroups { get; set; }
        public List<ReturnOrderLine> returnOrderLines { get; set; }
        public ReturnChannel returnChannel { get; set; }
    }
    public class CarrierInfoList
    {
        public string carrierId { get; set; }
        public string carrierName { get; set; }
        public string serviceType { get; set; }
        public string trackingNo { get; set; }
    }

    public class ReturnItemFee
    {
        public string chargeCategory { get; set; }
        public string chargeName { get; set; }
        public ChargePerUnit chargePerUnit { get; set; }
        public bool isDiscount { get; set; }
        public bool isBillable { get; set; }
        public List<Tax> tax { get; set; }
        public ExcessCharge excessCharge { get; set; }
        public List<Reference> references { get; set; }
    }

    public class ChargePerUnit
    {
        public double currencyAmount { get; set; }
        public string currencyUnit { get; set; }
    }

    public class ChargeTotal
    {
        public string name { get; set; }
        public Value value { get; set; }
    }

    public class CurrentTrackingStatus
    {
        public string status { get; set; }
        public DateTime statusTime { get; set; }
        public string currentRefundStatus { get; set; }
        public Quantity quantity { get; set; }
    }

    public class CustomerName
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
    }

    public class ExcessCharge
    {
        public string currencyAmount { get; set; }
        public string currencyUnit { get; set; }
    }

    public class ExcessTax
    {
        public string currencyAmount { get; set; }
        public string currencyUnit { get; set; }
    }

    public class ReturnItemInfo
    {
        public string sku { get; set; }
        public string condition { get; set; }
        public string productName { get; set; }
        public ItemWeight itemWeight { get; set; }
    }

    public class ItemWeight
    {
        public string unitOfMeasure { get; set; }
        public string measurementValue { get; set; }
    }

    public class Label
    {
        public string labelImageURL { get; set; }
        public List<CarrierInfoList> carrierInfoList { get; set; }
    }


    public class Quantity
    {
        public string unitOfMeasure { get; set; }
        public string measurementValue { get; set; }
    }

    public class Reference
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class RefundChannel
    {
        public string refundChannelName { get; set; }
        public Quantity quantity { get; set; }
    }

    public class ReturnChannel
    {
        public string channelName { get; set; }
    }

    public class ReturnLine
    {
        public int returnOrderLineNumber { get; set; }
    }

    public class ReturnLineGroup
    {
        public int groupNo { get; set; }
        public List<ReturnLine> returnLines { get; set; }
        public List<Label> labels { get; set; }
        public bool returnExpectedFlag { get; set; }
    }

    public class ReturnOrderLine
    {
        public int returnOrderLineNumber { get; set; }
        public int salesOrderLineNumber { get; set; }
        public string returnReason { get; set; }
        public string purchaseOrderId { get; set; }
        public string sellerOrderId { get; set; }
        public int purchaseOrderLineNumber { get; set; }
        public bool isReturnForException { get; set; }
        public string exceptionItemType { get; set; }
        public string rechargeReason { get; set; }
        public string returnCancellationReason { get; set; }
        public ReturnItemInfo item { get; set; }
        public List<ReturnItemFee> charges { get; set; }
        public UnitPrice unitPrice { get; set; }
        public List<ChargeTotal> chargeTotals { get; set; }
        public string currentDeliveryStatus { get; set; }
        public string currentRefundStatus { get; set; }
        public List<CurrentTrackingStatus> currentTrackingStatuses { get; set; }
        public List<RefundChannel> refundChannels { get; set; }
        public int cancellableQty { get; set; }
        public Quantity quantity { get; set; }
        public bool returnExpectedFlag { get; set; }
        public bool isFastReplacement { get; set; }
        public bool isKeepIt { get; set; }
        public bool lastItem { get; set; }
        public string refundedQty { get; set; }
        public string rechargeableQty { get; set; }
        public string refundChannel { get; set; }
        public List<ReturnTrackingDetail> returnTrackingDetail { get; set; }
        public string status { get; set; }
        public DateTime statusTime { get; set; }
    }

    public class ReturnTrackingDetail
    {
        public int sequenceNo { get; set; }
        public string eventTag { get; set; }
        public string eventDescription { get; set; }
        public DateTime eventTime { get; set; }
        public List<Reference> references { get; set; }
    }

    public class Tax
    {
        public string taxName { get; set; }
        public ExcessTax excessTax { get; set; }
        public TaxPerUnit taxPerUnit { get; set; }
    }

    public class TaxPerUnit
    {
        public double currencyAmount { get; set; }
        public string currencyUnit { get; set; }
    }

    public class TotalRefundAmount
    {
        public double currencyAmount { get; set; }
        public string currencyUnit { get; set; }
    }

    public class UnitPrice
    {
        public double currencyAmount { get; set; }
        public string currencyUnit { get; set; }
    }

    public class Value
    {
        public double currencyAmount { get; set; }
        public string currencyUnit { get; set; }
    }


}
