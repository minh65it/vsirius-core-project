using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class NotificationRepository : Repository<Notifications>, INotificationRepository
    {
        public NotificationRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
