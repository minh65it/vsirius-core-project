using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class PatternDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SampleImage { get; set; }
    }

  


}
