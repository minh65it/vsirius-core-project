
namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusCart
    {
        Guid CartId { get; set; }
        Guid? UserId { get; set; }
        string? UserName { get; set; }
        string? Email { get; set; }
        decimal? TotalPrice { get; set; }
        bool? Status { get; set; }
        decimal? Tax { get; set; }
        IEnumerable<VsiriusCartDetails>? VsiriusCartDetails { get; set; }
    }

    public class VsiriusCart : IVsiriusCart
    {
        public Guid CartId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public decimal? TotalPrice { get; set; }
        public virtual IEnumerable<VsiriusCartDetails>? VsiriusCartDetails { get; set; }
        public bool? Status { get; set; }
        public string Email { get; set; }
        public decimal? Tax { get; set; }
    }
}
