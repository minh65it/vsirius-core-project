using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI._Repositories.Repositories
{
    public class AuthenEtsyRepository : Repository<AuthenEtsy>, IAuthenEtsyRepository
    {
        public AuthenEtsyRepository(DataContext context) : base(context)
        {
        }
    }
}
