using System;

namespace StullerApiClient.Models
{
    [Serializable]
    public class FullySetImage
    {
        public string FullUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ZoomUrl { get; set; }
        public int SortOrder { get; set; }
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }
        public string Angle { get; set; }
    }

  


}
