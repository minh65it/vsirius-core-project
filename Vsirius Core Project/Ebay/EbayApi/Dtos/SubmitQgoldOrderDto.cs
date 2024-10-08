using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayApi.Dtos
{
    public class SubmitQgoldOrderDto
    {
        public string orderId { get; set; } = string.Empty;
        public string format { get; set; } = string.Empty;
        public string customer { get; set; } = string.Empty;
        public string po { get; set; } = string.Empty;
        public string shipToName { get; set; } = string.Empty;
        public string shipToAddress1 { get; set; } = string.Empty;
        public string shipToAddress2 { get; set; } = string.Empty;
        public string shipToAddress3 { get; set; } = string.Empty;
        public string shipToCity { get; set; } = string.Empty;
        public string shipToState_Province { get; set; } = string.Empty;
        public string shipToZip { get; set; } = string.Empty;
        public string shipToCountryCode { get; set; } = string.Empty;
        public string shipToPhone { get; set; } = string.Empty;
        public List<ItemQgoldParam> item { get; set; }
        public string vSiriusItem { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string ship_Method_Code { get; set; } = string.Empty;
        public string ship_Option_Code { get; set; } = string.Empty;
        public string gift_Message { get; set; } = string.Empty;
        public string cust_Order_Number { get; set; } = string.Empty;
        public string amz_Order_ID { get; set; } = string.Empty;
        public string amz_Item_ID { get; set; } = string.Empty;
        public DateTime timeStamp { get; set; }
        public string status { get; set; } = string.Empty;
        public string marketplace { get; set; } = string.Empty;
    }
    public class ItemQgoldParam
    {
        public string item { get; set; } = string.Empty;
        public string qty { get; set; } = string.Empty;
        public string size { get; set; } = string.Empty;
        public string price { get; set; } = string.Empty;
    }
}