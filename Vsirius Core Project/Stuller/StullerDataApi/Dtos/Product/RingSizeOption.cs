using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class RingSizeOptionDto
    {
        public double Size { get; set; }
        public bool IsStockedSize { get; set; }
        public RingSizePriceDto Price { get; set; }
    }

  


}
