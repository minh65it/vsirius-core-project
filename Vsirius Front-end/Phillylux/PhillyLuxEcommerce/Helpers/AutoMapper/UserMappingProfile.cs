using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDto, UserVM>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src =>
                $"{(string.IsNullOrWhiteSpace(src.PhoneNumber) ? "" : $"{src.PhoneNumber!.Substring(2)}")}")
                );
            CreateMap<UserVM, UserDto>();
        }
    }
}
