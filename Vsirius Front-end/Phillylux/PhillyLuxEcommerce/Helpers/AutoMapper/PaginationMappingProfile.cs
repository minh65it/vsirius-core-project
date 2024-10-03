using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class PaginationMappingProfile : Profile
    {
        public PaginationMappingProfile()
        {
            CreateMap<PaginationDto, PaginationVM>();
        }
    }
}
