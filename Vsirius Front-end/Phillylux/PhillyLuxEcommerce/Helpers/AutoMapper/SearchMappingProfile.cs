using AutoMapper;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Helpers.AutoMapper
{
    public class SearchMappingProfile : Profile
    {
        public SearchMappingProfile()
        {
            CreateMap<FilterDto, FilterVM>();
            CreateMap<PaginationDto, PaginationVM>();
        }
    }
}
