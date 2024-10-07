using System.ComponentModel.DataAnnotations;

namespace OrderProcessDomain.AmazonSeller
{
    public class QgoldFtpOrderObject
    {
        public string OrderId { get; set; }
        public string Format { get; set; } // "5"
        public string Customer { get; set; } // "58772"
        public string PO { get; set; }
        [Required]
        public string ShipToName { get; set; }
        [Required]
        public string ShipToAddress1 { get; set; }
        public string ShipToAddress2 { get; set; }
        public string ShipToAddress3 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToState_Province { get; set; }
        public string ShipToZip { get; set; }
        public string ShipToCountryCode { get; set; }
        public string ShipToPhone { get; set; }
        [Required]
        public List<QgoldFtpOrderItem> Item { get; set; }
        public string VSiriusItem { get; set; }
        public string Description { get; set; }
        public string Ship_Method_Code { get; set; }
        public string Ship_Option_Code { get; set; }
        public string Gift_Message { get; set; }
        public string Cust_Order_Number { get; set; }
        [Required]
        public string Amz_Order_ID { get; set; }
        public string Amz_Item_ID { get; set; }
        public DateTime TimeStamp { get; set; } //DateTime.Now
        public string Status { get; set; }
        public string Marketplace { get; set; }
    }
    public class QgoldFtpOrderItem
    {
        public string Item { get; set; }
        public string Qty { get; set; }
        public string Size { get; set; }
        public string Price { get; set; }
    }
}
