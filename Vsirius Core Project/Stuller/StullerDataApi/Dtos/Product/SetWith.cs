using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class SetWithDto
    {
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }

  


}
