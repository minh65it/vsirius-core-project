using ProductApi._Repositories.Interfaces;
using VsiriusDataModel.DataModel;
using VsiriusDataModel.DataContext;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusFilterRepository : Repository<VsiriusFilter>, IVsiriusFilterRepository
    {
        public VsiriusFilterRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
