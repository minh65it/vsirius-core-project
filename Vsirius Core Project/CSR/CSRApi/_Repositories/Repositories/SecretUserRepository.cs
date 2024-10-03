using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class SecretUserRepository : Repository<SecretIdentityUserInfo>, ISecretUserRepository
    {
        public SecretUserRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
