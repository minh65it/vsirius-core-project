using System.ComponentModel.DataAnnotations.Schema;

namespace EtsyIntegrationDataAPI.Models
{
    public class MaterialProduct
    {
        public string item_sku { get; set; }
        public string material { get; set; }
    }
}
