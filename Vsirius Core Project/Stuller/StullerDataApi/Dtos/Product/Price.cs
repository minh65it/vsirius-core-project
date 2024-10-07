using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class PriceDto
    {
        public double Value { get; set; }
        public string CurrencyCode { get; set; }
    }

  


}
