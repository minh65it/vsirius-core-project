using AutoMapper;

namespace VsiriusApi.Helper.AutoMapper
{
    public class DtoToDtoMappingProfile : Profile
    {
        public DtoToDtoMappingProfile()
        {
            #region ProductAI
            CreateMap<PimApi.Dtos.ProductDto, ProductAiApi.DTOs.PimProductDto>();
            #endregion

            #region Product
            CreateMap<PimApi.Dtos.ProductDto, ProductApi.Dtos.VsiriusProductDto>();
            CreateMap<ProductAiApi.DTOs.ProductAiDto, ProductApi.Helpers.httpClient.ProductAiDto>();
            #endregion

            #region Amazon
            CreateMap<OrderProcessApi.DTOs.QgoldOrderDTO, AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor.QgoldOrderDto>();
            CreateMap<OrderProcessDomain.Qgold.ItemQgold, AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers.QgoldInvoiceItem>();
            #endregion
        }
    }
}
