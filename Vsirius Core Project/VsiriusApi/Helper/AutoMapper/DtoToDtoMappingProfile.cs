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

            CreateMap<PimApi.Dtos.ProductDto, AmazonIntegrationDataApi.Dtos.AmazonJewelryDataFeedItemV3_Dto>();
            #endregion

            #region Walmart
            CreateMap<StullerOrderProcessAPI.Dtos.StullerOrderDTO, WalmartIntegrationDataApi.Dtos.Order.OrderStullerDTO>();
            CreateMap<StullerOrderProcessAPI.Models.ItemStuller, WalmartIntegrationDataApi.Dtos.Order.ItemStuller>();

            CreateMap<WalmartIntegrationDataApi.Dtos.Qgold.OrderQgoldParam, OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject>();
            CreateMap<WalmartIntegrationDataApi.Dtos.Qgold.ItemQgoldParam, OrderProcessDomain.AmazonSeller.QgoldFtpOrderItem>();
            #endregion
        }
    }
}
