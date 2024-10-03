using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class WishlistMappingProfile : Profile
    {
        public WishlistMappingProfile()
        {
            CreateMap<WishlistDto, WishlistVM>();

        }
    }
}
