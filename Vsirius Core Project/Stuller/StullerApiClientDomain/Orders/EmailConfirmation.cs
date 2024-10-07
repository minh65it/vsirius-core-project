namespace StullerApiClient.Models.Orders
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class EmailConfirmation
    {
        public string FromAddress { get; set; }
        public bool SendOrderConfirmation { get; set; }
        public bool SendShipmentConfirmation { get; set; }
        public string ToAddress { get; set; }
    }

  


}
