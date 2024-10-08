using System.ComponentModel.DataAnnotations.Schema;

namespace EtsyIntegrationDataAPI.Models
{
    public class TagProduct
    {
        public string item_sku { get; set; }
        public string tag { get; set; }
    }
}
