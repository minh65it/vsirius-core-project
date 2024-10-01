using CSRApi.Dtos;

namespace CSRApi._Services.Interfaces
{
    public interface IPageViewService
    {
        Task IncreaseView(CountViewDto pageViewDto);
        Task<bool> SaveInfoUser(IdentityUserIdDto dto);
    }
}
