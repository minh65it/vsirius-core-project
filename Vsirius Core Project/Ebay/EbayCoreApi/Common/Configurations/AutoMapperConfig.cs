using AutoMapper;
using EbayCoreApi.Common.Mappers;

namespace EbayCoreApi.Common.Configurations
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
        }
    }
}
