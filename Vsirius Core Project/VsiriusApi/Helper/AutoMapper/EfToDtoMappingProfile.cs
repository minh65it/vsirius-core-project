using AmazonIntegrationDataApi.Dtos.MongoDB;
using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
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
using OrderProcessApi.DTOs;
using OrderProcessDomain.Qgold;
using PimApi.Dtos.OrderDto;
using PimApi.Models;
using EbayCoreApi.Models;

namespace VsiriusApi.Helper.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            #region CSR
            CreateMap<Favourites, FavouritesDto>();
            CreateMap<Contacts, ContactDto>();
            CreateMap<Notifications, NotificationDto>();
            CreateMap<DrivenHtmlPages, DrivenHtmlPagesDto>();
            #endregion

            #region Identity
            CreateMap<Roles, RolesDto>();
            CreateMap<IdentityRole, RolesDto>();
            CreateMap<VsiriusUser, UserDto>();
            CreateMap<Employee, EmployeeDto>().ForMember(
                dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone)); ;
            CreateMap<RolesEmployee, RolesEmployeeDto>();
            #endregion

            #region Product
            CreateMap<VsiriusProduct, VsiriusProductDto>()
                .ForMember(dest => dest.standard_price, opt => opt.MapFrom(src => src.standard_price.ToString("0.00")))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<VsiriusFilter, VsiriusFilterParamDto>();
            CreateMap<VsiriusFilter, VsiriusFilterDto>();
            CreateMap<VsiriusHomeProduct, VsiriusHomeProductDto>();
            CreateMap<VsiriusValidProduct, VsiriusFilterDto>();
            #endregion

            #region Order
            CreateMap<VsiriusOrders, VsiriusOrderDto>();
            CreateMap<OrderStatus, OrderStatusDto>();

            CreateMap<VsiriusCart, VsiriusCartDetailsDto>();
            CreateMap<VsiriusCartDetails, VsiriusCartDetailsDto>();
            CreateMap<VsiriusCart, VsiriusListCartDto>();
            #endregion

            #region System
            CreateMap<Log, VsiriusSystem.Dtos.LogDto>();
            CreateMap<JobInfo, JobInfoDto>();
            CreateMap<JobInfo, CreateJobInfoDto>();
            #endregion

            #region Amazon
            CreateMap<AmazonJewelryDataFeedItem, AmazonJewelryDataFeedItemV3_Dto>();
            CreateMap<AmazonJewelryDataFeedItemV3, AmazonJewelryDataFeedItemV3_Dto>();
            CreateMap<AmazonJewelryDataForUpdate, AmazonJewelryDataToAmazon_Dto>();
            CreateMap<ReturnFBMOrder, ReturnFBMOrderRowDto>();
            CreateMap<QgoldFtpOrderObject, AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor.OrderMarketplaceDto>()
                .ForMember(des => des.Seller_Order_ID, act => act.MapFrom(src => src.Amz_Order_ID))
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();

            CreateMap<QgoldFtpOrderItem,AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor.OrderMarketplaceItem>().ReverseMap();
            #endregion

            #region Qgold
            CreateMap<QgoldProduct, QgoldCsvData_DTO>();
            CreateMap<QgoldProduct, QgoldData>();
            #endregion

            #region Walmart
            CreateMap<Walmart, WalmartDTO>();
            CreateMap<ReturnOrder, ReturnOrderDTO>();
            #endregion

            #region QgoldOrder
            CreateMap<QgoldFtpOrderObject, OrderMarketplace>()
                .ForMember(des => des.Seller_Order_ID, act => act.MapFrom(src => src.Amz_Order_ID))
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();

            CreateMap<OrderProcessDomain.AmazonSeller.QgoldFtpOrderItem, OrderProcessDomain.Qgold.OrderMarketplaceItem>().ReverseMap();
            CreateMap<OrderProcessDomain.Stuller.OrderStuller, StullerOrderDTO>().ReverseMap();
            CreateMap<OrderProcessDomain.Stuller.InvoiceDetail, OrderProcessApi.DTOs.InvoiceDetail>().ReverseMap();
            //send mail questions
            CreateMap<QgoldQA, QgoldQADTO>().ReverseMap();
            CreateMap<OrderProcessDomain.Qgold.FilesInfo, OrderProcessApi.DTOs.FilesInfo>().ReverseMap();
            CreateMap<OrderMarketplace, OrderProcessApi.DTOs.OrderMarketplaceDto>()
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();
            CreateMap<OrderProcessDomain.Qgold.OrderMarketplaceItem, OrderMarketplaceItemDto>().ReverseMap();
            CreateMap<OrderAmazonSubmitted, OrderAmazonSubmittedDto>()
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();
            CreateMap<ItemSubmitAmazon, ItemSubmitAmazonDto>().ReverseMap();
            #endregion

            #region Pim
            CreateMap<PimApi.Models.Product, PimApi.Dtos.ProductDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<PimApi.Models.Order, OrderDto>();
            #endregion

            #region Ebay
            CreateMap<EbayCoreApi.Models.Product, EbayCoreApi.Dtos.ProductDto>();
            CreateMap<EbayCoreApi.Models.Order, EbayCoreApi.Dtos.OrderDto>()
            .ForMember(x => x.Supplier, opt => opt.MapFrom(src => src.OrderDetails.Select(x => x.Sku).FirstOrDefault().StartsWith("Q") ? "Qgold" : "Stuller"));
            CreateMap<EbayCoreApi.Models.OrderDetail, EbayCoreApi.Dtos.OrderDetailDto>();
            #endregion
        }
    }
}
