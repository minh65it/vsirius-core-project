using PimApi.Dtos.Variation;

namespace PimApi._Services.Interfaces
{
    public interface IVariationService
    {
        Task<List<VariationDto>> GetAll();
        Task UploadVariationFromPIM();
    }
}
