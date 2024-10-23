using AutoMapper;
using CSRApi.Dtos;
using IdentityApi.Dtos.Auth;
using IdentityApi.Dtos.User;
using Microsoft.AspNetCore.Identity;

using VsiriusDataModel.DataModel;

namespace VsiriusApi.Helper.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            // CSR
            CreateMap<Favourites, FavouritesDto>();
            CreateMap<Contacts, ContactDto>();
            CreateMap<Notifications, NotificationDto>();
            CreateMap<DrivenHtmlPages, DrivenHtmlPagesDto>();

            // Identity
            CreateMap<Roles, RolesDto>();
            CreateMap<IdentityRole, RolesDto>();
            CreateMap<VsiriusUser, UserDto>();
            CreateMap<Employee, EmployeeDto>().ForMember(
                dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone)); ;
            CreateMap<RolesEmployee, RolesEmployeeDto>();

        }
    }
}
