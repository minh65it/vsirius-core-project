using AutoMapper;
using PackageApi.Dtos.Claim;
using System.Security.Claims;

namespace PackageApi.Utilities.Mapping
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<ClaimParam, Claim>().ReverseMap();
            CreateMap<ClaimDTO, Claim>().ReverseMap();
        }
    }
}
