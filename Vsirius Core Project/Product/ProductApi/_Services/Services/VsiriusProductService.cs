using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProductApi._Repositories.Interfaces;
using ProductApi._Repositories.Repositories;
using ProductApi._Services.Interfaces;
using ProductApi.Data;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using ProductApi.Models;
using System.Linq.Expressions;
using System.Reflection;

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
        private ILogService _logService;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VsiriusProductService(
            IVsiriusProductRepository vsiriusProductRepository,
            ValidationVsirius valid, IMapper mapper,
            IVsiriusFilterRepository vsiriusFilterRepository,
            MapperConfiguration mapperConfiguration,
            IVsiriusValidProductRepository vsiriusValidProductRepository,
            IVsiriusHomeProductRepository vsiriusHomeProductRepository,
            ILogService logService, IConfiguration configuration, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _vsiriusProductRepository = vsiriusProductRepository;
            _valid = valid;
            _mapper = mapper;
            _vsiriusFilterRepository = vsiriusFilterRepository;
            _mapperConfiguration = mapperConfiguration;
            _vsiriusValidProductRepository = vsiriusValidProductRepository;
            _vsiriusHomeProductRepository = vsiriusHomeProductRepository;
            _logService = logService;
            _configuration = configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> CreateProduct(VsiriusProductDto dto)
        {
            var checkData = await _vsiriusProductRepository.FindSingle(x => x.item_sku == dto.item_sku);
            if (checkData != null)
            {
                return new OperationResult { Success = false, Message = "Product already exists!" };
            }

            VsiriusProductDto msg = await _valid.IsValid(dto);
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
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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

        public async Task<PaginationUtility<VsiriusProductDto>> GetAll(List<VsiriusFilterParamDto> filterParam, PaginationParam pagination, string fullTextSearch)
        {
            var predicate = PredicateBuilder.New<VsiriusProduct>(x =>x.IsDelete != true);

            VsiriusProductDto dto = new VsiriusProductDto();
            var propertiesValid = dto.GetType().GetProperties();

            if (filterParam.Count != 0)
            {
                // lọc list các filterParam
                var checkFilterParam = false;
                foreach (var item in filterParam.Where(x => x.Value.Count > 0))
                {
                    // lọc các filed của VsiriusProductDto
                    foreach (PropertyInfo property in propertiesValid)
                    {
                        if (property.Name == item.FieldName)
                        {
                            checkFilterParam = true;
                            var lambda = CreateFieldCheckLambda<VsiriusProduct>(property.Name, item.Value);
                            predicate = predicate.And(lambda);
                        }
                    }
                }
                if (!checkFilterParam)
                    predicate = predicate.And(x => false);
            }
            if (!string.IsNullOrEmpty(fullTextSearch))
            {
                predicate.And(x => x.item_name.Contains(fullTextSearch) || x.standard_price.Contains(fullTextSearch)
                    || x.feed_product_type.Contains(fullTextSearch) || x.item_sku.Contains(fullTextSearch)
                    || x.item_type.Contains(fullTextSearch) || x.metal_type.Contains(fullTextSearch) 
                    || x.model_name.Contains(fullTextSearch) || x.color_name.Contains(fullTextSearch)
                    || x.item_type_name.Contains(fullTextSearch));
            }

            try
            {
                var data = _vsiriusProductRepository.FindAll(predicate).ProjectTo<VsiriusProductDto>(_mapperConfiguration);
                var result = await PaginationUtility<VsiriusProductDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
                return result;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
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

                List<VsiriusFilterParamDto> result = new List<VsiriusFilterParamDto>();

                result = data.GroupBy(x => new { x.FieldName, x.LabelName, x.ProductType })
                    .Select(x => new VsiriusFilterParamDto
                    {
                        FieldName = x.Key?.FieldName,
                        LabelName = x.Key?.LabelName,
                        ProductType = x.Key?.ProductType,
                        Value = x.Select(x => x.Value).ToList(),
                        GroupName = x.Select(x => x.GroupName).FirstOrDefault()
                    }).OrderBy(x => x.ProductType).Distinct().ToList();
                return result;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new VsiriusProductDto();
            }
        }

        public async Task<OperationResult> ImportFile(IFormFile fileExcel)
        {
            //if (fileExcel != null)
            //{
            //    long fileSize = fileExcel.Length; // kích thước tệp tin tính bằng byte
            //    if (fileSize > 50000000) // kiểm tra nếu kích thước lớn hơn 50 MB
            //    {
            //        return new OperationResult(false, "File is too big");
            //    }
            //}


            string? fileExtension = Path.GetExtension(fileExcel.FileName);
            string?[] arrNameFile = new string?[] {
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

            //multi task
            var transaction = await _context.Database.BeginTransactionAsync();
            var errorString = "";
            var checkError = false;

            async Task ReadRowAsync(int rowStart, int rowEnd, IVsiriusProductRepository repo)
            {
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

                    VsiriusProductDto msg = await _valid.IsValid(obj);
                    var propertiesValid = msg.GetType().GetProperties();

                    foreach (PropertyInfo property in propertiesValid)
                    {
                        object value = property.GetValue(msg);

                        if (value != null)
                        {
                            checkError = true;
                            errorString += row + property.Name + " " + value?.ToString() + "; ";
                        }
                    }

                    var checkData = await repo.FindSingle(x => x.item_sku == obj.item_sku);
                    if (checkData != null)
                    {
                        obj.UpdatedDate = DateTime.Now;
                        obj.CreatedDate = checkData.CreatedDate;
                        repo.Update(_mapper.Map<VsiriusProduct>(obj));
                    }
                    else
                    {
                        obj.CreatedDate = DateTime.Now;
                        repo.Add(_mapper.Map<VsiriusProduct>(obj));
                    }
                }
            }

            var option = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
                .Options;

            var context1 = new DataContext(option);
            var context2 = new DataContext(option);
            var context3 = new DataContext(option);
            var context4 = new DataContext(option);
            var context5 = new DataContext(option);
            var context6 = new DataContext(option);
            var context7 = new DataContext(option);
            var context8 = new DataContext(option);
            var context9 = new DataContext(option);
            var context10 = new DataContext(option);
            var context11 = new DataContext(option);
            var context12 = new DataContext(option);
            var context13 = new DataContext(option);
            var context14 = new DataContext(option);
            var context15 = new DataContext(option);
            var repo1 = new VsiriusProductRepository(context1);
            var repo2 = new VsiriusProductRepository(context2);
            var repo3 = new VsiriusProductRepository(context3);
            var repo4 = new VsiriusProductRepository(context4);
            var repo5 = new VsiriusProductRepository(context5);
            var repo6 = new VsiriusProductRepository(context6);
            var repo7 = new VsiriusProductRepository(context7);
            var repo8 = new VsiriusProductRepository(context8);
            var repo9 = new VsiriusProductRepository(context9);
            var repo10 = new VsiriusProductRepository(context10);
            var repo11 = new VsiriusProductRepository(context11);
            var repo12 = new VsiriusProductRepository(context12);
            var repo13 = new VsiriusProductRepository(context13);
            var repo14 = new VsiriusProductRepository(context14);
            var repo15 = new VsiriusProductRepository(context15);

            var totalRow = range.End.Row;
            List<Task> listTask = new List<Task>();
            if (totalRow < 1000)
                listTask.Add(ReadRowAsync(4, totalRow +1, repo1));
            else
            {
                var step = totalRow/ 15;
                listTask.Add(ReadRowAsync(4, step*2, repo1));
                listTask.Add(ReadRowAsync(step * 2, step * 3, repo2));
                listTask.Add(ReadRowAsync(step * 3, step * 4, repo3));
                listTask.Add(ReadRowAsync(step * 4, step * 5, repo4));
                listTask.Add(ReadRowAsync(step * 5, step * 6, repo5));
                listTask.Add(ReadRowAsync(step * 6, step * 7, repo6));
                listTask.Add(ReadRowAsync(step * 7, step * 8, repo7));
                listTask.Add(ReadRowAsync(step * 8, step * 9, repo8));
                listTask.Add(ReadRowAsync(step * 9, step * 10, repo9));
                listTask.Add(ReadRowAsync(step * 10, step * 11, repo10));
                listTask.Add(ReadRowAsync(step * 11, step * 12, repo11));
                listTask.Add(ReadRowAsync(step * 12, step * 13, repo12));
                listTask.Add(ReadRowAsync(step * 13, step * 14, repo13));
                listTask.Add(ReadRowAsync(step * 14, step * 15, repo14));
                listTask.Add(ReadRowAsync(step * 15, totalRow + 1, repo15));

            }
            await Task.WhenAll(listTask);

            if (checkError)
            {
                return new OperationResult { Success = false, Message = errorString };
            }
            //end multi task

            try
            {
                List<Task> listTaskSave = new List<Task>();
                listTaskSave.Add(repo1.SaveChangesAsync());
                listTaskSave.Add(repo2.SaveChangesAsync());
                listTaskSave.Add(repo3.SaveChangesAsync());
                listTaskSave.Add(repo4.SaveChangesAsync());
                listTaskSave.Add(repo5.SaveChangesAsync());
                listTaskSave.Add(repo6.SaveChangesAsync());
                listTaskSave.Add(repo7.SaveChangesAsync());
                listTaskSave.Add(repo8.SaveChangesAsync());
                listTaskSave.Add(repo9.SaveChangesAsync());
                listTaskSave.Add(repo10.SaveChangesAsync());
                listTaskSave.Add(repo11.SaveChangesAsync());
                listTaskSave.Add(repo12.SaveChangesAsync());
                listTaskSave.Add(repo13.SaveChangesAsync());
                listTaskSave.Add(repo14.SaveChangesAsync());
                listTaskSave.Add(repo15.SaveChangesAsync());
                await Task.WhenAll(listTaskSave);
                //await _vsiriusProductRepository.SaveChangesAsync();

                await transaction.CommitAsync();
                return new OperationResult { Success = true };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> RemoveProduct(string item_sku)
        {
            var data = await _vsiriusProductRepository.FindSingle(x => x.item_sku == item_sku);
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "No Data" };
            }

            var dataHome = await _vsiriusHomeProductRepository.FindSingle(x => x.item_sku == item_sku);
            if (dataHome != null)
            {
                dataHome.IsDelete = true;
                _vsiriusHomeProductRepository.Update(dataHome);
            }
            data.IsDelete = true;
            _vsiriusProductRepository.Update(data);
            try
            {
                await _vsiriusProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Successful delete!!!" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> UpdateProduct(VsiriusProductDto dto)
        {
            var data = await _vsiriusProductRepository.FindSingle(x => x.item_sku == dto.item_sku);
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "No Data" };
            }

            VsiriusProductDto msg = await _valid.IsValid(dto);
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
            _vsiriusProductRepository.Update(dataUpdate);
            try
            {
                await _vsiriusProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Successful update!!!" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
                return await PaginationUtility<VsiriusProductDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
                var data = await _vsiriusValidProductRepository.FindAll(predicate).ToListAsync();
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
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
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
                return null;
            if (item_sku_category.ToUpper() == "Q")
                predicate.And(x => x.item_sku.Contains("Q-"));
            if (item_sku_category.ToUpper() == "S")
                predicate.And(x => x.item_sku.Contains("S-"));
            var data = await _vsiriusProductRepository.FindAll(predicate).Select(x => x.item_sku).ToListAsync();
            return data;
        }

        public async Task<OperationResult> RemoveMultipleProduct(List<string> listSKU)
        {
            try
            {
                foreach (var item in listSKU)
                {
                    var data = await _vsiriusProductRepository.FindSingle(x => x.item_sku == item);
                    data.IsDelete = true;
                    _vsiriusProductRepository.Update(data);
                }
                await _vsiriusProductRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Successful delete!!!" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
