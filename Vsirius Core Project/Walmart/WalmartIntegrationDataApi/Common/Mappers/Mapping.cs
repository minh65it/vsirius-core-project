using AutoMapper;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Dtos.Return;
using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi.Common.Mappers
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<WalmartDTO, Walmart>();
            CreateMap<Walmart, WalmartDTO>();

            CreateMap<ReturnOrder, ReturnOrderDTO>();
            CreateMap<ReturnOrderDTO, ReturnOrder>();
        }
    }
}
