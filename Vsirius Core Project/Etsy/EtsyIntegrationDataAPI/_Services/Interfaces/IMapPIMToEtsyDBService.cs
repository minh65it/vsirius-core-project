using EtsyIntegrationDataAPI.Utilities;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IMapPIMToEtsyDBService
    {
        Task<OperationResult> MapToEtsyDB();
    }
}
