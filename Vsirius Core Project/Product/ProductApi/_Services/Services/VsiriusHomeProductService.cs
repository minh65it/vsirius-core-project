using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductApi._Repositories.Interfaces;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using ProductApi.Models;

namespace ProductApi._Services.Services
{
    public class VsiriusHomeProductService : IVsiriusHomeProductService
    {
        private readonly IVsiriusProductRepository _vsiriusProductRepository;
        private readonly IVsiriusHomeProductRepository _vsiriusHomeProductRepository;
        private readonly IMapper _mapper;
        private ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VsiriusHomeProductService(
            IVsiriusProductRepository vsiriusProductRepository,
            IMapper mapper,
            IVsiriusHomeProductRepository vsiriusHomeProductRepository,
            ILogService logService,
            IHttpContextAccessor httpContextAccessor)
        {
            _vsiriusProductRepository = vsiriusProductRepository;
            _mapper = mapper;
            _vsiriusHomeProductRepository = vsiriusHomeProductRepository;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> CreateUpdateHomeProduct(VsiriusHomeProductDto dto)
        {
            var checkData = await _vsiriusHomeProductRepository.FindSingle(x => x.item_sku == dto.item_sku);
            if (checkData != null)
            {
                dto.IsDelete = false;
                dto.UpdatedDate = DateTime.Now;
                dto.UpdatedBy = null;
                var data = _mapper.Map<VsiriusHomeProduct>(dto);
                _vsiriusHomeProductRepository.Update(data);
            }
            else
            {
                dto.IsDelete = false;
                dto.UpdatedDate = null;
                dto.UpdatedBy = null;
                dto.CreatedDate = DateTime.Now;
                dto.CreatedBy = null;
                var data = _mapper.Map<VsiriusHomeProduct>(dto);
                _vsiriusHomeProductRepository.Add(data);
            }

            try
            {
                var mess = checkData != null ? "Update successful" : "Create success";
                    await _vsiriusHomeProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = mess};
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<List<VsiriusProductDto>> GetAllHomeProduct(VsiriusHomeProductParams param)
        {
            var predicate = PredicateBuilder.New<VsiriusHomeProduct>(false);
            if (param.Filter.ToLower() == VsiriusHomeProductFilterParams.FilterPopular)
                predicate.And(x => x.IsPopular == true && x.IsDelete == false);
            if (param.Filter.ToLower() == VsiriusHomeProductFilterParams.FilterLatest)
                predicate.And(x => x.IsLatest == true && x.IsDelete == false);

            try
            {
                var homeProduct = await _vsiriusHomeProductRepository.FindAll(predicate).Select(x => x.item_sku).ToListAsync();
                var product = _vsiriusProductRepository.FindAll();

                var data = new List<VsiriusProduct>();
                int indexHome = 1;
                foreach (var item in homeProduct)
                {
                    var findData = await _vsiriusProductRepository.FindSingle(x => x.item_sku == item);
                    if (findData != null)
                    {
                        data.Add(findData);
                        indexHome++;
                    }
                    if (indexHome > param.TakeProduct)
                        break;
                }
                var dataMap = _mapper.Map<List<VsiriusProductDto>>(data);
                return dataMap;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<VsiriusProductDto>();
            }
        }

        public async Task<OperationResult> RemoveHomeProduct(string item_sku)
        {
            var checkData = await _vsiriusHomeProductRepository.FindSingle(x => x.item_sku == item_sku);
            if (checkData == null)
            {
                return new OperationResult { Success = false, Message = "Product not exists!" };
            }

            _vsiriusHomeProductRepository.Remove(checkData);
            try
            {
                await _vsiriusHomeProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<List<VsiriusProductDto>> GetRelatedProduct(VsiriusHomeProductParams param)
        {
            var predicate = PredicateBuilder.New<VsiriusProduct>(false);
            if (!string.IsNullOrWhiteSpace(param.Filter))
            {
                predicate.And(x => x.item_type.ToLower() == param.Filter.ToLower());
            }

            try
            {
                var data = await _vsiriusProductRepository.FindAll(predicate).Take(param.TakeProduct).ToListAsync();
                return _mapper.Map<List<VsiriusProductDto>>(data);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<VsiriusProductDto>();
            }
        }

        public async Task<List<VsiriusHomeProductDto>> GetAllHomeProductBackend()
        {
            try
            {
                var dataHome = _vsiriusHomeProductRepository.FindAll();
                var data = _vsiriusProductRepository.FindAll();
                var result = await dataHome.LeftJoin(data, x => x.item_sku, y => y.item_sku, (x, y) => new VsiriusHomeProductDto
                {
                    item_sku = x.item_sku,
                    item_name = y.item_name,
                    IsActive = x.IsActive,
                    IsPopular = x.IsPopular,
                    IsLatest = x.IsLatest,

                }).ToListAsync();

                return result;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<VsiriusHomeProductDto>();
            }
        }
    }
}
