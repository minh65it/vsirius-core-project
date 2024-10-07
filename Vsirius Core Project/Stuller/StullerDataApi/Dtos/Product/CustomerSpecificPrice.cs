using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class CustomerSpecificPriceDto
    {
        public double Value { get; set; }
        public string CurrencyCode { get; set; }
    }

  


}
