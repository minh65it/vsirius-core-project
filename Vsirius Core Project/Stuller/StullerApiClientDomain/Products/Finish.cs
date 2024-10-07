using System;

namespace StullerApiClient.Models
{
    [Serializable]
    public class Finish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string WhiteSampleImage { get; set; }
        public string YellowSampleImage { get; set; }
    }

  


}
