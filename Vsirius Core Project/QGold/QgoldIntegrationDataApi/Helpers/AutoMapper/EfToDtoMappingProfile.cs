

using AutoMapper;
using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Models;

namespace QgoldIntegrationDataApi.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<QgoldProduct, QgoldCsvData_DTO>();
            CreateMap<QgoldProduct, QgoldData>();
        }
    }
}