
namespace VsiriusDataModel.DataModel
{
    interface INotificationMain
    {
        Guid Id { get; set; }
        string NotificationId { get; set; }

        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }

        bool IsShow { get; set; }
    }

    public class NotificationMain : INotificationMain
    {
        public Guid Id { get; set; }
        public string NotificationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsShow { get; set; }
    }
}
