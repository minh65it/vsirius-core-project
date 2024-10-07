using System.Collections.Generic;

namespace StullerApiClient.Models.Orders
{
    public class Engraving
    {
        public List<EngravingLine> EngravingLine { get; set; }
        public int Location { get; set; }
        public string EngravingType { get; set; }
        public string FontType { get; set; }
        public string FontSize { get; set; }
        public string FillOption { get; set; }
        public string FillColor { get; set; }
    }

  


}
