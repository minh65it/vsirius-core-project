using VsiriusDataModel.DataModel;

namespace OrdersAPI.Dtos
{
    public class OrderHistory
    {
        public VsiriusOrderDto Orders { get; set; }
        public VsiriusProduct Product { get; set; }
    }
}
