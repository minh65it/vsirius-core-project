using AutoMapper;
using PackageApi.Dtos.Claim;
using PackageApi.Models;

namespace PackageApi.Utilities.Mapping
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<Claim, ClaimParam>().ReverseMap().ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentType, opt => opt.Ignore());
            CreateMap<Claim, ClaimDTO>().ReverseMap();
        }
    }
}
