using AutoMapper;
using PimApi.Dtos;
using PimApi.Dtos.OrderDto;
using PimApi.Models;

namespace PimApi.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile() 
        {
            CreateMap<Product, ProductDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Order, OrderDto>();
        }
    }
}
