using System;
using System.Collections.Generic;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class AdvancedProductFilterDto
    {
        public string Type { get; set; }
        public List<AdvancedProductFilterValueDto> Values { get; set; }
    }

  


}
