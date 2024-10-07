using System;
using System.Collections.Generic;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class EngravingOptionDto
    {
        public int LocationNumber { get; set; }
        public int Lines { get; set; }
        public string Description { get; set; }
        public List<TypeDto> Types { get; set; }
    }

  


}
