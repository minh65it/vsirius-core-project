using System;

namespace StullerApiClient.Models
{
    [Serializable]
    public class Pattern
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SampleImage { get; set; }
    }

  


}
