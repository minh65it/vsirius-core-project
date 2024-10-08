using System.Collections.Generic;

namespace EbayCoreApi.Dtos
{
    public class OrderStullerDto
    {
        public string orderID { get; set; }
        public string confirmation { get; set; }
        public string po { get; set; }
        public string orderDate { get; set; }
        public string status { get; set; }
        public string account { get; set; }
        public string estTotalPrice { get; set; }
        public string urlDetail { get; set; }
        public string invoice { get; set; }
        public string shipDate { get; set; }
        public string deliveryDate { get; set; }
        public string carrier { get; set; }
        public string tracking { get; set; }
        public string shippingUpdates { get; set; }
        public string invoiceAmount { get; set; }
        public List<Item> items { get; set; }
        public string shipping { get; set; }
        public string billing { get; set; }
        public string zipCode { get; set; }
        public string address1 { get; set; }
        public string buyerName { get; set; }
        public string shippingMethod { get; set; }
        public string marketplace { get; set; }
        public string marketplaceOrderId { get; set; }
    }
    public class Item
    {
        public string productSku { get; set; }
        public string ordered { get; set; }
        public string shipped { get; set; }
        public string estShipDate { get; set; }
        public string unitPrice { get; set; }
        public string estPrice { get; set; }
    }

}