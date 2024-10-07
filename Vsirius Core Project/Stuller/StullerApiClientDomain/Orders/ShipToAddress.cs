namespace StullerApiClient.Models.Orders
{
    public class ShipToAddress
    {
        public Address Address { get; set; }
        public bool ShipComplete { get; set; }
        public string ShipMethodType { get; set; }
        public bool RemovePricing { get; set; }
        public bool SignatureRequired { get; set; }
    }

  


}
