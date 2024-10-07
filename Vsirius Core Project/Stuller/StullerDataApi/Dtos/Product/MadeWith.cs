using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class MadeWithDto
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string UnitOfSale { get; set; }
    }

  


}
