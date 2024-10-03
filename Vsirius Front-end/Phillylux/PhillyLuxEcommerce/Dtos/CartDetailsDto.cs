namespace PhillyLuxEcommerce.Dtos;

public class CartDto
{
    public Guid? CartId { get; set; }
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public decimal? TotalPrice { get; set; }
    public decimal? Tax { get; set; }
    public int QuantityItem { get; set; }
    public bool? Status { get; set; }
    public List<CartDetailDto> ListCartDetailsDtos { get; set; }
}

public class CartDetailDto
{
    public Guid? CartDetailsId { get; set; }
    public string? ProductId { get; set; }
    public int? Quantity { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? InfoBrowser { get; set; }
    public string ProductName { get; set; }
    public string? Image { get; set; }
    public decimal? Price { get; set; }
    public decimal? PriceSale { get; set; }
    public string? Size { get; set; }
    public string? Color { get; set; }
}

