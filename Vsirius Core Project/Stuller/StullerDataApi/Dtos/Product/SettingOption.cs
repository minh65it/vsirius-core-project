using System;

namespace StullerDataApi.Dtos
{
    [Serializable]
    public class SettingOptionDto
    {
        public int LocationNumber { get; set; }
        public int StoneCount { get; set; }
        public string SettingType { get; set; }
        public string Description { get; set; }
        public double SizeMM { get; set; }
        public double Dimension1 { get; set; }
        public double Dimension2 { get; set; }
        public double Dimension3 { get; set; }
        public string GroupName { get; set; }
        public string Shape { get; set; }
        public int ShapeCode { get; set; }
    }

  


}
