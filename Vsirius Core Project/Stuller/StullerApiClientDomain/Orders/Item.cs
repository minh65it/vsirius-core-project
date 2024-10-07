using System.Collections.Generic;

namespace StullerApiClient.Models.Orders
{
    public class Item
    {
        public Clasp Clasp { get; set; }
        public List<Stone> Stones { get; set; }
        public List<Engraving> Engravings { get; set; }
        public EarringBack EarringBack { get; set; }
        public Chain Chain { get; set; }
        public List<Note> Notes { get; set; }
        public string Number { get; set; }
        public double RingSize { get; set; }
        public int ChainLength { get; set; }
        public string Instructions { get; set; }
        public string AdditionalDescription { get; set; }
        public string RequestedShipDate { get; set; }
    }

  


}
