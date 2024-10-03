using AutoMapper;
using IdentityApi.Dtos.Auth;
using IdentityApi.Dtos.User;
using VsiriusDataModel.DataModel;
using Microsoft.AspNetCore.Identity;

namespace IdentityApi.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<Roles, RolesDto>();
            CreateMap<IdentityRole, RolesDto>();
            CreateMap<VsiriusUser, UserDto>();
            CreateMap<Employee, EmployeeDto>().ForMember(
                dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone)); ;
            CreateMap<RolesEmployee, RolesEmployeeDto>();
        }
    }
}
