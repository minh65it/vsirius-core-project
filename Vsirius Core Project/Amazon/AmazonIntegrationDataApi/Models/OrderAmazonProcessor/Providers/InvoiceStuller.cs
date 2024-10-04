using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers
{
    public class Date
    {
        //[JsonProperty("$numberLong")]
        public string numberLong { get; set; }
    }

    public class Id
    {
        //[JsonProperty("$oid")]
        public string oid { get; set; }
    }

    public class InvoiceDate
    {
        //[JsonProperty("$date")]
        public DateTime date { get; set; }
    }

    public class InvoiceDetailStuller
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

    public class OrderDate
    {
        //[JsonProperty("$date")]
        public Date date { get; set; }
    }

    public class InvoiceStuller
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
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
        public List<InvoiceDetailStuller> InvoiceDetails { get; set; }
    }
}
