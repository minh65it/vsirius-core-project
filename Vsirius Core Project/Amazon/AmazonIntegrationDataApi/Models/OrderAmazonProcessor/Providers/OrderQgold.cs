using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers
{
    public class OrderQgold
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Order { get; set; }
        public string PO { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public string Date { get; set; }
        public string Items { get; set; }
        public string Total { get; set; }
        public List<InvoiceDetail> InvoiceDetails { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public class InvoiceDetail
    {
        public string ShipToText { get; set; }
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ZipCode { get; set; }
        public string SoldToText { get; set; }
        public string SoldToAddress1 { get; set; }
        public string SoldToAddress2 { get; set; }
        public string Date { get; set; }
        public string DocumentType { get; set; }
        public string PlacedBy { get; set; }
        public string PlacedVia { get; set; }
        public string ShipVia { get; set; }
        public string PackagingOption { get; set; }
        public string Tracking { get; set; }
        public string DueDate { get; set; }
        public string Term { get; set; }
        public List<QgoldInvoiceItem> QgoldInvoiceItems { get; set; }
    }

    public class QgoldInvoiceItem
    {
        public string ItemDesc { get; set; }
        public string Style { get; set; }
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Price { get; set; }
        public string ItemTotal { get; set; }
        public string Invoice { get; set; }
    }
}
