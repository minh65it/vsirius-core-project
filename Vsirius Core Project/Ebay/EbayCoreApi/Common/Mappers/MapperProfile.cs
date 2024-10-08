using AutoMapper;
using EbayCoreApi.Dtos;
using EbayCoreApi.Models;

namespace EbayCoreApi.Common.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile() {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<Order, OrderDto>()
            .ForMember(x => x.Supplier, opt => opt.MapFrom(src => src.OrderDetails.Select(x => x.Sku).FirstOrDefault().StartsWith("Q") ? "Qgold" : "Stuller"))
            ;
            CreateMap<OrderDto, Order>();
            CreateMap<OrderDetailDto, OrderDetail>();
            CreateMap<OrderDetail, OrderDetailDto>();
        }
    }
}
