using AutoMapper;
using CSRApi.Dtos;
using IdentityApi.Dtos.Auth;
using IdentityApi.Dtos.User;
using Microsoft.AspNetCore.Identity;
using OrdersAPI.Dtos;
using ProductApi.Dtos;
using VsiriusDataModel.DataModel;
using VsiriusSystem.Dtos;
using VsiriusSystem.Models;

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

            // Product
            CreateMap<VsiriusProduct, VsiriusProductDto>()
                .ForMember(dest => dest.standard_price, opt => opt.MapFrom(src => src.standard_price.ToString("0.00")))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilter, VsiriusFilterParamDto>();
            CreateMap<VsiriusFilter, VsiriusFilterDto>();
            CreateMap<VsiriusHomeProduct, VsiriusHomeProductDto>();
            CreateMap<VsiriusValidProduct, VsiriusFilterDto>();

            // Order
            CreateMap<VsiriusOrders, VsiriusOrderDto>();
            CreateMap<OrderStatus, OrderStatusDto>();

            CreateMap<VsiriusCart, VsiriusCartDetailsDto>();
            CreateMap<VsiriusCartDetails, VsiriusCartDetailsDto>();
            CreateMap<VsiriusCart, VsiriusListCartDto>();

            // System
            CreateMap<Log, VsiriusSystem.Dtos.LogDto>();
            CreateMap<JobInfo, JobInfoDto>();
            CreateMap<JobInfo, CreateJobInfoDto>();
        }
    }
}
