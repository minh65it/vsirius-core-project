using System;
using System.Collections.Generic;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class TypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public List<FontDto> Fonts { get; set; }
        public List<FinishDto> Finishes { get; set; }
        public List<PatternDto> Patterns { get; set; }
        public List<FillOptionDto> FillOptions { get; set; }
        public LaborDto Labor { get; set; }
    }

  


}
