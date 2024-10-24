
namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusCartDetails
    {
        Guid CartDetailId { get; set; }
        Guid CartId { get; set; }
        string? ProductId { get; set; }
        int? Quantity { get; set; }
        decimal? Price { get; set; }
        decimal? PriceSale { get; set; }
        DateTime? CreateDate { get; set; }
        string? InfoBrowser { get; set; }
        bool? IsDelete { get; set; }
        VsiriusCart? VsiriusCart { get; set; }
    }

    public class VsiriusCartDetails : IVsiriusCartDetails
    {
        public Guid CartDetailId { get; set; }
        public Guid CartId { get; set; }
        public string? ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public decimal? PriceSale { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? InfoBrowser { get; set; }
        public virtual VsiriusCart? VsiriusCart { get; set; }
        public bool? IsDelete { get; set; }
    }
}
