using CSRApi._Repositories.Interfaces;
using VsiriusDataModel.DataContext;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class FavouritesRepository : Repository<Favourites>, IFavouritesRepository
    {
        public FavouritesRepository(VsiriusContext context) : base(context)
        {
        }
    }
}
