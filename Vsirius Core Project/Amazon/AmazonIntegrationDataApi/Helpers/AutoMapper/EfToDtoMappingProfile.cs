
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Dtos.MongoDB;
using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Models;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using AutoMapper;
using FikaAmazonAPI.ReportGeneration;

namespace AmazonIntegrationDataApi.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<AmazonJewelryDataFeedItem, AmazonJewelryDataFeedItemV3_Dto>();
            CreateMap<AmazonJewelryDataFeedItemV3, AmazonJewelryDataFeedItemV3_Dto>();
            CreateMap<AmazonJewelryDataForUpdate, AmazonJewelryDataToAmazon_Dto>();
            CreateMap<ReturnFBMOrder, ReturnFBMOrderRowDto>();
            CreateMap<QgoldFtpOrderObject, OrderMarketplaceDto>()
                .ForMember(des => des.Seller_Order_ID, act => act.MapFrom(src => src.Amz_Order_ID))
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();

            CreateMap<QgoldFtpOrderItem, OrderMarketplaceItem>().ReverseMap();
        }
    }
}