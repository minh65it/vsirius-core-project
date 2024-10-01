using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class NotificationDetailRepository : Repository<NotificationDetail>, INotificationDetailRepository
    {
        public NotificationDetailRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
