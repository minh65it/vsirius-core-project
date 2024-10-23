using AutoMapper;
using CSRApi.Dtos;
using IdentityApi.Dtos.Auth;
using IdentityApi.Dtos.User;
using Microsoft.AspNetCore.Identity;
using VsiriusDataModel.DataModel;


namespace VsiriusApi.Helper.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            // CSR
            CreateMap<FavouritesDto, Favourites>();
            CreateMap<ContactDto, Contacts>();
            CreateMap<NotificationDto, Notifications>();
            CreateMap<IdentityUserIdDto, SecretIdentityUserInfo>();
            CreateMap<DrivenHtmlPagesDto, DrivenHtmlPages>();

            // Identity
            CreateMap<RolesDto, Roles>();
            CreateMap<RolesDto, IdentityRole>();
            CreateMap<UserDto, VsiriusUser>();
            CreateMap<EmployeeDto, Employee>().ForMember(
                dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<RolesEmployeeDto, RolesEmployee>();

        }
    }
}
