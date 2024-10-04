using OrdersAPI._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Repositories.Repositories
{
    public class ConfigJsonRepository : Repository<ConfigJson>, IConfigJsonRepository
    {
        public ConfigJsonRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
