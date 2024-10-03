using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class DrivenHtmlPagesRepository : Repository<DrivenHtmlPages>, IDrivenHtmlPagesRepository
    {
        public DrivenHtmlPagesRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
