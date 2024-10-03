using AutoMapper;
using ProductApi.Dtos;
using VsiriusDataModel.DataModel;

namespace ProductApi.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<VsiriusProduct, VsiriusProductDto>()
                .ForMember(dest => dest.standard_price, opt => opt.MapFrom(src => src.standard_price.ToString("0.00")))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilter, VsiriusFilterParamDto>();
            CreateMap<VsiriusFilter, VsiriusFilterDto>();
            CreateMap<VsiriusHomeProduct, VsiriusHomeProductDto>();
            CreateMap<VsiriusValidProduct, VsiriusFilterDto>();
        }
    }
}
