using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class NotificationMainRepository : Repository<NotificationMain>, INotificationMainRepository
    {
        public NotificationMainRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
