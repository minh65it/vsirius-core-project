using AutoMapper;
using OrdersAPI.Dtos;
using VsiriusDataModel.DataModel;

namespace OrdersAPI.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap< VsiriusOrderDto, VsiriusOrders>();
            CreateMap< OrderStatusDto, OrderStatus>();

            CreateMap<VsiriusCartDto, VsiriusCart>();
            CreateMap<VsiriusCartDetailsDto, VsiriusCartDetails>()
                .ForMember(cartDetail => cartDetail.CartDetailId, opt => opt.MapFrom(src => Guid.NewGuid()));
        }
    }
}
