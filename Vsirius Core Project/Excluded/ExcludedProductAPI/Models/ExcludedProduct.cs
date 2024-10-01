using System.ComponentModel.DataAnnotations;

namespace ExcludedProductAPI.Models
{
    public class ExcludedProduct
    {
        [Key]
        public string MarketplaceSKU { get; set; }
        public string Marketplace {  get; set; }
        public string SupplierSKU { get; set; }
        public ExclusionType ExclusionType { get; set; }
        public string ExclusionNote { get; set;  }
        public DateTime TimeStamp { get; set;  }
    }

    public enum ExclusionType
    {
        NoStock,
        Deleted
    }
}
