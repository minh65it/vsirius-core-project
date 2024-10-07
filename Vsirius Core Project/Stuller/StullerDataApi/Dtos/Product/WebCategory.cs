using System;

namespace StullerDataApi.Dtos
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    [Serializable]
    public class WebCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string CategoryImageUrl { get; set; }
    }

  


}
