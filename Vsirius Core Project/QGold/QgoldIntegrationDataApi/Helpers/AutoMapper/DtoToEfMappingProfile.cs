using AutoMapper;
using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Models;

namespace QgoldIntegrationDataApi.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<QgoldCsvData_DTO, QgoldProduct>();

            CreateMap<QgoldData, QgoldProduct>();
        }
    }
}