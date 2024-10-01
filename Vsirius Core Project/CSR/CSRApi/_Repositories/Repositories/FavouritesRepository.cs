using CSRApi._Repositories.Interfaces;
using CSRApi.Data;
using VsiriusDataModel.DataModel;

namespace CSRApi._Repositories.Repositories
{
    public class FavouritesRepository : Repository<Favourites>, IFavouritesRepository
    {
        public FavouritesRepository(CSRDbContext context) : base(context)
        {
        }
    }
}
