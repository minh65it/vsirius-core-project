using ProductApi._Repositories.Interfaces;
using VsiriusDataModel.DataModel;
using VsiriusDataModel.DataContext;

namespace ProductApi._Repositories.Repositories
{
    public class VsiriusHomeProductRepository : Repository<VsiriusHomeProduct>, IVsiriusHomeProductRepository
    {
        public VsiriusHomeProductRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
