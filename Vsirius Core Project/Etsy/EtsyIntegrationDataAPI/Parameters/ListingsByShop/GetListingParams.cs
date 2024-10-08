namespace EtsyIntegrationDataAPI.Parameters.ListingsByShop
{
    public class GetListingParams
    {
        public string state { get; set; } = "active";
        public int limit { get; set; } = 100;
        public int offset { get; set; } = 0;
        public string sort_on { get; set; } = "created";
        public string sort_order { get; set; } = "desc";
        public string[]? includes { get; set; }
    }
}
