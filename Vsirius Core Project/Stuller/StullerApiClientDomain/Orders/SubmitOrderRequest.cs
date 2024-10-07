
namespace StullerApiClient.Models.Orders
{

    public class SubmitOrderRequest
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

  


}
