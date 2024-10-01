using AutoMapper;
using VsiriusSystem.Dtos;
using VsiriusSystem.Models;

namespace VsiriusSystem.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<Log, LogDto>();
            CreateMap<JobInfo, JobInfoDto>();
            CreateMap<JobInfo, CreateJobInfoDto>();
        }
    }
}
