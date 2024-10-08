using System.ComponentModel.DataAnnotations.Schema;

namespace EtsyIntegrationDataAPI.Models
{
    public class StyleProduct
    {
        public string item_sku { get; set; }
        public string style { get; set; }
    }
}
