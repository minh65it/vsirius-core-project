using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class UserRepository : Repository<VsiriusUser>, IUserRepository
    {
        public UserRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
