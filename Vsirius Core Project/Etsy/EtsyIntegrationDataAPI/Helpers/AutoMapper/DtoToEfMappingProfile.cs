using AutoMapper;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<ProductEtsyDto, ProductEtsy>();
        }
    }
}
