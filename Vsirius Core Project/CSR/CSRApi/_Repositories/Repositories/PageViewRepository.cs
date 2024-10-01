using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class PageViewRepository : Repository<PageView>, IPageViewRepository
    {
        public PageViewRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
