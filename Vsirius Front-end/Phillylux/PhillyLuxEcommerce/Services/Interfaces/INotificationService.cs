using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto> GetTopNotification();
        Task<OperationResult> UpdateNotificationByUser(string email, string notificationId);
        Task<PaginationUtility<NotificationDto>> GetAllNotification(string email, int PageNumber, int PageSize = 10);
        Task<List<NotificationDto>> GetNotificationNotRead(string email);
    }
}
