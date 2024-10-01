using AutoMapper;
using OrdersAPI.Dtos;
using VsiriusDataModel.DataModel;

namespace OrdersAPI.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<VsiriusOrders, VsiriusOrderDto>();
            CreateMap<OrderStatus, OrderStatusDto>();

            CreateMap<VsiriusCart, VsiriusCartDetailsDto>();
            CreateMap<VsiriusCartDetails, VsiriusCartDetailsDto>();
            CreateMap<VsiriusCart, VsiriusListCartDto>();
        }
    }
}
