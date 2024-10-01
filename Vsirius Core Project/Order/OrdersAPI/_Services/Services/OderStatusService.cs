using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;
using OrdersAPI.Helpers.Utilities;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Services.Services
{
    public class OderStatusService : IOderStatusService
    {

        private readonly IOrderStatusRepository _repository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _logService;
        public OderStatusService(IOrderStatusRepository repository, IMapper mapper, MapperConfiguration mapperConfiguration, IHttpContextAccessor httpContextAccessor, ILogService logService)
        {
            _repository = repository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        public async Task<OperationResult> Create(OrderStatusDto dto)
        {
            var data = _mapper.Map<OrderStatus>(dto);
            _repository.Add(data);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult { Success = true, Data = data, Message = "Created Order Status Successfully" };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> Delete(int id)
        {
            var data = await _repository.FindSingle(x => x.Id == id);
            if (data != null)
            {
                _repository.Remove(data);
            }
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Deleted Order Status Successfully" };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginationUtility<OrderStatusDto>> GetData(PaginationParam pagination, string keyword)
        {
            var predicate = PredicateBuilder.New<OrderStatus>(true);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.ToLower();
                predicate.And(x => x.Name.ToLower().Contains(keyword) || x.Description.ToLower().Contains(keyword));
            }
            try
            {
                var data = _repository.FindAll().ProjectTo<OrderStatusDto>(_mapperConfiguration).AsNoTracking();
                return await PaginationUtility<OrderStatusDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
            
        }

        public async Task<List<KeyValuePair<int, string>>> GetListStatus()
        {
            try
            {
                return await _repository.FindAll(x => x.IsActive == true).Select(x => new KeyValuePair<int, string>(x.Id, x.Name)).ToListAsync();
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<KeyValuePair<int, string>>();
            }
        }

        public async Task<OperationResult> Update(OrderStatusDto dto)
        {
            var data = _mapper.Map<OrderStatus>(dto);
            _repository.Update(data);
            try
            {
                await _repository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Updated Order Status Successfully" };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
