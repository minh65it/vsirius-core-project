namespace OrdersAPI.Dtos
{
    public class UpdateStatusOrderDto
    {
        public string OrderId {  get; set; }
        public int OrderStatus {  get; set; }
        public string Note { get; set; }
    }
}
