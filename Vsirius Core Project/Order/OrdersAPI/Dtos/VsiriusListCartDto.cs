using VsiriusDataModel.DataModel;

namespace OrdersAPI.Dtos
{
    public class VsiriusListCartDto
    {
        public Guid? CartId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Tax { get; set; }
        public int QuantityItem { get; set; }
        public bool? Status { get; set; }
        public List<VsiriusListCartDetailsDto> ListCartDetailsDtos { get; set; }
    }
}
