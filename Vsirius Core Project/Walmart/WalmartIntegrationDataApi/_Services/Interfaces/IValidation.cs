using WalmartIntegrationDataApi.Dtos.Product;

namespace WalmartIntegrationDataApi._Services.Interfaces
{
    public interface IValidation
    {
        public WalmartDTO Validate(WalmartDTO data);
    }
}
