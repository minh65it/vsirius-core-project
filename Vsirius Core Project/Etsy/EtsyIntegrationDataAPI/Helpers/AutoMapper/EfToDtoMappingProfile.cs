using AutoMapper;
using EtsyIntegrationDataAPI.Dtos.OrderEtsy;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Models;

namespace EtsyIntegrationDataAPI.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<ProductEtsy, ProductEtsyDto>()
                .ForMember(dest => dest.materials, opt => opt.MapFrom(src => src.materials.Select(x => x.material).ToList()))
                .ForMember(dest => dest.tags, opt => opt.MapFrom(src => src.tags.Select(x => x.tag).ToList()))
                .ForMember(dest => dest.styles, opt => opt.MapFrom(src => src.styles.Select(x => x.style).ToList()))
                .ForMember(dest => dest.image_ids, opt => opt.MapFrom(src => src.image_ids.Select(x => x.listing_image_id).ToList()))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => new Dtos.ProductEtsyDto.PriceDto() 
                {
                    amount = src.price,
                    currency_code = "",
                    divisor = null,
                }));
            CreateMap<Order, OrderDto>();

            CreateMap<Shipment, ShipmentDto>();
            CreateMap<Transaction, TransactionDto>();
            CreateMap<Refund, RefundDto>();
        }
    }
}
