using ProductApi.Helpers.Utilities;

namespace ProductApi._Services.Interfaces
{
    public interface IMapVSiriusDBToVSiriusWebDB
    {
        Task<OperationResult> MapToVSiriusDB();
        Task<OperationResult> UpdateFilter();
    }
}
