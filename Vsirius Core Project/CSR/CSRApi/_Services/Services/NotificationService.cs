using AutoMapper;
using CSRApi._Repositories.Interfaces;
using CSRApi._Repositories.Repositories;
using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;
using Korzh.EasyQuery.Linq;
using Korzh.EasyQuery.Services;
using LinqKit;
using System.Data.Entity;
using VsiriusDataModel.DataModel;
using VSiriusSystemLog.WriteLog;

namespace CSRApi._Services.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationDetailRepository _notificationDetailRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationMainRepository _notificationMainRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILoggerCustom _logService;
        public NotificationService(INotificationRepository notificationRepository, INotificationDetailRepository notificationDetailRepository,
            IEmployeeRepository employeeRepository, IUserRepository userRepository, IMapper mapper, INotificationMainRepository notificationMainRepository
            , IHttpContextAccessor httpContextAccessor, ILoggerCustom logService)
        {
            _notificationRepository = notificationRepository;
            _notificationDetailRepository = notificationDetailRepository;
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _notificationMainRepository = notificationMainRepository;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        public async Task<OperationResult> Create(NotificationDto dto)
        {
            var data = _mapper.Map<Notifications>(dto);
            var transaction = await _notificationRepository.BeginTransactionAsync();
            try
            {
                _notificationRepository.Add(data);
                await _notificationRepository.SaveChangesAsync();
                if (dto.isAll == true)
                {
                    if (dto.StartDate < dto.EndDate)
                    {
                        bool isOverlap = _notificationMainRepository.FindAll().ToList().Any(i =>
                                            (dto.StartDate >= i.StartDate && dto.StartDate < i.EndDate) ||
                                            (dto.EndDate > i.StartDate && dto.EndDate <= i.EndDate) ||
                                            (i.StartDate >= dto.StartDate && i.StartDate < dto.EndDate) ||
                                            (i.EndDate > dto.StartDate && i.EndDate <= dto.EndDate)
                                        );

                        if (isOverlap)
                        {
                            return new OperationResult { Success = false, Message = "Notification is showing, please select the date again" };
                        }

                        NotificationMain notificationMain = new()
                        {
                            NotificationId = data.Id.ToString(),
                            IsShow = true,
                            StartDate = dto.StartDate,
                            EndDate = dto.EndDate,
                        };
                        _notificationMainRepository.Add(notificationMain);
                        await _notificationMainRepository.SaveChangesAsync();
                    }
                }
                if (dto.Users.Count > 0)
                {
                    foreach (var userId in dto.Users)
                    {
                        NotificationDetail notificationDetail = new()
                        {
                            EmployeeId = data.CreateBy,
                            NotificationId = data.Id.ToString(),
                            isView = false,
                            UserId = userId,
                        };
                        _notificationDetailRepository.Add(notificationDetail);
                    }
                    await _notificationDetailRepository.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return new OperationResult { Success = true, Message = "Add notification successfully", Data = data };
            }

            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> Delete(string id)
        {
            var data = await _notificationRepository.FindSingle(x => x.Id.ToString() == id);
            try
            {
                if (data.isAll == true)
                {
                    var notificationMain = _notificationMainRepository.FindAll(x => x.NotificationId == data.Id.ToString()).ToList();
                    _notificationMainRepository.RemoveMultiple(notificationMain);
                    await _notificationMainRepository.SaveChangesAsync();
                }
                else
                {
                    var notificationDetail = _notificationDetailRepository.FindAll(x => x.NotificationId == data.Id.ToString()).ToList();
                    _notificationDetailRepository.RemoveMultiple(notificationDetail);
                    await _notificationDetailRepository.SaveChangesAsync();
                }

                _notificationRepository.Remove(data);
                await _notificationRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Deleted notification" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginationUtility<NotificationDto>> GetData(PaginationParam pagination, string keyword)
        {
            try
            {
                var data = _notificationRepository.FindAll().FullTextSearchQuery(keyword).Select(x => new NotificationDto()
                {
                    Content = x.Content,
                    CreateBy = x.CreateBy,
                    CreateTime = x.CreateTime,
                    Id = x.Id,
                    Name = x.Name,
                    isAll = x.isAll
                }).OrderByDescending(x => x.CreateTime).AsNoTracking();
                return await PaginationUtility<NotificationDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex) 
            {
                await _logService.CreateLogError(exception: ex.Message);
                return null;
            }
        }

        public async Task<PaginationUtility<NotificationDto>> GetNotificationByUser(PaginationParam pagination, string email)
        {
            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                {
                    var list = new List<NotificationDto>();
                    return PaginationUtility<NotificationDto>.Create(list, pagination.PageNumber, pagination.PageSize, true); ;
                }
                var data = _notificationDetailRepository.FindAll(x => x.UserId == user.Id)
                           .Join(_notificationRepository.FindAll(),
                           x => x.NotificationId,
                           y => y.Id.ToString(),
                           (x, y) => new { Detail = x, Noti = y })
                           .Select(x => new NotificationDto()
                           {
                               Id = x.Noti.Id,
                               Content = x.Noti.Content,
                               CreateTime = x.Noti.CreateTime,
                               Name = x.Noti.Name,
                               CreateBy = x.Noti.CreateBy,
                               isView = x.Detail.isView
                           }).OrderByDescending(x => x.CreateTime);
                return await PaginationUtility<NotificationDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return null;
            }
        }

        public async Task<NotificationDto> GetNotificationMain()
        {
            var time = DateTime.Now.Date;
            var predicate = PredicateBuilder.New<NotificationMain>(x => x.EndDate.Date >= time && x.IsShow == true);
            try
            {
                var item = await _notificationMainRepository.FindSingle(predicate);
                if (item != null)
                {
                    var data = _notificationRepository.FindAll(x => x.Id == Guid.Parse(item.NotificationId))
                          .Select(x => new NotificationDto()
                          {
                              Content = x.Content,
                              EndDate = item.EndDate
                          }).FirstOrDefault();
                    if (data != null)
                    {
                        return data;
                    }
                }
                return new NotificationDto();
            }
            catch(Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new NotificationDto();
            }
            
        }

        public async Task<List<NotificationDto>> GetNotificationNotRead(string email)
        {
            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == email);
                var data = _notificationDetailRepository.FindAll(x => x.UserId == user.Id && x.isView == false)
                           .Join(_notificationRepository.FindAll(),
                           x => x.NotificationId,
                           y => y.Id.ToString(),
                           (x, y) => new { Detail = x, Noti = y })
                           .Select(x => new NotificationDto()
                           {
                               Id = x.Noti.Id,
                               Content = x.Noti.Content,
                               CreateTime = x.Noti.CreateTime,
                               Name = x.Noti.Name,
                               CreateBy = x.Noti.CreateBy,
                               isView = x.Detail.isView
                           }).OrderByDescending(x => x.CreateTime).ToList();
                return data;
            }
            catch(Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new List<NotificationDto>();
            }
           
        }
        public async Task<OperationResult> UpdateView(string email, string notificationId)
        {
            var user = await _userRepository.FirstOrDefaultAsync(x => x.Email == email);
            var data = await _notificationDetailRepository.FindSingle(x => x.NotificationId == notificationId && x.UserId == user.Id);
            data.isView = true;
            try
            {
                _notificationDetailRepository.Update(data);
                await _notificationRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Seen notifation" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
