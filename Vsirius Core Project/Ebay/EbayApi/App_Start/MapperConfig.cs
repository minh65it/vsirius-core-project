using AutoMapper;
using EbayApi.Dtos;
using EbayApi.Models;
using EbayApi.Utils;
using System.Linq;

namespace EbayApi.App_Start
{
    public class MapperConfig
    {
        public static void InitialCreate()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, ProductDto>();
                cfg.CreateMap<ProductDto, Product>();
                cfg.CreateMap<Order, OrderDto>()               
                .ForMember(x => x.Supplier, opt => opt.MapFrom(src => src.OrderDetails.Select(x => x.Sku).FirstOrDefault().StartsWith("Q") ? "Qgold": "Stuller"))
                ;
                cfg.CreateMap<OrderDto, Order>();
                cfg.CreateMap<OrderDetailDto, OrderDetail>();
                cfg.CreateMap<OrderDetail, OrderDetailDto>();
            });

            AutoMapperHelper.Mapper = configuration.CreateMapper();
        }
    }
}