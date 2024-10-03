using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class PageViewRepository : Repository<PageView>, IPageViewRepository
    {
        public PageViewRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
