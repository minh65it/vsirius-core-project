using AmazonIntegrationDataApi.Dtos.MongoDB;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Models;
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
using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Models;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Models;
using WalmartIntegrationDataApi.Dtos.Return;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Models;
using PackageApi.Models;


namespace VsiriusApi.Helper.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            #region CSR
            CreateMap<FavouritesDto, Favourites>();
            CreateMap<ContactDto, Contacts>();
            CreateMap<NotificationDto, Notifications>();
            CreateMap<IdentityUserIdDto, SecretIdentityUserInfo>();
            CreateMap<DrivenHtmlPagesDto, DrivenHtmlPages>();
            #endregion

            #region Package
            CreateMap<PackageApi.Dtos.Claim.ClaimParam, PackageApi.Models.Claim>().ReverseMap();
            CreateMap<PackageApi.Dtos.Claim.ClaimDTO, PackageApi.Models.Claim>().ReverseMap();
            #endregion

            #region Identity
            CreateMap<RolesDto, Roles>();
            CreateMap<RolesDto, IdentityRole>();
            CreateMap<UserDto, VsiriusUser>();
            CreateMap<EmployeeDto, Employee>().ForMember(
                dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));
            CreateMap<RolesEmployeeDto, RolesEmployee>();
            #endregion

            #region Product
            CreateMap<VsiriusProductDto, VsiriusProduct>()
                .ForMember(dest => dest.standard_price, opt => opt.MapFrom(src => decimal.Parse(src.standard_price ?? "0")))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilterParamDto, VsiriusFilter>();
            CreateMap<VsiriusFilterDto, VsiriusFilter>();
            CreateMap<VsiriusHomeProductDto, VsiriusHomeProduct>();
            CreateMap<VsiriusFilterDto, VsiriusValidProduct>();
            #endregion

            #region Order
            CreateMap<VsiriusOrderDto, VsiriusOrders>();
            CreateMap<OrderStatusDto, OrderStatus>();

            CreateMap<VsiriusCartDto, VsiriusCart>();
            CreateMap<VsiriusCartDetailsDto, VsiriusCartDetails>()
                .ForMember(cartDetail => cartDetail.CartDetailId, opt => opt.MapFrom(src => Guid.NewGuid()));
            #endregion

            #region System
            CreateMap<VsiriusSystem.Dtos.LogDto, Log>();
            CreateMap<JobInfoDto, JobInfo>();
            CreateMap<CreateJobInfoDto, JobInfo>();
            #endregion

            #region Amazon
            CreateMap<AmazonJewelryDataFeedItemV3_Dto, AmazonJewelryDataFeedItem>();
            CreateMap<AmazonJewelryDataFeedItemV3_Dto, AmazonJewelryDataFeedItemV3>()
                .ForMember(des => des.IsDeleted, act => act.MapFrom(src => src.isDelete));
            CreateMap<AmazonJewelryDataToAmazon_Dto, AmazonJewelryDataForUpdate>();
            CreateMap<ReturnFBMOrderRowDto, ReturnFBMOrder>();
            #endregion

            #region Qgold
            CreateMap<QgoldCsvData_DTO, QgoldProduct>();
            CreateMap<QgoldData, QgoldProduct>();
            #endregion

            #region Walmart
            CreateMap<WalmartDTO, Walmart>();
            CreateMap<ReturnOrderDTO, ReturnOrder>();
            #endregion

            #region Pim
            CreateMap<PimApi.Dtos.ProductDto,  PimApi.Models.Product>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            #endregion

            #region Ebay
            CreateMap<EbayCoreApi.Dtos.ProductDto, EbayCoreApi.Models.Product>();
            CreateMap<EbayCoreApi.Dtos.OrderDto, EbayCoreApi.Models.Order>();
            CreateMap<EbayCoreApi.Dtos.OrderDetailDto, EbayCoreApi.Models.OrderDetail>();
            #endregion

            #region Etsy
            CreateMap<ProductEtsyDto, ProductEtsy>();
            #endregion
        }
    }
}
