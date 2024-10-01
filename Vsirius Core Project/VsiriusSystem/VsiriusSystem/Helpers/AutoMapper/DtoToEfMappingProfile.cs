using AutoMapper;
using VsiriusSystem.Dtos;
using VsiriusSystem.Models;

namespace VsiriusSystem.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<LogDto, Log>();
            CreateMap<JobInfoDto, JobInfo>();
            CreateMap<CreateJobInfoDto, JobInfo>();
        }
    }
}
