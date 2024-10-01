using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;

namespace CSRApi._Services.Interfaces
{
    public interface IFavouritesService
    {
        Task<OperationResult> Create(FavouritesDto dto);
        Task<OperationResult> Delete(string id);

        Task<PaginationUtility<FavouritesDto>> GetData(PaginationParam pagination, string userId);
        Task<List<FavouritesDto>> GetAll(string userId);
    }
}
