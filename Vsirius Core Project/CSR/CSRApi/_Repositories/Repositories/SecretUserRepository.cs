using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class SecretUserRepository : Repository<SecretIdentityUserInfo>, ISecretUserRepository
    {
        public SecretUserRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
