using System;

namespace StullerApiClient.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    [Serializable]
    public class AdvancedProductFilterValue
    {
        public string DisplayValue { get; set; }
        public string Value { get; set; }
    }

  


}
