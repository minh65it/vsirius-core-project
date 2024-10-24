
namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusOrderDetail
    {
        Guid OrderDetailId { get; set; }
        string OrderId { get; set; }
        string? ProductId { get; set; }
        int? QuantityItem { get; set; }
        decimal? Price { get; set; }
    }

    public class VsiriusOrderDetails : IVsiriusOrderDetail
    {
        public Guid OrderDetailId { get; set; }
        public string  OrderId { get; set; }
        public string? ProductId { get; set; }
        public int? QuantityItem { get; set; }
        public decimal? Price { get; set; }
    }
}
