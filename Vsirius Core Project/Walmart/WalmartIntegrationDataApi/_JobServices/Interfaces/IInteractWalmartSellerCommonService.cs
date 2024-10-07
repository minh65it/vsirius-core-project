using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.Dtos.SellerItem;

namespace WalmartIntegrationDataApi._JobServices.Interfaces
{
    public interface IInteractWalmartSellerCommonService
    {
        Task<TokenResult> GetTokenFromWalmartSeller();
        Task<List<Item>> GetItemsFromSeller(bool isActive, bool isPublish = false);
    }
}
