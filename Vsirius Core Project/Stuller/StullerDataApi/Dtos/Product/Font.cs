using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class FontDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int MaxCharacters { get; set; }
        public string SampleImage { get; set; }
        public string ValidationPattern { get; set; }
        public string ValidationMessage { get; set; }
    }

  


}
