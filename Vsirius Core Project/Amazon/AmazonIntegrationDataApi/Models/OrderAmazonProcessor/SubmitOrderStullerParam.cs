namespace AmazonIntegrationDataApi.Models.OrderAmazonProcessor
{
    public class SubmitOrderStullerParam
    {
        public string ShipToName { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountryCode { get; set; }
        public string ShipToPhone { get; set; }
        public List<QgoldFtpOrderItem> Item { get; set; }
        public string Description { get; set; }
        public string ShipMethodCode { get; set; }
        public string ShipOptionCode { get; set; }
        public string SellerOrderID { get; set; }
        public string SellerName { get; set; }
        public DateTime TimeStamp { get; set; } //DateTime.Now
    }
    public class SubmitOrderDTO
    {
        public CustomerData CustomerData { get; set; }
        public Contact Contact { get; set; }
        public Payment Payment { get; set; }
        public ShipToAddress ShipToAddress { get; set; }
        public BillToAddress BillToAddress { get; set; }
        public List<Line> Lines { get; set; }
        public string Type { get; set; }
        public double Version { get; set; }
        public string Token { get; set; }
        public string Account { get; set; }
        public string OrderID { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string IfOosType { get; set; }
        public bool TestMode { get; set; }
        public string StoreNumber { get; set; }
        public string Instructions { get; set; }
        public string PackingSlipPath { get; set; }
        public string OrderStatusPostToUrl { get; set; }
    }
    public class AdditionalData
    {
    }

    public class Address
    {
        public string Name { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }

    public class BillToAddress
    {
        public Address Address { get; set; }
        public bool SameAsShipTo { get; set; }
    }

    public class Chain
    {
        public string Number { get; set; }
    }

    public class Clasp
    {
        public string Number { get; set; }
    }

    public class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
    }

    public class CustomerData
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public EmailConfirmation EmailConfirmation { get; set; }
        public ExtraAmount ExtraAmount { get; set; }
        public double FreightAmount { get; set; }
        public double TaxAmount { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public AdditionalData AdditionalData { get; set; }
    }

    public class EarringBack
    {
        public string Number { get; set; }
    }

    public class EmailConfirmation
    {
        public string FromAddress { get; set; }
        public bool SendOrderConfirmation { get; set; }
        public bool SendShipmentConfirmation { get; set; }
        public string ToAddress { get; set; }
    }

    public class Engraving
    {
        public List<EngravingLine> EngravingLine { get; set; }
        public int Location { get; set; }
        public string EngravingType { get; set; }
        public string FontType { get; set; }
        public string FontSize { get; set; }
        public string FillOption { get; set; }
        public string FillColor { get; set; }
        public string SpecialFinish { get; set; }
    }

    public class EngravingLine
    {
        public int LineLocation { get; set; }
        public string Text { get; set; }
    }

    public class ExtraAmount
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class Item
    {
        public Clasp Clasp { get; set; }
        public List<Stone> Stones { get; set; }
        public List<Engraving> Engravings { get; set; }
        public EarringBack EarringBack { get; set; }
        public Chain Chain { get; set; }
        public List<Note> Notes { get; set; }
        public string Number { get; set; }
        public double RingSize { get; set; }
        public int ChainLength { get; set; }
        public string Instructions { get; set; }
        public string AdditionalDescription { get; set; }
        public string RequestedShipDate { get; set; }
    }

    public class Line
    {
        public List<Item> Items { get; set; }
        public Serial Serial { get; set; }
        public int Quantity { get; set; }
        public double ExtendedPrice { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string BoxType { get; set; }
        public bool GiftWrap { get; set; }
        public string BoxDescription { get; set; }
        public string Message { get; set; }
        public string LineNumber { get; set; }
        public string Source { get; set; }
        public string CustomerLineReference { get; set; }
    }

    public class Note
    {
        public string NoteType { get; set; }
        public string Text { get; set; }
    }

    public class Payment
    {
        public string Type { get; set; }
    }

    public class Serial
    {
        public string Instructions { get; set; }
        public string Value { get; set; }
    }

    public class ShipToAddress
    {
        public Address Address { get; set; }
        public bool ShipComplete { get; set; }
        public string ShipMethodType { get; set; }
        public bool RemovePricing { get; set; }
        public bool SignatureRequired { get; set; }
    }

    public class Stone
    {
        public int Location { get; set; }
        public string Number { get; set; }
        public string SerialNumber { get; set; }
        public bool IsCustomerStone { get; set; }
        public double CustomerStoneValue { get; set; }
    }
}
