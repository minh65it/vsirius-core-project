using System;

namespace StullerApiClient.Models
{
    [Serializable]
    public class MonogramOption
    {
        public int LocationNumber { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int MaxCharacters { get; set; }
    }

  


}
