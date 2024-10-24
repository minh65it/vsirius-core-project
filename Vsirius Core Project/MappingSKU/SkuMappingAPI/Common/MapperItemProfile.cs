using AutoMapper;
using SkuMappingAPI.DTOs;
using SkuMappingAPI.Models;

namespace SkuMappingAPI.Common
{
    public class MapperItemProfile : Profile
    {
        public MapperItemProfile() 
        {
            CreateMap<QgoldToWalmartDTO, SKUInfo>();
            CreateMap<SKUInfo, QgoldToWalmartDTO>();

        }
    }
}
