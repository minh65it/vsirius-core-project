using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class NotificationRepository : Repository<Notifications>, INotificationRepository
    {
        public NotificationRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
