using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class PublicationDto
    {
        public string Name { get; set; }
        public int PageNumber { get; set; }
        public string Edition { get; set; }
    }

  


}
