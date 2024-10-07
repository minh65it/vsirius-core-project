namespace WalmartIntegrationDataApi.Dtos.Qgold
{
    public class OrderQgoldDto
    {
        public string? orderQgoldId { get; set; }
        public string? orderStatus { get; set; }
        public string? orderDate { get; set; }
        public string? buyerName { get; set; }
        public string? address1 { get; set; }
        public string? zipCode { get; set; }
        public string? tracking { get; set; }
        public string? shipVia { get; set; }
        public List<Item>? items { get; set; }
        public string? total { get; set; }
        public string? marketplace { get; set; }
        public string? marketOrderId { get; set; }
        public bool isSendMailLostPackage { get; set; }
    }

    public class Item
    {
        public string? style { get; set; }
        public string? itemDesc { get; set; }
        public string? qty { get; set; }
        public string? uom { get; set; }
        public string? price { get; set; }
        public string? itemTotal { get; set; }
        public string? invoice { get; set; }
    }

}
