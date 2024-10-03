using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductApi._Repositories.Interfaces;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using VsiriusDataModel.DataModel;

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
                return new OperationResult { Success = true, Message = mess };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<List<VsiriusProductDto>> GetAllHomeProduct(VsiriusHomeProductParams param)
        {
            if (string.IsNullOrWhiteSpace(param.Filter))
                return null;
            var predicate = PredicateBuilder.New<VsiriusHomeProduct>(x => x.IsActive == true);
            if (param.Filter.ToLower() == VsiriusHomeProductFilterParams.FilterPopular)
                predicate.And(x => x.IsPopular == true && x.IsDelete == false);
            if (param.Filter.ToLower() == VsiriusHomeProductFilterParams.FilterLatest)
                predicate.And(x => x.IsLatest == true && x.IsDelete == false);

            try
            {
                var data = await (from home in _vsiriusHomeProductRepository.FindAll(predicate).OrderByDescending(o => o.UpdatedDate).Select(x => x.item_sku)
                                  join products in _vsiriusProductRepository.FindAll(x => x.IsDelete != true)
                                  on home equals products.item_sku
                                  select new VsiriusProductDto()
                                  {
                                      item_sku = home,
                                      item_name = products.item_name,
                                      bullet_point1 = products.bullet_point1,
                                      standard_price = products.standard_price.ToString("0.00"),
                                      sale_price = products.sale_price,
                                      color_name = products.color_name,
                                      size_name = products.size_name,
                                      main_image_url = products.main_image_url,
                                      other_image_url1 = products.other_image_url1,
                                      image_standard_1 = products.other_image_url2,
                                      image_standard_2 = products.other_image_url3,
                                      image_standard_3 = products.other_image_url4,
                                      image_standard_4 = products.other_image_url5,
                                      image_standard_5 = products.other_image_url6,
                                  }).Take(param.TakeProduct).ToListAsync();
                return data;
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
            catch (Exception ex)
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
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<VsiriusHomeProductDto>();
            }
        }
    }
}
