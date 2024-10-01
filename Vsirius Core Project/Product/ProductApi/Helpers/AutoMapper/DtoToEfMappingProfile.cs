using AutoMapper;
using ProductApi.Dtos;
using ProductApi.Models;

namespace ProductApi.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<VsiriusProductDto, VsiriusProduct>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilterParamDto, VsiriusFilter>();
            CreateMap<VsiriusFilterDto, VsiriusFilter>();
            CreateMap<VsiriusHomeProductDto, VsiriusHomeProduct>();
            CreateMap<VsiriusFilterDto, VsiriusValidProduct>();
        }
    }
}
