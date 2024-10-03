using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<VsiriusProductDto, ProductDetailVM>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.item_name))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.item_sku))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.standard_price))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.quantity))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.product_description))
                .ForMember(dest => dest.BulletPoints, opt => opt.MapFrom(src => $"{src.bullet_point1}|{src.bullet_point2}|{src.bullet_point3}" +
                $"|{src.bullet_point4}|{src.bullet_point5}|{src.bullet_point6}|{src.bullet_point7}" +
                $"|{src.bullet_point8}|{src.bullet_point9}|{src.bullet_point10}"))
                .ForMember(dest => dest.main_image_url, opt => opt.MapFrom(src => src.main_image_url))
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.item_type))
                .ForMember(dest => dest.MetalType, opt => opt.MapFrom(src => src.metal_type))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.color_name));

            CreateMap<VsiriusProductDto, SummaryProductVM>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.item_sku))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.item_name))
                .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => $"{src.bullet_point1} ..."))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.standard_price))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src => src.sale_price))
                .ForMember(dest => dest.MainImageUrl, opt => opt.MapFrom(src => src.main_image_url))
                .ForMember(dest => dest.SubImageUrl,
                opt => opt.MapFrom(src => $"{(string.IsNullOrWhiteSpace(src.other_image_url1) ? src.main_image_url : src.other_image_url1)}"))

                 .ForMember(dest => dest.image_standard_1, opt => opt.MapFrom(src => src.image_standard_1))
                .ForMember(dest => dest.image_standard_2, opt => opt.MapFrom(src => $"{(string.IsNullOrWhiteSpace(src.image_standard_2) ? src.image_standard_1 : src.image_standard_2)}"))
                .ForMember(dest => dest.image_standard_3, opt => opt.MapFrom(src => src.image_standard_3))
                .ForMember(dest => dest.image_standard_4, opt => opt.MapFrom(src => src.image_standard_4))
                .ForMember(dest => dest.image_standard_5, opt => opt.MapFrom(src => src.image_standard_5))
                ;
        }
    }
}
