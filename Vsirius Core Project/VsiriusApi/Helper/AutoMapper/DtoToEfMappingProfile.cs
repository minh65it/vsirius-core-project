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

            // Product
            CreateMap<VsiriusProductDto, VsiriusProduct>()
                .ForMember(dest => dest.standard_price, opt => opt.MapFrom(src => decimal.Parse(src.standard_price ?? "0")))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilterParamDto, VsiriusFilter>();
            CreateMap<VsiriusFilterDto, VsiriusFilter>();
            CreateMap<VsiriusHomeProductDto, VsiriusHomeProduct>();
            CreateMap<VsiriusFilterDto, VsiriusValidProduct>();

            // Order
            CreateMap<VsiriusOrderDto, VsiriusOrders>();
            CreateMap<OrderStatusDto, OrderStatus>();

            CreateMap<VsiriusCartDto, VsiriusCart>();
            CreateMap<VsiriusCartDetailsDto, VsiriusCartDetails>()
                .ForMember(cartDetail => cartDetail.CartDetailId, opt => opt.MapFrom(src => Guid.NewGuid()));

            // System
            CreateMap<VsiriusSystem.Dtos.LogDto, Log>();
            CreateMap<JobInfoDto, JobInfo>();
            CreateMap<CreateJobInfoDto, JobInfo>();

        }
    }
}
