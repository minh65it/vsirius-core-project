namespace EtsyIntegrationDataAPI.Dtos.OrderEtsy
{
    public class ReceiptShipment
    {
        public long receipt_id { get; set; }
        public string tracking_code { get; set; }
        public string carrier_name { get; set; }
        public bool send_bcc { get; set; }
        public string note_to_buyer { get; set; }
    }
}
