using System.Collections.Generic;

namespace EbayCoreApi.Dtos
{
    public class TrackingDto
    {
        public string OrderId { get; set; }
        public string TrackingNo { get; set; }
        public string ShipVia {  get; set; }
        public IEnumerable<TrackingDetailDto> DetailItems { get; set; }
    }
    public class TrackingDetailDto
    {
        public string LineItemId { get; set; }
        public int Quantity { get; set; }
        public string ItemID { get; set; }
        public string TransactionID { get; set; }
    }
}