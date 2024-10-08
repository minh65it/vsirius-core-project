namespace EtsyIntegrationDataAPI.Parameters.OrderParmas
{
    public class GetShopReceiptsParams
    {
        public int? min_created { get; set; }
        public int? max_created { get; set; }
        public int? min_last_modified { get; set; }
        public int? max_last_modified { get; set; }
        public int limit { get; set; } = 100;
        public int offset { get; set; } = 0;
        public string? sort_on { get; set; } = "created";
        public string? sort_order { get; set; } = "desc";
        public bool? was_paid { get; set; }
        public bool? was_shipped { get; set; }
        public bool? was_delivered { get; set; }
        public bool? was_canceled { get; set; }
    }
}
