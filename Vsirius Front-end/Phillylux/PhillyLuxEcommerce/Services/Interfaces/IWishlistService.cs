using PhillyLuxEcommerce.Helpers.Utilities;

namespace PhillyLuxEcommerce.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<OperationResult> AddWishlist(string productId);
        Task<OperationResult> GetWishlistByUserId(int? pageNumber);
        Task<OperationResult> GetWishlistAllByUser();
        Task<OperationResult> DeleteWishlist(string idWishlist);
        Task<OperationResult> GetCountWishlistUser();
    }
}
