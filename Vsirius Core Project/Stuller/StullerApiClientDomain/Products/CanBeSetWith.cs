using System;

namespace StullerApiClient.Models
{
    [Serializable]
    public class CanBeSetWith
    {
        public int Quantity { get; set; }
        public string Shape { get; set; }
        public string Size { get; set; }
        public string SettingType { get; set; }
    }

  


}
