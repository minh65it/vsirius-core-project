namespace OrdersAPI.Dtos
{
    public class VsiriusCartDetailsDto
    {
        //public Guid? CartId { get; set; }
        /// <summary>
        /// Required : Mã sản phẩm
        /// </summary>
        public string? ProductId { get; set; }

        /// <summary>
        /// Required : Số lượng sản phẩm
        /// </summary>
        public int? Quantity { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? InfoBrowser { get; set; }

        /// <summary>
        /// Optional : Nhập User
        /// </summary>
        public Guid? UserId { get; set; }
        public string? UserName { get; set; }

        /// <summary>
        /// Required : Nhập Email
        /// </summary>
        public string? Email { get; set; }
    }

}
