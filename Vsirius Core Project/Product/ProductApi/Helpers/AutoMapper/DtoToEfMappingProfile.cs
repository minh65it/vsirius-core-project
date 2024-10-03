using AutoMapper;
using ProductApi.Dtos;
using VsiriusDataModel.DataModel;

namespace ProductApi.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<VsiriusProductDto, VsiriusProduct>()
                .ForMember(dest => dest.standard_price, opt => opt.MapFrom(src => decimal.Parse(src.standard_price ?? "0")))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilterParamDto, VsiriusFilter>();
            CreateMap<VsiriusFilterDto, VsiriusFilter>();
            CreateMap<VsiriusHomeProductDto, VsiriusHomeProduct>();
            CreateMap<VsiriusFilterDto, VsiriusValidProduct>();
        }
    }
}
