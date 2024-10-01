using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;

namespace CSRApi._Services.Interfaces
{
    public interface INotificationService
    {
        Task<OperationResult> Create(NotificationDto dto);
        Task<OperationResult> UpdateView(string email,string notificationId);
        Task<OperationResult> Delete(string id);
        Task<PaginationUtility<NotificationDto>> GetData(PaginationParam pagination, string keyword);

        Task<PaginationUtility<NotificationDto>> GetNotificationByUser(PaginationParam pagination, string email);
        Task<List<NotificationDto>> GetNotificationNotRead(string email);
        Task<NotificationDto> GetNotificationMain();
    }
}
