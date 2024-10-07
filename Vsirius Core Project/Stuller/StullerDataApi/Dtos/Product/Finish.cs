using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class FinishDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string WhiteSampleImage { get; set; }
        public string YellowSampleImage { get; set; }
    }

  


}
