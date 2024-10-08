namespace EtsyIntegrationDataAPI.Models
{
    public class SellerTaxonomy
    {
        public long id { get; set; }
        public int level { get; set; }
        public string name { get; set; }
        public long? parent_id { get; set; }
    }
}
