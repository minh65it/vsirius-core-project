using System;

namespace StullerApiClient.Models.Orders
{
    public class CustomerData
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public EmailConfirmation EmailConfirmation { get; set; }
        public ExtraAmount ExtraAmount { get; set; }
        public double FreightAmount { get; set; }
        public double TaxAmount { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public AdditionalData AdditionalData { get; set; }
    }

  


}
