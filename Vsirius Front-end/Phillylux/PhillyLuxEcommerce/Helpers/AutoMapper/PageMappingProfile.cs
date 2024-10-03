using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class PageMappingProfile : Profile
    {
        public PageMappingProfile()
        {
            CreateMap<ContactVM, ContactDto>()
               .ForMember(contact => contact.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(contact => contact.Phone, opt => opt.MapFrom(src => src.Phone))
               .ForMember(contact => contact.FullName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(contact => contact.Content, opt => opt.MapFrom(src => src.Message));

            CreateMap<UserDto, ContactVM>()
               .ForMember(contact => contact.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(contact => contact.FullName, opt => opt.MapFrom(src => src.FullName))
               .ForMember(contact => contact.Phone, opt => opt.MapFrom(src =>
                $"{(string.IsNullOrWhiteSpace(src.PhoneNumber) ? "" : $"{src.PhoneNumber!.Substring(2)}")}"))
               ;

            CreateMap<PageInfoDto, PageVM>();
        }
    }
}
