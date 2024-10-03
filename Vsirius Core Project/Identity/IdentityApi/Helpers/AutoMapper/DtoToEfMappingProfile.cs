using AutoMapper;
using IdentityApi.Dtos.Auth;
using IdentityApi.Dtos.User;
using VsiriusDataModel.DataModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<RolesDto, Roles>();
            CreateMap<RolesDto, IdentityRole>();
            CreateMap<UserDto, VsiriusUser>();
            CreateMap<EmployeeDto, Employee>().ForMember(
                dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<RolesEmployeeDto, RolesEmployee>();
        }
    }
}
