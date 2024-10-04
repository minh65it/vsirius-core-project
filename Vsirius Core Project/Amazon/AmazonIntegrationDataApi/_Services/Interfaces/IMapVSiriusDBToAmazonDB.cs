using AmazonIntegrationDataApi.Helpers.Utilities;

namespace AmazonIntegrationDataApi._Services.Interfaces
{
    public interface IMapVSiriusDBToAmazonDB
    {
        Task<OperationResult> MapToAmazonDB();
    }
}
