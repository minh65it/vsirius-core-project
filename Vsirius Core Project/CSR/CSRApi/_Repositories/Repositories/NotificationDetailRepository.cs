using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class NotificationDetailRepository : Repository<NotificationDetail>, INotificationDetailRepository
    {
        public NotificationDetailRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
