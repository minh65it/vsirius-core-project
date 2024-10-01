using AutoMapper;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<VsiriusProduct, VsiriusProductDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilter, VsiriusFilterParamDto>();
            CreateMap<VsiriusFilter, VsiriusFilterDto>();
            CreateMap<VsiriusHomeProduct, VsiriusHomeProductDto>();
            CreateMap<VsiriusValidProduct, VsiriusFilterDto>();
        }
    }
}
