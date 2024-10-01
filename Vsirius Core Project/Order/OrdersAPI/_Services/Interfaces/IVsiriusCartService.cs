using OrdersAPI.Dtos;
using OrdersAPI.Helpers.Utilities;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Services.Interfaces
{
    public interface IVsiriusCartService
    {
        Task<PaginationUtility<VsiriusListCartDto>> ListCartAll(PaginationParam pagination, string? fullTextSearch, bool isPaging = true);
        Task<List<VsiriusListCartDetailsDto>> GetCartDetailByCartId(Guid cartId, bool isDelete);
        Task<VsiriusListCartDto> ListCartByUser(Guid userId, string? email);
        Task<OperationResult> AddCartOrDelete(VsiriusCartDetailsDto dto, HttpHeaderDto httpHeader);
        Task<bool> UpdateTotalPriceCart(VsiriusCartDetails cartDetail);
    }
}
