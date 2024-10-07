using AutoMapper;
using OrderProcessApi.DTOs;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;
using OrderProcessDomain.Stuller;

namespace OrderProcessApi.Utilities.Mapping
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<QgoldFtpOrderObject, OrderMarketplace>()
                .ForMember(des => des.Seller_Order_ID, act => act.MapFrom(src => src.Amz_Order_ID))
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();

            CreateMap<QgoldFtpOrderItem, OrderMarketplaceItem>().ReverseMap();
            CreateMap<OrderStuller, StullerOrderDTO>().ReverseMap();
            CreateMap<OrderProcessDomain.Stuller.InvoiceDetail, OrderProcessApi.DTOs.InvoiceDetail>().ReverseMap();
            //send mail questions
            CreateMap<QgoldQA, QgoldQADTO>().ReverseMap();
            CreateMap<OrderProcessDomain.Qgold.FilesInfo, DTOs.FilesInfo>().ReverseMap();
            CreateMap<OrderMarketplace, OrderMarketplaceDto>()
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();
            CreateMap<OrderMarketplaceItem, OrderMarketplaceItemDto>().ReverseMap();
            CreateMap<OrderAmazonSubmitted, OrderAmazonSubmittedDto>()
                .ForMember(des => des.Item, act => act.MapFrom(src => src.Item)).ReverseMap();
            CreateMap<ItemSubmitAmazon, ItemSubmitAmazonDto>().ReverseMap();
        }
    }
}
