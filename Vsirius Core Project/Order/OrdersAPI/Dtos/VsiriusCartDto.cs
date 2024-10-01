
namespace OrdersAPI.Dtos
{
    public class VsiriusCartDto
    {
        public Guid? CartId { get; set; }
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public decimal? TotalPrice { get; set; }
        public decimal? Tax { get; set; }
    }
}
