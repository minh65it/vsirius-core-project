using AutoMapper;
using CSRApi.Dtos;
using VsiriusDataModel.DataModel;

namespace CSRApi.Helpers.AutoMapper
{
    public class DtoToEfMappingProfile : Profile
    {
        public DtoToEfMappingProfile()
        {
            CreateMap<FavouritesDto, Favourites>();
            CreateMap<ContactDto, Contacts>();
            CreateMap<NotificationDto, Notifications>();
            CreateMap<IdentityUserIdDto, SecretIdentityUserInfo>();
            CreateMap<DrivenHtmlPagesDto, DrivenHtmlPages>();
        }
    }
}
