using VsiriusDataModel.DataModel;

namespace OrdersAPI.Dtos
{
    public class VsiriusOrderDto 
    {
        public string OrderId { get; set; }
        public Guid CartId { get; set; }
        public string UserId { get; set; }
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool? Gender { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string Phone { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public bool? Shipping { get; set; }
        public int? PaymentMethod { get; set; }
        public string? Note { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreateOrderDate { get; set; }
        public int? TotalQuantityItem { get; set; }
        public decimal? TotalPrice { get; set; }
        public bool isRead { get; set; }
        public string StatusName { get; set; }
        public string ProductId { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public List<VsiriusProduct> ListProducts { get; set; }
        public string CardCode { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string? TransactionId { get; set; }   
    }
}
