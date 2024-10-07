using System;
using System.Collections.Generic;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class FillOptionDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<ColorDto> Colors { get; set; }
    }

  


}
