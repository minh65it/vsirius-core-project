using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EbayCoreApi.Models
{
    public class OrderDetail
    {
        [Key] public string OrderDetailId { get; set; }
        public string OrderId { get; set; }
        [ForeignKey("OrderId")] public virtual Order Order { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string TransactionID { get; set; }
        public string ItemID { get; set; }
    }
}