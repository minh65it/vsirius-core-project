using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class MonogramChainOptionDto
    {
        public int ChainItemId { get; set; }
        public int ChainLength { get; set; }
        public string ChainLengthDescription { get; set; }
    }

  


}
