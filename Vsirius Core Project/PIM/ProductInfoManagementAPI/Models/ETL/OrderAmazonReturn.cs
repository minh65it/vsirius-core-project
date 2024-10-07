using MongoDB.Bson.Serialization.Attributes;

namespace PimApi.Models.ETL
{
    public class OrderAmazonReturn
    {
        [BsonId]
        public MongoDB.Bson.ObjectId _id { get; set; }
        public string OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ReturnRequestDate { get; set; }
        public string ReturnRequestStatus { get; set; }
        public string AmazonRMAID { get; set; }
        public string MerchantRMAID { get; set; }
        public string LabelType { get; set; }
        public string LabelCost { get; set; }
        public string CurrencyCode { get; set; }
        public string ReturnCarrier { get; set; }
        public string TrackingID { get; set; }
        public string LabelToBePaidBy { get; set; }
        public bool AToZClaim { get; set; }
        public bool IsPrime { get; set; }
        public string ASIN { get; set; }
        public string MerchantSKU { get; set; }
        public string ItemName { get; set; }
        public int ReturnQuantity { get; set; }
        public string ReturnReason { get; set; }
        public bool InPolicy { get; set; }
        public string ReturnType { get; set; }
        public string Resolution { get; set; }
        public string InvoiceNumber { get; set; }
        public object ReturnDeliveryDate { get; set; }
        public string OrderAmount { get; set; }
        public int OrderQuantity { get; set; }
        public string SafeTActionReason { get; set; }
        public string SafeTClaimId { get; set; }
        public string SafeTClaimState { get; set; }
        public string SafeTClaimCreationTime { get; set; }
        public string SafeTClaimReimbursementAmount { get; set; }
        public object RefundedAmount { get; set; }
        public object Category { get; set; }
        public object refNumber { get; set; }
    }
}
