using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class NotificationMainRepository : Repository<NotificationMain>, INotificationMainRepository
    {
        public NotificationMainRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
