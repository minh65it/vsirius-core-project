namespace AmazonIntegrationDataApi.Models.OrderAmazonProcessor
{
    public class OrderAmazon
    {
        //[BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string OrderStatus { get; set; }
        public string FulfillmentChannel { get; set; }
        public string PaymentMethod { get; set; }
        public object EasyShipShipmentStatus { get; set; }
        public string OrderType { get; set; }
        public object BuyerInvoicePreference { get; set; }
        public object ElectronicInvoiceStatus { get; set; }
        public string AmazonOrderId { get; set; }
        public object SellerOrderId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string SalesChannel { get; set; }
        public object OrderChannel { get; set; }
        public string ShipServiceLevel { get; set; }
        public OrderTotal OrderTotal { get; set; }
        public int NumberOfItemsShipped { get; set; }
        public int NumberOfItemsUnshipped { get; set; }
        public object PaymentExecutionDetail { get; set; }
        public List<string> PaymentMethodDetails { get; set; }
        public string MarketplaceId { get; set; }
        public string ShipmentServiceLevelCategory { get; set; }
        public object CbaDisplayableShippingLabel { get; set; }
        public DateTime EarliestShipDate { get; set; }
        public DateTime LatestShipDate { get; set; }
        public DateTime EarliestDeliveryDate { get; set; }
        public DateTime LatestDeliveryDate { get; set; }
        public bool IsBusinessOrder { get; set; }
        public bool IsPrime { get; set; }
        public bool IsPremiumOrder { get; set; }
        public bool IsGlobalExpressEnabled { get; set; }
        public object ReplacedOrderId { get; set; }
        public bool IsReplacementOrder { get; set; }
        public object PromiseResponseDueDate { get; set; }
        public object IsEstimatedShipDateSet { get; set; }
        public bool IsSoldByAB { get; set; }
        public object IsIBA { get; set; }
        public DefaultShipFromLocationAddress DefaultShipFromLocationAddress { get; set; }
        public object BuyerTaxInformation { get; set; }
        public object FulfillmentInstruction { get; set; }
        public bool IsISPU { get; set; }
        public bool IsAccessPointOrder { get; set; }
        public object MarketplaceTaxInfo { get; set; }
        public object SellerDisplayName { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public BuyerInfo BuyerInfo { get; set; }
        public AutomatedShippingSettings AutomatedShippingSettings { get; set; }
        public bool HasRegulatedItems { get; set; }
        public object ItemApprovalTypes { get; set; }
        public object ItemApprovalStatus { get; set; }
        public List<OrderItemList> OrderItemList { get; set; }
        public bool IsUpdated { get; set; }
    }
    public class AutomatedShippingSettings
    {
        public bool HasAutomatedShippingSettings { get; set; }
        public object AutomatedCarrier { get; set; }
        public object AutomatedShipMethod { get; set; }
    }

    public class BuyerInfo
    {
        public string BuyerEmail { get; set; }
        public object BuyerName { get; set; }
        public object BuyerCounty { get; set; }
        public object BuyerTaxInfo { get; set; }
        public object PurchaseOrderNumber { get; set; }
        public object BuyerCustomizedInfo { get; set; }
        public object GiftWrapPrice { get; set; }
        public object GiftWrapTax { get; set; }
        public object GiftMessageText { get; set; }
        public object GiftWrapLevel { get; set; }
    }

    public class BuyerRequestedCancel
    {
        public bool IsBuyerRequestedCancel { get; set; }
        public string BuyerCancelReason { get; set; }
    }

    public class DefaultShipFromLocationAddress
    {
        public object AddressType { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public object AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public object County { get; set; }
        public object District { get; set; }
        public string StateOrRegion { get; set; }
        public object Municipality { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public object Phone { get; set; }
    }

    //public class Id
    //{
    //    [JsonProperty("$oid")]
    //    public string oid { get; set; }
    //}

    public class ItemPrice
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class ItemTax
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class OrderItemList
    {
        public object DeemedResellerCategory { get; set; }
        public string ASIN { get; set; }
        public string SellerSKU { get; set; }
        public string OrderItemId { get; set; }
        public string Title { get; set; }
        public int QuantityOrdered { get; set; }
        public int QuantityShipped { get; set; }
        public ProductInfo ProductInfo { get; set; }
        public object PointsGranted { get; set; }
        public ItemPrice ItemPrice { get; set; }
        public ShippingPrice ShippingPrice { get; set; }
        public ItemTax ItemTax { get; set; }
        public ShippingTax ShippingTax { get; set; }
        public ShippingDiscount ShippingDiscount { get; set; }
        public ShippingDiscountTax ShippingDiscountTax { get; set; }
        public PromotionDiscount PromotionDiscount { get; set; }
        public PromotionDiscountTax PromotionDiscountTax { get; set; }
        public object PromotionIds { get; set; }
        public object CODFee { get; set; }
        public object CODFeeDiscount { get; set; }
        public bool IsGift { get; set; }
        public object ConditionNote { get; set; }
        public string ConditionId { get; set; }
        public string ConditionSubtypeId { get; set; }
        public object ScheduledDeliveryStartDate { get; set; }
        public object ScheduledDeliveryEndDate { get; set; }
        public object PriceDesignation { get; set; }
        public TaxCollection TaxCollection { get; set; }
        public object SerialNumberRequired { get; set; }
        public bool IsTransparency { get; set; }
        public object IossNumber { get; set; }
        public object StoreChainStoreId { get; set; }
        public BuyerInfo BuyerInfo { get; set; }
        public BuyerRequestedCancel BuyerRequestedCancel { get; set; }
        public object ItemApprovalContext { get; set; }
        public object SerialNumbers { get; set; }
    }

    public class OrderTotal
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class ProductInfo
    {
        public int NumberOfItems { get; set; }
    }

    public class PromotionDiscount
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class PromotionDiscountTax
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class ShippingAddress
    {
        public string AddressType { get; set; }
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string District { get; set; }
        public string StateOrRegion { get; set; }
        public string Municipality { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
    }

    public class ShippingDiscount
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class ShippingDiscountTax
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class ShippingPrice
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }

    public class ShippingTax
    {
        public string CurrencyCode { get; set; }
        public string Amount { get; set; }
    }
    public class TaxCollection
    {
        public object Model { get; set; }
        public object ResponsibleParty { get; set; }
    }
}
