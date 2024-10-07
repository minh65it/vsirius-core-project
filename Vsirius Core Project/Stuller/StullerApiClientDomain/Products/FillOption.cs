using System;
using System.Collections.Generic;

namespace StullerApiClient.Models
{
    [Serializable]
    public class FillOption
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<Color> Colors { get; set; }
    }

  


}
