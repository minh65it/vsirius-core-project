using System;

namespace StullerDataApi.Dtos
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    [Serializable]
    public class AdvancedProductFilterValueDto
    {
        public string DisplayValue { get; set; }
        public string Value { get; set; }
    }

  


}
