

namespace VsiriusDataModel.DataModel
{
    public interface IVsiriusOrders
    {
        string OrderId { get; set; }
        Guid CartId { get; set; }
        string UserId { get; set; }
        string? FullName { get; set; }
        string? FirstName { get; set; }
        string? LastName { get; set; }
        string? Email { get; set; }
        string? Phone { get; set; }
        DateTime? BirthDay { get; set; }
        bool? Gender { get; set; }
        string? Address1 { get; set; }
        string? Address2 { get; set; }
        string? City { get; set; }
        string? State { get; set; }
        string? PostCode { get; set; }
        string? Country { get; set; }
        bool? Shipping { get; set; }
        int? PaymentMethod { get; set; }
        string? Note { get; set; }
        int? StatusId { get; set; }
        DateTime? CreateOrderDate { get; set; }
        int? TotalQuantityItem { get; set; }
        decimal? TotalPrice { get; set; }
        decimal? Tax { get; set; }
        string? TransactionId {  get; set; }
        bool isRead { get; set; }

    }

    public class VsiriusOrders : IVsiriusOrders
    {
        public string OrderId { get; set; }
        public Guid CartId { get; set; }
        public string UserId { get; set; }
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public DateTime? BirthDay { get; set; }
        public bool? Gender { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
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
        public decimal? Tax { get; set; }
        public bool isRead { get; set; }
        public string? TransactionId { get; set; }
    }
}
