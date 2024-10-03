using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.BulkExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProductApi._Repositories.Interfaces;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using VsiriusDataModel.DataModel;
using System.Linq.Expressions;
using System.Reflection;
using VsiriusDataModel.DataContext;
using Nest;
using System.Collections.Immutable;

namespace ProductApi._Services.Services
{
    public class VsiriusProductService : IVsiriusProductService
    {
        private readonly IVsiriusProductRepository _vsiriusProductRepository;
        private readonly IVsiriusFilterRepository _vsiriusFilterRepository;
        private readonly ValidationVsirius _valid;
        private readonly IVsiriusValidProductRepository _vsiriusValidProductRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IVsiriusHomeProductRepository _vsiriusHomeProductRepository;
        private readonly VsiriusContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IElasticSearchProductService _elasticSearchProductService;
        public VsiriusProductService(
            IVsiriusProductRepository vsiriusProductRepository,
            ValidationVsirius valid, IMapper mapper,
            IVsiriusFilterRepository vsiriusFilterRepository,
            MapperConfiguration mapperConfiguration,
            IVsiriusValidProductRepository vsiriusValidProductRepository,
            IVsiriusHomeProductRepository vsiriusHomeProductRepository,
            VsiriusContext context,
            IHttpContextAccessor httpContextAccessor,
            IElasticSearchProductService elasticSearchProductService)
        {
            _vsiriusProductRepository = vsiriusProductRepository;
            _valid = valid;
            _mapper = mapper;
            _vsiriusFilterRepository = vsiriusFilterRepository;
            _mapperConfiguration = mapperConfiguration;
            _vsiriusValidProductRepository = vsiriusValidProductRepository;
            _vsiriusHomeProductRepository = vsiriusHomeProductRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _elasticSearchProductService = elasticSearchProductService;
        }

        public async Task<OperationResult> CreateProduct(VsiriusProductDto dto)
        {
            var isExist = await _vsiriusProductRepository.AnyAsync(x => x.item_sku == dto.item_sku);
            if (isExist)
            {
                return new OperationResult { Success = false, Message = "Product already exists!" };
            }

            VsiriusProductDto msg = _valid.IsValid(dto);
            var propertiesValid = msg.GetType().GetProperties();

            var checkError = false;
            var errorString = "";
            foreach (PropertyInfo property in propertiesValid)
            {
                object value = property.GetValue(msg);

                if (value != null)
                {
                    checkError = true;
                    errorString += property.Name + " " + value?.ToString() + "; ";
                }
            }
            if (checkError)
                return new OperationResult { Success = false, Data = msg, Message = errorString };
            var data = _mapper.Map<VsiriusProduct>(dto);
            _vsiriusProductRepository.Add(data);
            try
            {
                await _vsiriusProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Data = data };
            }
            catch (System.Exception ex)
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

        public async Task<PaginationUtility<VsiriusProductSearchDto>> GetAll(
            List<VsiriusFilterParamDto>? filterParam,
            PaginationParam pagination,
            SearchProductParam searchParam)
        {
            return await _elasticSearchProductService.GetListSkuFromTextSearch(filterParam, pagination, searchParam);
            //string fullTextSearch = searchParam?.Keyword == null ? string.Empty : searchParam?.Keyword.Trim().ToLower()!;

            //var predicate = PredicateBuilder.New<VsiriusProduct>(x => x.IsDelete == false);
            //if (searchParam?.IncludeNoStock != true)
            //{
            //    predicate.And(x => x.quantity.CompareTo("0") > 0);
            //}

            //VsiriusProductDto dto = new();
            //var propertiesValid = dto.GetType().GetProperties();

            //if (filterParam != null && filterParam.Count != 0)
            //{
            //    // lọc list các filterParam
            //    var checkFilterParam = false;
            //    foreach (var item in filterParam.Where(x => x.Value.Count > 0))
            //    {
            //        // lọc các filed của VsiriusProductDto
            //        foreach (PropertyInfo property in propertiesValid)
            //        {
            //            if (property.Name == item.FieldName)
            //            {
            //                checkFilterParam = true;
            //                var lambda = CreateFieldCheckLambda<VsiriusProduct>(property.Name, item.Value);
            //                predicate = predicate.And(lambda);
            //            }
            //        }
            //    }
            //    if (!checkFilterParam)
            //        predicate = predicate.And(x => false);
            //}

            //if (!string.IsNullOrEmpty(fullTextSearch))
            //{
                //var lstSkuElastic = await _elasticSearchProductService.GetListSkuFromTextSearch(fullTextSearch);
                //predicate.And(x => lstSkuElastic.Contains(x.item_sku));  
            //}


            //if (searchParam != null && (searchParam.MaxPrice != null || searchParam.MinPrice != null))
            //{
            //    if (searchParam.MaxPrice != null)
            //    {
            //        predicate.And(x => x.standard_price <= searchParam.MaxPrice);
            //    }

            //    if (searchParam.MinPrice != null)
            //    {
            //        predicate.And(x => x.standard_price >= searchParam.MinPrice);
            //    }
            //}

            //try
            //{
            //    var data = _vsiriusProductRepository
            //        .FindAll(predicate).AsNoTracking()
            //        .Select(x => new VsiriusProductSearchDto()
            //        {
            //            item_sku = x.item_sku,
            //            item_name = x.item_name,
            //            bullet_point1 = x.bullet_point1,
            //            bullet_point2 = x.bullet_point2,
            //            bullet_point3 = x.bullet_point3,
            //            bullet_point4 = x.bullet_point4,
            //            bullet_point5 = x.bullet_point5,
            //            bullet_point6 = x.bullet_point6,
            //            bullet_point7 = x.bullet_point7,
            //            bullet_point8 = x.bullet_point8,
            //            bullet_point9 = x.bullet_point9,
            //            bullet_point10 = x.bullet_point10,
            //            standard_price = x.standard_price.ToString("0.00"),
            //            sale_price = x.sale_price,
            //            color_name = x.color_name,
            //            size_name = x.size_name,
            //            main_image_url = x.main_image_url,
            //            other_image_url1 = x.other_image_url1,
            //            image_standard_1 = x.image_standard_1,
            //            image_standard_2 = x.image_standard_2,
            //            image_standard_3 = x.image_standard_3,
            //            image_standard_4 = x.image_standard_4,
            //            image_standard_5 = x.image_standard_5,
            //            quantity = x.quantity,
            //            color_map = x.color_map,
            //            feed_product_type = x.feed_product_type,
            //            gem_type1 = x.gem_type1,
            //            item_type = x.item_type,
            //            metal_type = x.metal_type
            //        });

            //    var result = await PaginationUtility<VsiriusProductSearchDto>
            //        .CreateCustomAsync(data, pagination.PageNumber, pagination.PageSize, true);

            //    return result;

            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        }
        private static IReadOnlyList<string> AnalyzeKeyword(string fullTextSearch)
        {
            var result = fullTextSearch.Split(' ').Where(x => !string.IsNullOrEmpty(x) && x.Length >= 3).ToList();
            return result;
        }

        public async Task<List<VsiriusFilterParamDto>> GetFilter(string feed_product_type)
        {


            var predicate = PredicateBuilder.New<VsiriusFilter>(false);
            if (!string.IsNullOrWhiteSpace(feed_product_type))
            {
                predicate = predicate.Or(x => x.ProductType == feed_product_type && x.IsActive == true);
                predicate = predicate.Or(x => (string.IsNullOrWhiteSpace(x.ProductType) && x.FieldName != "feed_product_type" && x.IsActive == true));
            }
            else
            {
                predicate = predicate.And(x => (string.IsNullOrWhiteSpace(x.ProductType) && x.FieldName == "feed_product_type" && x.IsActive == true));
            }

            try
            {
                var data = await _vsiriusFilterRepository.FindAll(predicate).ToListAsync();

                var result = data.GroupBy(x => new { x.FieldName, x.LabelName, x.ProductType })
                    .Select(x => new VsiriusFilterParamDto
                    {
                        FieldName = x.Key.FieldName,
                        LabelName = x.Key.LabelName,
                        ProductType = x.Key.ProductType,
                        Value = x.Select(y => y.Value).ToList(),
                        GroupName = x.Select(y => y.GroupName).FirstOrDefault()
                    })
                    .OrderBy(x => x.ProductType)
                    .Distinct()
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                return new List<VsiriusFilterParamDto>();
            }
        }

        public async Task<VsiriusProductDto> GetProduct(string item_sku)
        {
            try
            {
                var data = await _vsiriusProductRepository.FindSingle(x => x.item_sku == item_sku);
                return _mapper.Map<VsiriusProductDto>(data);
            }
            catch (Exception ex)
            {
                return new VsiriusProductDto();
            }
        }

        async Task<string> ReadRowAsync2(int rowStart, int rowEnd, ExcelWorksheet worksheet)
        {
            var errorString = "";

            List<VsiriusProductDto> lstDataSuccess = new List<VsiriusProductDto>();
            for (int row = rowStart; row < rowEnd; row++)
            {
                var obj = new VsiriusProductDto();
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

                VsiriusProductDto msg = _valid.IsValid(obj);
                var propertiesValid = msg.GetType().GetProperties();
                var rowErr = string.Empty;
                foreach (PropertyInfo property in propertiesValid)
                {
                    var value = property.GetValue(msg)?.ToString();

                    if (value != null)
                    {
                        rowErr += $"{row}:{property.Name}: {value}; \n";
                    }
                }

                if (string.IsNullOrEmpty(rowErr))
                {
                    lstDataSuccess.Add(obj);
                }
                else
                {
                    errorString += rowErr;
                }

            }

            if (errorString == "")
            {
                var lstId = lstDataSuccess.Select(x => x.item_sku);
                var lstUpdateId = await _vsiriusProductRepository.FindAll(x => lstId.Contains(x.item_sku)).Select(x => x.item_sku).ToListAsync();
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
                var lstData = _mapper.Map<List<VsiriusProduct>>(lstDataSuccess);

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
            }

            return errorString;
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

        public async Task<OperationResult> RemoveProduct(string item_sku)
        {
            var data = await _vsiriusProductRepository.FirstOrDefaultAsync(x => x.item_sku == item_sku);
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "No Data" };
            }
            data.IsDelete = true;
            _vsiriusProductRepository.Update(data);

            var dataHome = await _vsiriusHomeProductRepository.FirstOrDefaultAsync(x => x.item_sku == item_sku);
            if (dataHome != null)
            {
                dataHome.IsDelete = true;
                _vsiriusHomeProductRepository.Update(dataHome);
            }

            try
            {
                await _vsiriusProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Successful delete!!!" };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> UpdateProduct(VsiriusProductDto dto)
        {
            var data = await _vsiriusProductRepository.FirstOrDefaultAsync(x => x.item_sku == dto.item_sku);
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "No Data" };
            }

            VsiriusProductDto msg = _valid.IsValid(dto);
            var propertiesValid = msg.GetType().GetProperties();

            var checkError = false;
            var errorString = "";
            foreach (PropertyInfo property in propertiesValid)
            {
                object value = property.GetValue(msg);

                if (value != null)
                {
                    checkError = true;
                    errorString += property.Name + " " + value?.ToString() + "; ";
                }
            }
            if (checkError)
                return new OperationResult { Success = false, Data = msg, Message = errorString };

            var dataUpdate = _mapper.Map<VsiriusProduct>(dto);
            dataUpdate.UpdatedDate = DateTime.Now;
            _vsiriusProductRepository.Update(dataUpdate);
            try
            {
                await _vsiriusProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Successful update!!!" };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<PaginationUtility<VsiriusProductDto>> GetAllLayer1(string layer1)
        {
            var predicate = PredicateBuilder.New<VsiriusProduct>(true);
            predicate = predicate.And(x => x.feed_product_type == layer1);
            PaginationParam pagination = new PaginationParam();

            try
            {
                var data = _vsiriusProductRepository.FindAll(predicate).ProjectTo<VsiriusProductDto>(_mapperConfiguration);
                return await PaginationUtility<VsiriusProductDto>.CreateNolockAsync(data.AsNoTracking(), pagination.PageNumber, pagination.PageSize, true);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<VsiriusFilterParamDto>> GetValueForAdd(string feed_product_type)
        {
            var predicate = PredicateBuilder.New<VsiriusValidProduct>(false);
            List<VsiriusFilterParamDto> result = new List<VsiriusFilterParamDto>();

            if (!string.IsNullOrWhiteSpace(feed_product_type))
            {
                predicate = predicate.Or(x => x.ProductType == feed_product_type);
                predicate = predicate.Or(x => (string.IsNullOrWhiteSpace(x.ProductType) && x.FieldName != "feed_product_type"));
            }
            else
            {
                return result;
            }

            try
            {
                var data = await _vsiriusValidProductRepository.FindAll(predicate).AsNoTracking().ToListAsync();
                result = data.GroupBy(x => new { x.FieldName, x.LabelName, x.ProductType })
                    .Select(x => new VsiriusFilterParamDto
                    {
                        FieldName = x.Key?.FieldName,
                        LabelName = x.Key?.LabelName,
                        ProductType = x.Key?.ProductType,
                        Value = x.Select(x => x.Value).ToList(),
                        GroupName = x.Select(x => x.GroupName).FirstOrDefault()
                    }).OrderBy(x => x.ProductType).Distinct().ToList();
            }
            catch (Exception ex)
            {
                return new List<VsiriusFilterParamDto>();
            }



            VsiriusProductDto dto = new VsiriusProductDto();
            var propertiesValid = dto.GetType().GetProperties();

            var indexNumber = 1;
            foreach (PropertyInfo property in propertiesValid)
            {
                var checkExist = false;
                foreach (var item in result)
                {
                    if (property.Name == item.FieldName)
                    {
                        checkExist = true;
                        break;
                    }
                }
                if (!checkExist && property.Name != "feed_product_type")
                {
                    result.Add(new VsiriusFilterParamDto
                    {
                        FieldName = property.Name,
                        Value = null,
                        GroupName = GetValueSection(indexNumber),
                    });
                }
                indexNumber++;
            }

            return result;
        }
        public static string GetValueSection(int number)
        {
            if (number <= 121)
            {
                return "Required";
            }
            else if (number >= 122 && number <= 138)
            {
                return "Images";
            }
            else if (number >= 139 && number <= 169)
            {
                return "Variation";
            }
            else if (number >= 170 && number <= 335)
            {
                return "Discovery";
            }
            else if (number >= 335 && number <= 383)
            {
                return "Product Enrichment";
            }
            else if (number >= 384 && number <= 454)
            {
                return "Dimensions";
            }
            else if (number == 455)
            {
                return "Fulfillment";
            }
            else if (number >= 456 && number <= 525)
            {
                return "Compliance";
            }
            else if (number >= 526 && number <= 547)
            {
                return "Offer";
            }
            return "B2B";

        }

        public async Task<List<string>> GetAllItemSKU(string item_sku_category)
        {
            var predicate = PredicateBuilder.New<VsiriusProduct>(x => x.IsDelete != true);
            if (string.IsNullOrWhiteSpace(item_sku_category))
            {
                return new List<string>();
            }
            else
            {
                predicate.And(x => x.item_sku.StartsWith(item_sku_category.ToUpper()));
            }

            var data = await _vsiriusProductRepository
                .FindAll(predicate).AsNoTracking()
                .Select(x => x.item_sku)
                .ToListAsync();
            return data;
        }

        public async Task<OperationResult> RemoveMultipleProduct(List<string> listSKU)
        {
            try
            {
                var batchs = Common.DivideList(listSKU, 200);
                foreach (var batch in batchs)
                {
                    var curDate = DateTime.Now;
                    await _context.VsiriusProduct.Where(x => listSKU.Contains(x.item_sku))
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

        public async Task<PaginationUtility<VsiriusProductDto>> GetAllBackOffice(
            List<VsiriusFilterParamDto>? filterParam,
            PaginationParam pagination,
            SearchProductParam searchParam)
        {
            return await _elasticSearchProductService.GetAllBackOffice(filterParam, pagination, searchParam);
        }

        public bool IsAllPropertiesNull(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);

                if (value != null)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<PaginationUtility<VsiriusProductDto>> GetData(PaginationParam pagination, string? keyword, bool isPagging)
        {
            try
            {
                var predicate = PredicateBuilder.New<VsiriusProduct>(x => x.IsDelete != true);
                if (!string.IsNullOrEmpty(keyword))
                {
                    predicate.And(x => x.item_name.Contains(keyword)
                        || x.feed_product_type.Contains(keyword) || x.item_sku.Contains(keyword)
                        || x.item_type.Contains(keyword) || x.metal_type.Contains(keyword)
                        || x.model_name.Contains(keyword) || x.color_name.Contains(keyword)
                        || x.item_type_name.Contains(keyword));
                }

                var query = _context.VsiriusProduct.AsNoTracking().Where(predicate).ProjectTo<VsiriusProductDto>(_mapperConfiguration);
                var result = await PaginationUtility<VsiriusProductDto>.CreateCustomAsync(query, pagination.PageNumber, pagination.PageSize, isPagging);

                return result;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
