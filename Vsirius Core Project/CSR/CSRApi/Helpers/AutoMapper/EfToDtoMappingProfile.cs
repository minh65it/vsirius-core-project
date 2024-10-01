using AutoMapper;
using CSRApi.Dtos;
using VsiriusDataModel.DataModel;

namespace CSRApi.Helpers.AutoMapper
{
    public class EfToDtoMappingProfile : Profile
    {
        public EfToDtoMappingProfile()
        {
            CreateMap<Favourites, FavouritesDto>();
            CreateMap<Contacts, ContactDto>();
            CreateMap<Notifications, NotificationDto>();
            CreateMap<DrivenHtmlPages, DrivenHtmlPagesDto>();
        }
    }
}
