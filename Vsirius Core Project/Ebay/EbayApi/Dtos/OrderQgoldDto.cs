using System.Collections.Generic;

namespace EbayApi.Dtos
{
    public class OrderQgoldDto
    {
        public string OrderQgoldId { get; set; }
        public string OrderStatus { get; set; }
        public string OrderDate { get; set; }
        public string BuyerName { get; set; }
        public string Address1 { get; set; }
        public string ZipCode { get; set; }
        public string Tracking { get; set; }
        public string ShipVia { get; set; }
        public List<ItemQgold> Items { get; set; }
        public string Total { get; set; }
        public string Marketplace { get; set; }
        public string MarketOrderId { get; set; }
        public bool IsSendMailLostPackage { get; set; }
    }

    public class ItemQgold
    {
        public string Style { get; set; }
        public string ItemDesc { get; set; }
        public string Qty { get; set; }
        public string Uom { get; set; }
        public string Price { get; set; }
        public string ItemTotal { get; set; }
        public string Invoice { get; set; }
    }
}