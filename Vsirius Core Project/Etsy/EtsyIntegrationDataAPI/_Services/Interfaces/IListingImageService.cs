using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;

namespace EtsyIntegrationDataAPI._Services.Interfaces
{
    public interface IListingImageService
    {
        Task<List<ImageProductDto>> UploadListingImage(string authorization, long listing_id);
    }
}
