
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.Utilities;

namespace AmazonIntegrationDataApi._Services.Interfaces
{
    public interface IAmazonJewelryDataFeedItemService
    {
        Task<PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>> GetData(PaginationParam pagination, string keyword,bool isPagging);
        Task<PaginationUtility<AmazonJewelryDataToAmazon_Dto>> GetDataForAmazon(PaginationParam pagination, string keyword,bool isPagging, bool? isGetAll);
        Task<PaginationUtility<AmazonJewelryDataToWalmart_Dto>> GetDataForWalmart(PaginationParam pagination, string keyword,bool isPagging);
        Task<OperationResult> Add(AmazonJewelryDataFeedItemV3_Dto dto);
        Task<OperationResult> Delete(AmazonJewelryDataFeedItemV3_Dto dto);
        Task<OperationResult> Update(AmazonJewelryDataFeedItemV3_Dto dto);
        Task<OperationResult> GenDataByExcel(IFormFile fileExcel);
        Task<List<string>> GetAllItemSKU(string item_sku_category);
        Task<OperationResult> RemoveMultipleProduct(List<string> listSKU);
    }
}