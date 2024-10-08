using System.ComponentModel.DataAnnotations;

namespace EtsyIntegrationDataAPI.Models
{
    public class PropertiesProduct
    {
        [Key]
        public long Id { get; set; }
        public string item_sku { get; set; }
        public long taxonomy_id { get; set; }
        public long property_id { get; set; }
        public long value_id { get; set; }
        public string name { get; set; }
        public int? scale_id { get; set; }
    }
}
