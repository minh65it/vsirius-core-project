namespace StullerOrderProcessAPI.Dtos
{
    public class MapOrderParam
    {
        public string OrderId { get; set; } = string.Empty;
        public string ShipToName { get; set; } = string.Empty;
        public string ShipToAddress1 { get; set; } = string.Empty;
        public string ShipToZip { get; set; } = string.Empty;
        public List<string>? ItemSkus { get; set; }

    }
}
