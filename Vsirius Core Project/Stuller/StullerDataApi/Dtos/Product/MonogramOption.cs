using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class MonogramOptionDto
    {
        public int LocationNumber { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int MaxCharacters { get; set; }
    }

  


}
