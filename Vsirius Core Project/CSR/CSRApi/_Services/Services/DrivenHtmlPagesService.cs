using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSRApi._Repositories.Interfaces;
using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;
using Korzh.EasyQuery.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataModel;

namespace CSRApi._Services.Services
{
    public class DrivenHtmlPagesService : IDrivenHtmlPagesService
    {
        private readonly IDrivenHtmlPagesRepository _drivenHtmlPagesRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _logService;
        public DrivenHtmlPagesService(IDrivenHtmlPagesRepository drivenHtmlPagesRepository, IMapper mapper, MapperConfiguration mapperConfiguration, IHttpContextAccessor httpContextAccessor, ILogService logService)
        {
            _drivenHtmlPagesRepository = drivenHtmlPagesRepository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        public async Task<OperationResult> Create(DrivenHtmlPagesDto dto)
        {
            var data = _mapper.Map<DrivenHtmlPages>(dto);
            data.IsDelete = false;
            data.CreatedDate = DateTime.Now;
            try
            {
                _drivenHtmlPagesRepository.Add(data);
                await _drivenHtmlPagesRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Add Driven Html Pages succeed" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> Delete(DrivenHtmlPagesDto dto)
        {
            var data = await _drivenHtmlPagesRepository.FindById(dto.Id);
            if(data == null)
            {
                return new OperationResult { Success = true, Message = "Driven Html Pages does not exist" };
            }
            data.IsDelete = true;
            try
            {
                _drivenHtmlPagesRepository.Update(data);
                await _drivenHtmlPagesRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Delete Driven Html Pages succeed" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginationUtility<DrivenHtmlPagesDto>> GetData(PaginationParam pagination, string searchText)
        {
            var predicate = PredicateBuilder.New<DrivenHtmlPages>(x => x.IsDelete == false);

            try
            {
                var data = _drivenHtmlPagesRepository.FindAll(predicate).FullTextSearchQuery(searchText).ProjectTo<DrivenHtmlPagesDto>(_mapperConfiguration);
                var result = await PaginationUtility<DrivenHtmlPagesDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
                return result;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
           
        }

        public async Task<DrivenHtmlPagesDto> GetDetail(int id)
        {
            try
            {
                var data = await _drivenHtmlPagesRepository.FindById(id);
                if (data == null)
                {
                    return null;
                }
                return _mapper.Map<DrivenHtmlPagesDto>(data);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new DrivenHtmlPagesDto();
            }
            
        }

        public async Task<List<DrivenHtmlPagesDto>> GetListData()
        {
            try
            {
                var data = await _drivenHtmlPagesRepository.FindAll(x => x.IsDelete == false).Select(x => new DrivenHtmlPagesDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Url = x.Url
                }).ToListAsync();
                return data;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<DrivenHtmlPagesDto>();  
            }
            
        }

        public async Task<OperationResult> Update(DrivenHtmlPagesDto dto)
        {
            var dataCheck = await _drivenHtmlPagesRepository.FindSingle(x => x.Id == dto.Id);
            if (dataCheck == null)
            {
                return new OperationResult { Success = true, Message = "Driven Html Pages does not exist" };
            }
            var data = _mapper.Map<DrivenHtmlPages>(dto);
            data.CreatedDate = dto.CreatedDate;
            data.UpdatedDate = DateTime.Now;
            try
            {
                _drivenHtmlPagesRepository.Update(data);
                await _drivenHtmlPagesRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Update Driven Html Pages succeed" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
