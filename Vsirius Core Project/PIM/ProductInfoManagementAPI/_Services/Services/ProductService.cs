using System.Linq.Expressions;
using System.Reflection;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using EFCore.BulkExtensions;
using AutoMapper.QueryableExtensions;
using PimApi._Repositories.Interfaces;
using PimApi.Helpers.Utilities;
using PimApi._Services.Interfaces;
using PimApi.Context;
using PimApi.Models;
using PimApi.Dtos;

namespace PimApi._Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductService(
            IProductRepository productRepository,
            MapperConfiguration mapperConfiguration,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            DataContext context)
        {
            _productRepository = productRepository;
            _mapperConfiguration = mapperConfiguration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<OperationResult> CreateProduct(ProductDto dto)
        {
            var isExist = await _productRepository.AnyAsync(x => x.item_sku == dto.item_sku);
            if (isExist)
            {
                return new OperationResult { Success = false, Message = "Product already exists!" };
            }

            var data = _mapper.Map<Product>(dto);
            _productRepository.Add(data);
            try
            {
                await _productRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Data = data };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
        public static Expression<Func<T, bool>> CreateFieldCheckLambda<T>(string fieldName, List<string> l)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, fieldName);
            var constant = Expression.Constant(l);
            var containsMethod = typeof(List<string>).GetMethod("Contains", new[] { typeof(string) });
            var containsCall = Expression.Call(constant, containsMethod, property);
            return Expression.Lambda<Func<T, bool>>(containsCall, parameter);
        }

        public async Task<PaginationUtility<VsiriusProductSearchDto>> GetAll(GetAllParmas filterParam, PaginationParam pagination)
        {
            var predicate = PredicateBuilder.New<Product>(x => x.IsDelete != true);

            ProductDto dto = new ProductDto();
            var propertiesValid = dto.GetType().GetProperties();

            if (filterParam.FilterParamDto.Count != 0)
            {
                // lọc list các filterParam
                var checkFilterParam = false;
                foreach (var item in filterParam.FilterParamDto.Where(x => x.Value.Count > 0))
                {
                    // lọc các filed của VsiriusProductDto
                    foreach (PropertyInfo property in propertiesValid)
                    {
                        if (property.Name == item.FieldName)
                        {
                            checkFilterParam = true;
                            var lambda = CreateFieldCheckLambda<Product>(property.Name, item.Value);
                            predicate = predicate.And(lambda);
                        }
                    }
                }
                if (!checkFilterParam)
                    predicate = predicate.And(x => false);
            }
            if (!string.IsNullOrEmpty(filterParam.Keyword))
            {
                predicate.And(x => x.item_name.Contains(filterParam.Keyword) || x.standard_price.Contains(filterParam.Keyword)
                    || x.feed_product_type.Contains(filterParam.Keyword) || x.item_sku.Contains(filterParam.Keyword)
                    || x.item_type.Contains(filterParam.Keyword) || x.metal_type.Contains(filterParam.Keyword)
                    || x.model_name.Contains(filterParam.Keyword) || x.color_name.Contains(filterParam.Keyword)
                    || x.item_type_name.Contains(filterParam.Keyword));
            }
            if (filterParam.marketplace != null)
            {
                switch (filterParam.marketplace)
                {
                    case Marketplace.IsVSirius:
                        predicate.And(x => x.IsVSirius == true);
                        break;
                    case Marketplace.IsPhillyLux:
                        predicate.And(x => x.IsPhillyLux == true);
                        break;
                    case Marketplace.IsAmazon:
                        predicate.And(x => x.IsAmazon == true);
                        break;
                    case Marketplace.IsWalmart:
                        predicate.And(x => x.IsWalmart == true);
                        break;
                    case Marketplace.IsEbay:
                        predicate.And(x => x.IsEbay == true);
                        break;
                    case Marketplace.IsEtsy:
                        predicate.And(x => x.IsEtsy == true);
                        break;
                    default:
                        predicate.And(x => x.IsDelete != true);
                        break;
                }
            }

            try
            {
                var data = _productRepository.FindAll(predicate).Select(x => new VsiriusProductSearchDto()
                {
                    item_sku = x.item_sku,
                    item_name = x.item_name,
                    bullet_point1 = x.bullet_point1,
                    standard_price = x.standard_price,
                    sale_price = x.sale_price,
                    color_name = x.color_name,
                    size_name = x.size_name,
                    main_image_url = x.main_image_url,
                    other_image_url1 = x.other_image_url1
                });

                var result = await PaginationUtility<VsiriusProductSearchDto>.CreateNolockAsync(data.AsNoTracking(), pagination.PageNumber, pagination.PageSize, true);

                return result;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<string>> GetAllItemSKU(string item_sku_category)
        {
            var predicate = PredicateBuilder.New<Product>(x => x.IsDelete != true);
            if (string.IsNullOrWhiteSpace(item_sku_category))
            {
                return new List<string>();
            }
            else
            {
                predicate.And(x => x.item_sku.StartsWith(item_sku_category.ToUpper()));
            }

            var data = await _productRepository
                .FindAll(predicate).AsNoTracking()
                .Select(x => x.item_sku)
                .ToListAsync();
            return data;
        }

        async Task<string> ReadRowAsync2(int rowStart, int rowEnd, ExcelWorksheet worksheet)
        {
            List<ProductDto> lstDataSuccess = new List<ProductDto>();
            for (int row = rowStart; row < rowEnd; row++)
            {
                var obj = new ProductDto();
                var properties = obj.GetType().GetProperties(); // lấy danh sách các property của đối tượng

                int coulm = 1;
                foreach (var prop in properties)
                {
                    var columnName = prop.Name.Count(); // lấy tên của property
                    var cellValue = worksheet.Cells[row, coulm].Value; // lấy giá trị của cell

                    if (cellValue != null && cellValue.ToString() != "")
                    {
                        prop.SetValue(obj, Convert.ChangeType(cellValue, prop.PropertyType), null);
                    }
                    coulm++;
                }
                lstDataSuccess.Add(obj);

            }

            var lstId = lstDataSuccess.Select(x => x.item_sku);
            var lstUpdateId = await _productRepository.FindAll(x => lstId.Contains(x.item_sku)).Select(x => x.item_sku).ToListAsync();
            var curDate = DateTime.Now;

            //lst Update
            lstDataSuccess.Where(x => lstUpdateId.Contains(x.item_sku)).ForEach(x =>
            {
                x.UpdatedDate = curDate;
            });

            //lst Add
            lstDataSuccess.Where(x => !lstUpdateId.Contains(x.item_sku)).ForEach(x =>
            {
                x.CreatedDate = curDate;
            });
            var lstData = _mapper.Map<List<Product>>(lstDataSuccess);

            int batchSize = 10_000; // Số lượng tối đa trong mỗi danh sách con
            int numBatches = (int)Math.Ceiling((double)lstData.Count / batchSize);

            for (int i = 0; i < numBatches; i++)
            {
                var batch = lstData
                    .Skip(i * batchSize)
                    .Take(batchSize)
                    .ToList();

                await _context.BulkInsertOrUpdateAsync(batch);
            }
            return "";
        }

        public async Task<OperationResult> ImportFile(IFormFile fileExcel)
        {
            if (fileExcel != null)
            {
                long fileSize = fileExcel.Length; // kích thước tệp tin tính bằng byte
                if (fileSize > 50_000_000) // kiểm tra nếu kích thước lớn hơn 50 MB
                {
                    return new OperationResult(false, "File is too big");
                }
            }

            string? fileExtension = Path.GetExtension(fileExcel.FileName);
            string?[] arrNameFile = new string?[]
            {
                ".xlsx", ".xls",".csv"
            };

            if (!arrNameFile.Contains(fileExtension))
            {
                return new OperationResult(false, "Not file excel");
            }

            var stream = fileExcel.OpenReadStream();
            var workbook = new ExcelPackage(stream);

            // Access the first worksheet
            var worksheet = workbook.Workbook.Worksheets[0];
            var range = worksheet.Dimension;
            var totalRow = range.End.Row;

            try
            {
                var result = await ReadRowAsync2(4, totalRow + 1, worksheet);
                if (result != "")
                {
                    return new OperationResult { Success = false, Message = result };
                }
                else
                {
                    return new OperationResult { Success = true };
                }
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> RemoveMultipleProduct(List<string> listSKU)
        {
            try
            {
                var batchs = Common.DivideList(listSKU, 200);
                foreach (var batch in batchs)
                {
                    var curDate = DateTime.Now;
                    await _context.Products.Where(x => listSKU.Contains(x.item_sku))
                         .ExecuteUpdateAsync(s => s
                         .SetProperty(i => i.IsDelete, i => true)
                         .SetProperty(i => i.UpdatedDate, i => curDate));
                }

                return new OperationResult { Success = true, Message = "Successful delete!!!" };

            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> RemoveProduct(string item_sku)
        {
            var data = await _productRepository.FirstOrDefaultAsync(x => x.item_sku == item_sku);
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "No Data" };
            }
            data.IsDelete = true;
            _productRepository.Update(data);

            try
            {
                await _productRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Successful delete!!!" };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> UpdateProduct(ProductDto dto)
        {
            var data = await _productRepository.FirstOrDefaultAsync(x => x.item_sku == dto.item_sku);
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "No Data" };
            }


            var dataUpdate = _mapper.Map<Product>(dto);
            dataUpdate.UpdatedDate = DateTime.Now;
            _productRepository.Update(dataUpdate);
            try
            {
                await _productRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Successful update!!!" };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<ProductDto> GetProduct(string item_sku)
        {
            try
            {
                var data = await _productRepository.FindSingle(x => x.item_sku == item_sku);
                return _mapper.Map<ProductDto>(data);
            }
            catch (Exception ex)
            {
                return new ProductDto();
            }
        }

        public async Task<PaginationUtility<ProductDto>> GetData(PaginationParam pagination, SearchParams searchParams, bool isPagging)
        {
            try
            {
                var predicate = PredicateBuilder.New<Product>(x => x.IsDelete != true);
                if (searchParams.marketplace != null)
                {
                    switch (searchParams.marketplace)
                    {
                        case Marketplace.IsVSirius:
                            predicate.And(x => x.IsVSirius == true);
                            break;
                        case Marketplace.IsPhillyLux:
                            predicate.And(x => x.IsPhillyLux == true);
                            break;
                        case Marketplace.IsAmazon:
                            predicate.And(x => x.IsAmazon == true);
                            break;
                        case Marketplace.IsWalmart:
                            predicate.And(x => x.IsWalmart == true);
                            break;
                        case Marketplace.IsEbay:
                            predicate.And(x => x.IsEbay == true);
                            break;
                        case Marketplace.IsEtsy:
                            predicate.And(x => x.IsEtsy == true);
                            break;
                        default:
                            predicate.And(x => x.IsDelete != true);
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(searchParams.Keyword))
                {
                    predicate.And(x => x.item_name.Contains(searchParams.Keyword) || x.standard_price.Contains(searchParams.Keyword)
                        || x.feed_product_type.Contains(searchParams.Keyword) || x.item_sku.Contains(searchParams.Keyword)
                        || x.item_type.Contains(searchParams.Keyword) || x.metal_type.Contains(searchParams.Keyword)
                        || x.model_name.Contains(searchParams.Keyword) || x.color_name.Contains(searchParams.Keyword)
                        || x.item_type_name.Contains(searchParams.Keyword));
                }

                var query = _context.Products.AsNoTracking().Where(predicate).ProjectTo<ProductDto>(_mapperConfiguration);
                var result = await PaginationUtility<ProductDto>.CreateCustomAsync(query, pagination.PageNumber, pagination.PageSize, isPagging);

                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<PaginationUtility<ProductDto>> GetAllItemMartketplace(Marketplace martketplace, PaginationParam pagination)
        {
            try
            {
                var predicate = PredicateBuilder.New<Product>(x => x.IsDelete != true);
                if (martketplace == null)
                {
                    return null;
                }
                switch (martketplace)
                {
                    case Marketplace.IsVSirius:
                        predicate.And(x => x.IsVSirius == true);
                        break;
                    case Marketplace.IsPhillyLux:
                        predicate.And(x => x.IsPhillyLux == true);
                        break;
                    case Marketplace.IsAmazon:
                        predicate.And(x => x.IsAmazon == true);
                        break;
                    case Marketplace.IsWalmart:
                        predicate.And(x => x.IsWalmart == true);
                        break;
                    case Marketplace.IsEbay:
                        predicate.And(x => x.IsEbay == true);
                        break;
                    case Marketplace.IsEtsy:
                        predicate.And(x => x.IsEtsy == true);
                        break;
                    default:
                        return null;
                }

                var query = _context.Products.AsNoTracking().Where(predicate).ProjectTo<ProductDto>(_mapperConfiguration);
                var result = await PaginationUtility<ProductDto>.CreateCustomAsync(query, pagination.PageNumber, pagination.PageSize, true);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<OperationResult> UpdateMartketplace(Marketplace martketplace, List<string> listSKU, bool isValue)
        {
            try
            {
                var batchs = Common.DivideList(listSKU, 200);
                foreach (var batch in batchs)
                {
                    var curDate = DateTime.Now;
                    await _context.Products
                        .Where(x => batch.Contains(x.item_sku)) // Check against batch instead of listSKU
                        .ForEachAsync(async s =>
                        {
                            // Update properties dynamically based on marketplace
                            s.GetType().GetProperty(martketplace.ToString())?.SetValue(s, isValue);
                            s.UpdatedDate = curDate;
                        });

                    // Save changes after each batch to improve efficiency
                    await _context.SaveChangesAsync();
                }

                return new OperationResult { Success = true, Message = "Successful delete!!!" };

            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginationUtility<ProductDto>> GetData(PaginationParam pagination, string? keyword, bool isPagging)
        {
            try
            {
                var predicate = PredicateBuilder.New<Product>(x => x.IsDelete != true);
                if (!string.IsNullOrEmpty(keyword))
                {
                    predicate.And(x => x.item_name.Contains(keyword)
                        || x.feed_product_type.Contains(keyword) || x.item_sku.Contains(keyword)
                        || x.item_type.Contains(keyword) || x.metal_type.Contains(keyword)
                        || x.model_name.Contains(keyword) || x.color_name.Contains(keyword)
                        || x.item_type_name.Contains(keyword));
                }

                var query = _context.Products.AsNoTracking().Where(predicate).ProjectTo<ProductDto>(_mapperConfiguration);
                var result = await PaginationUtility<ProductDto>.CreateCustomAsync(query, pagination.PageNumber, pagination.PageSize, isPagging);

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
