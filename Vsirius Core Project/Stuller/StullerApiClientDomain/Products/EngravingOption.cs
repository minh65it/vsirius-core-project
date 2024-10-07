using System;
using System.Collections.Generic;

namespace StullerApiClient.Models
{
    [Serializable]
    public class EngravingOption
    {
        public int LocationNumber { get; set; }
        public int Lines { get; set; }
        public string Description { get; set; }
        public List<Type> Types { get; set; }
    }

  


}
