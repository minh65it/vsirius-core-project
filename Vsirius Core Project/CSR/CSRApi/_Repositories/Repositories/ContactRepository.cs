using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class ContactRepository : Repository<Contacts>, IContactRepository
    {
        public ContactRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
