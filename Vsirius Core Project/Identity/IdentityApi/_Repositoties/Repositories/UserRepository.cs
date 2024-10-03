using IdentityApi._Repositories.Interfaces;
using IdentityApi.Data;
using IdentityApi.Models;
using IdentityApi.Models.User;

namespace IdentityApi._Repositories.Repositories
{
    public class UserRepository : Repository<VsiriusUser>, IUserRepository
    {
        public UserRepository(IdentityContext context) : base(context)
        {
        }
    }
}
