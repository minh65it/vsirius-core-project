using System;

namespace StullerApiClient.Models
{
    [Serializable]
    public class RingSizeOption
    {
        public double Size { get; set; }
        public bool IsStockedSize { get; set; }
        public RingSizePrice Price { get; set; }
    }

  


}
