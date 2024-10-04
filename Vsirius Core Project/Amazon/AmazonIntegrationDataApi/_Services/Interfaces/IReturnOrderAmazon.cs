using AmazonIntegrationDataApi.Dtos.MongoDB;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Utilities;

namespace AmazonIntegrationDataApi._Services.Interfaces
{
    public interface IReturnOrderAmazon
    {
        Task<OperationResult> GetNewReturnOrders();
        Task<PaginationResult<ReturnFBMOrderRowDto>> GetReturnOrders(PaginationParam paginationParam, ReturnFBMOrderParams seachParam);
        Task<ReturnFBMOrderRowDto> GetDetail(string amazonRMAID);
    }
}
