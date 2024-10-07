using System.Collections.Generic;

namespace StullerApiClient.Models.Orders
{
    public class Line
    {
        public List<Item> Items { get; set; }
        public Serial Serial { get; set; }
        public int Quantity { get; set; }
        public double ExtendedPrice { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public string BoxType { get; set; }
        public bool GiftWrap { get; set; }
        public string BoxDescription { get; set; }
        public string Message { get; set; }
        public string LineNumber { get; set; }
        public string Source { get; set; }
        public string CustomerLineReference { get; set; }
    }

  


}
