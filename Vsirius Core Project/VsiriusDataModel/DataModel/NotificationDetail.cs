

namespace VsiriusDataModel.DataModel
{
    interface INotificationDetail
    {
        Guid Id { get; set; }
        string NotificationId { get; set; }
        string EmployeeId { get; set; }

        string UserId { get; set; }

        bool isView { get; set; }
    }

    public class NotificationDetail : INotificationDetail
    {
        public Guid Id { get; set; }
        public string NotificationId { get; set; }
        public string EmployeeId { get; set; }
        public string UserId { get; set; }
        public bool isView { get; set; }
    }
}
