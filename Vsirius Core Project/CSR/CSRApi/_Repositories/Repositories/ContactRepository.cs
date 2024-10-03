using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class ContactRepository : Repository<Contacts>, IContactRepository
    {
        public ContactRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
