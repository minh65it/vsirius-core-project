using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class DrivenHtmlPagesRepository : Repository<DrivenHtmlPages>, IDrivenHtmlPagesRepository
    {
        public DrivenHtmlPagesRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
