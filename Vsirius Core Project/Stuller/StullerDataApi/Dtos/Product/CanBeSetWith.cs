using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class CanBeSetWithDto
    {
        public int Quantity { get; set; }
        public string Shape { get; set; }
        public string Size { get; set; }
        public string SettingType { get; set; }
    }

  


}
