using VsiriusDataModel.DataModel;

namespace OrdersAPI.Dtos
{
    public class OrderStatusDto : OrderStatus
    {
    }
    public enum OrderStatusSeedData
    {
        Pending,
        Approved,
        Shipping,
        OrderDelivered,
        Cancel
    }
}
