using AutoMapper;
using PackageApi.Dtos.Claim;
using PackageApi.Models;

namespace PackageApi.Utilities.Mapping
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<PackageApi.Models.Claim, PackageApi.Dtos.Claim.ClaimParam>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentType, opt => opt.Ignore());
            CreateMap<PackageApi.Models.Claim, PackageApi.Dtos.Claim.ClaimDTO>().ReverseMap();
        }
    }
}
