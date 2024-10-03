using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderInformationVM, VsiriusOrderDto>().ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode));

            CreateMap<VsiriusListCartDto, OrderCartVM>()
               .ForMember(order => order.CartId, opt => opt.MapFrom(src => src.CartId))
               .ForMember(order => order.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(order => order.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(order => order.QuantityTotalItem, opt => opt.MapFrom(src => src.QuantityItem))
               .ForMember(order => order.UserId, opt => opt.MapFrom(src => src.UserId))
               .ForMember(order => order.UserName, opt => opt.MapFrom(src => src.UserName))
               .ForMember(order => order.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
               .ForMember(order => order.Tax, opt => opt.MapFrom(src => src.Tax))
            ;

            CreateMap<VsiriusListCartDetailsDto, OrderProductVM>()
               .ForMember(order => order.SKU, opt => opt.MapFrom(src => src.ProductId))
               .ForMember(order => order.ProductName, opt => opt.MapFrom(src => src.ProductName))
               .ForMember(order => order.Size, opt => opt.MapFrom(src => src.Size))
               .ForMember(order => order.PriceSale, opt => opt.MapFrom(src => src.PriceSale))
               .ForMember(order => order.Price, opt => opt.MapFrom(src => src.Price))
               .ForMember(order => order.QuantityItem, opt => opt.MapFrom(src => src.Quantity))
               .ForMember(order => order.Color, opt => opt.MapFrom(src => src.Color))
               .ForMember(order => order.main_image_url, opt => opt.MapFrom(src => src.Image));


            CreateMap<UserDto, OrderInformationVM>()
               .ForMember(order => order.UserId, opt => opt.MapFrom(src => src.Id))
               .ForMember(order => order.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(order => order.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
               .ForMember(order => order.Address1, opt => opt.MapFrom(src => src.Address1))
               .ForMember(order => order.City, opt => opt.MapFrom(src => src.City))
               .ForMember(order => order.Address1, opt => opt.MapFrom(src => src.Address1))
               .ForMember(order => order.PostCode, opt => opt.MapFrom(src => src.PostCode))
               .ForMember(order => order.Country, opt => opt.MapFrom(src => src.Country))
               .ForMember(order => order.TwoFactorEnabled, opt => opt.MapFrom(src => src.TwoFactorEnabled))
               .ForMember(order => order.EmailConfimed, opt => opt.MapFrom(src => src.EmailConfirmed))
               .ForMember(order => order.State, opt => opt.MapFrom(src => src.State));

            CreateMap<VsiriusOrderDto, OrderInformationVM>();

            CreateMap<VsiriusProductDto, OrderProductVM>()
                .ForMember(order => order.SKU, opt => opt.MapFrom(src => src.item_sku))
               .ForMember(order => order.Size, opt => opt.MapFrom(src => src.size_name))
               .ForMember(order => order.PriceSale, opt => opt.MapFrom(src => src.sale_price))
               .ForMember(order => order.Price, opt => opt.MapFrom(src => src.standard_price))
               .ForMember(order => order.ProductName, opt => opt.MapFrom(src => src.item_name))
               .ForMember(order => order.QuantityItem, opt => opt.MapFrom(src => src.quantity))
               .ForMember(order => order.Color, opt => opt.MapFrom(src => src.color_name))
               .ForMember(order => order.main_image_url, opt => opt.MapFrom(src => src.main_image_url));

            CreateMap<VsiriusOrderStatusDto, OrderStatusVM>();

        }
    }
}
