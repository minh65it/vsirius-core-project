using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class HomeMappingProfile : Profile
    {
        public HomeMappingProfile()
        {
            CreateMap<VsiriusProductDto, HomeVMProduct>()
                .ForMember(homeVMProduct => homeVMProduct.SKU, opt => opt.MapFrom(src => src.item_sku))
                .ForMember(homeVMProduct => homeVMProduct.ProductName, opt => opt.MapFrom(src => src.item_name))
                .ForMember(homeVMProduct => homeVMProduct.BulletPoint1, opt => opt.MapFrom(src => src.bullet_point1))
                .ForMember(homeVMProduct => homeVMProduct.Price, opt => opt.MapFrom(src => src.standard_price))
                .ForMember(homeVMProduct => homeVMProduct.PriceSale, opt => opt.MapFrom(src => src.sale_price))
                .ForMember(homeVMProduct => homeVMProduct.Color, opt => opt.MapFrom(src => src.color_name))
                .ForMember(homeVMProduct => homeVMProduct.Size, opt => opt.MapFrom(src => src.size_name))
                .ForMember(homeVMProduct => homeVMProduct.main_image_url, opt => opt.MapFrom(src => src.main_image_url))
                .ForMember(homeVMProduct => homeVMProduct.other_image_url1, opt => opt.MapFrom(src => src.other_image_url1))
                .ForMember(homeVMProduct => homeVMProduct.image_standard_1, opt => opt.MapFrom(src => src.image_standard_1))
                .ForMember(homeVMProduct => homeVMProduct.image_standard_2, opt => opt.MapFrom(src => src.image_standard_2))
                .ForMember(homeVMProduct => homeVMProduct.image_standard_3, opt => opt.MapFrom(src => src.image_standard_3))
                .ForMember(homeVMProduct => homeVMProduct.image_standard_4, opt => opt.MapFrom(src => src.image_standard_4))
                .ForMember(homeVMProduct => homeVMProduct.image_standard_5, opt => opt.MapFrom(src => src.image_standard_5))
                ;

            CreateMap<VsiriusFilterDto, HomeVMFilter>()
                .ForMember(HomeVMFilter => HomeVMFilter.FieldName, opt => opt.MapFrom(src => src.FieldName))
                .ForMember(HomeVMFilter => HomeVMFilter.LabelName, opt => opt.MapFrom(src => src.LabelName))
                .ForMember(HomeVMFilter => HomeVMFilter.Value, opt => opt.MapFrom(src => src.Value))
            ;
        }
    }
}
