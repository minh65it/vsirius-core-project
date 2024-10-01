using AutoMapper;
using CSRApi._Repositories.Interfaces;
using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using VsiriusDataModel.DataModel;

namespace CSRApi._Services.Services
{
    public class PageViewService : IPageViewService
    {
        private readonly IPageViewRepository _pageViewRepository;
        private readonly ISecretUserRepository _secretUserRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _logService;

        public PageViewService(IPageViewRepository pageViewRepository, IMapper mapper, ISecretUserRepository secretUserRepository, IHttpContextAccessor httpContextAccessor, ILogService logService)
        {
            _secretUserRepository = secretUserRepository;
            _pageViewRepository = pageViewRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        public async Task IncreaseView(CountViewDto dto)
        {
            try
            {
                var curDate = DateTime.Now;
                int curDay = curDate.Day;
                int curMonth = curDate.Month;
                int curYear = curDate.Year;

                var obj = await _pageViewRepository.FindSingle(
                    x => x.Day.Equals(curDay) && x.Month.Equals(curMonth) && x.Year.Equals(curYear)
                    && x.PageUrl.Equals(dto.PageURL) && x.PageTitle.Equals(dto.PageTitle) && x.PageParam.Equals(dto.PageParam));

                if (obj == null)
                {
                    PageView pageViewInfo = new()
                    {
                        FirstView = curDate,
                        LastView = curDate,
                        Day = curDay,
                        Month = curMonth,
                        Year = curYear,
                        DateViewCount = 1,
                        PageParam = dto.PageParam,
                        PageTitle = dto.PageTitle,
                        PageUrl = dto.PageURL
                    };
                    _pageViewRepository.Add(pageViewInfo);
                }
                else
                {
                    obj.LastView = curDate;
                    obj.DateViewCount = obj.DateViewCount + 1;
                    _pageViewRepository.Update(obj);
                }
                await _pageViewRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
            }
        }

        public async Task<bool> SaveInfoUser(IdentityUserIdDto dto)
        {
            try
            {
                var obj = _mapper.Map<SecretIdentityUserInfo>(dto);
                _secretUserRepository.Add(obj);
                await _secretUserRepository.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }
    }
}
