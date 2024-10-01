using CSRApi._Repositories;
using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class VsiriusProductRepository : Repository<VsiriusProduct>, IVsiriusProductRepository
    {
        public VsiriusProductRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
