
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Dtos.MongoDB;
using AmazonIntegrationDataApi.Models;
using AutoMapper;

namespace AmazonIntegrationDataApi.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<AmazonJewelryDataFeedItemV3_Dto, AmazonJewelryDataFeedItem>();
            CreateMap<AmazonJewelryDataFeedItemV3_Dto, AmazonJewelryDataFeedItemV3>()
                .ForMember(des => des.IsDeleted, act => act.MapFrom(src => src.isDelete));
            CreateMap<AmazonJewelryDataToAmazon_Dto, AmazonJewelryDataForUpdate>();
            CreateMap<ReturnFBMOrderRowDto, ReturnFBMOrder>();
        }
    }
}