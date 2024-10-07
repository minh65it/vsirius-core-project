using System;
using System.Collections.Generic;

namespace StullerApiClient.Models
{
    [Serializable]
    public class AdvancedProductFilter
    {
        public string Type { get; set; }
        public List<AdvancedProductFilterValue> Values { get; set; }
    }

  


}
