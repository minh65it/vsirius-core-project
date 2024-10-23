

namespace VsiriusDataModel.DataModel
{
    public interface IOrderStatus
    {
        string IdGuild { get; set; }
        int Id { get; set; }
        string Name { get; set; }

        string Description { get; set; }
        bool IsActive { get; set; }
    }

    public class OrderStatus : IOrderStatus
    {
        public string IdGuild { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
