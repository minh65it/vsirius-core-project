using AutoMapper;
using AutoMapper.QueryableExtensions;
using Korzh.EasyQuery.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ProductApi._Repositories.Interfaces;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using ProductApi.Models;

using System.Text.RegularExpressions;

namespace ProductApi._Services.Services
{
    public class VsiriusFilterService : IVsiriusFilterService
    {
        private readonly IVsiriusFilterRepository _vsiriusFilterRepository;
        private readonly IVsiriusValidProductRepository _vsiriusValidProductRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private ILogService _logService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VsiriusFilterService(IVsiriusFilterRepository vsiriusFilterRepository, IMapper mapper,
            MapperConfiguration mapperConfiguration, ILogService logService, IWebHostEnvironment hostingEnvironment,
            IVsiriusValidProductRepository vsiriusValidProductRepository, IHttpContextAccessor httpContextAccessor)
        {
            _vsiriusFilterRepository = vsiriusFilterRepository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _logService = logService;
            _hostingEnvironment = hostingEnvironment;
            _vsiriusValidProductRepository = vsiriusValidProductRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PaginationUtility<VsiriusFilterDto>> GetListFilter(PaginationParam pagination, string? fullTextSearch, bool isPaging = true)
        {
            var predicate = PredicateBuilder.New<VsiriusFilter>(x => x.IsDelete == false);
            try
            {
                var data = _vsiriusFilterRepository.FindAll(predicate)
                                               .FullTextSearchQuery(fullTextSearch)
                                               .ProjectTo<VsiriusFilterDto>(_mapperConfiguration).AsNoTracking();

                return await PaginationUtility<VsiriusFilterDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
            
        }

        public async Task<VsiriusFilterDto> GetFilter(int filterId)
        {
            try
            {
                var data = await _vsiriusFilterRepository.FindSingle(x => x.FilterId == filterId);
                return _mapper.Map<VsiriusFilterDto>(data);
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new VsiriusFilterDto();
            }
            
        }

        public async Task<OperationResult> GetFilterByProductType()
        {
            try
            {
                var data = await _vsiriusFilterRepository.FindAll(x => x.FieldName == "feed_product_type" &&
                                                                       x.IsDelete == false &&
                                                                       x.IsActive == true).OrderBy(x => x.OrderId == null ? int.MaxValue : x.OrderId)
                                                                       .ThenBy(x => x.OrderId).ToListAsync();

                var result = _mapper.Map<List<VsiriusFilterDto>>(data);
                return new OperationResult { Success = true, Data = result };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> AddFilterByExcel(IFormFile fileExcel)
        {
            string extension = Path.GetExtension(fileExcel.FileName);
            if (extension != ".xlsx" && extension != ".xls" && extension != ".xlsm" && extension != ".csv")
            {
                return new OperationResult(false, "Not file excel");
            }

            List<VsiriusFilterDto> filterDtos = new List<VsiriusFilterDto>();

            MemoryStream stream = new MemoryStream();
            fileExcel.CopyTo(stream);
            ExcelPackage package = new ExcelPackage(stream);

            ExcelWorkbook workbook = package.Workbook;
            if (workbook == null)
            {
                return new OperationResult(false, "Not file excel");
            }

            ExcelWorksheet worksheetDataDefinitions = package.Workbook.Worksheets["Data Definitions"];
            ExcelWorksheet worksheetValidValues = package.Workbook.Worksheets["Valid Values"];

            int rowCountDataDefinitions = worksheetDataDefinitions.Dimension.End.Row;
            int rowCountValidValues = worksheetValidValues.Dimension.End.Row;
            int columnCountValidValues = worksheetValidValues.Dimension.End.Column;

            #region Get Value Sheet Data Definitions
            string groupName = "";
            List<VsiriusFilterDto> listFilterDataDefinitions = new List<VsiriusFilterDto>();
            for (int row = 3; row <= rowCountDataDefinitions; row++)
            {
                VsiriusFilterDto filterDto = new VsiriusFilterDto();
                var valueGroupName = worksheetDataDefinitions.Cells[row, 1].Value?.ToString();
                var valueFieldName = worksheetDataDefinitions.Cells[row, 2].Value?.ToString();
                var valueLableName = worksheetDataDefinitions.Cells[row, 3].Value?.ToString();

                if (valueGroupName == null)
                {
                    valueGroupName = groupName;
                }
                else
                {
                    if (valueGroupName.Contains("-"))
                    {
                        string[] groupNameSplit = valueGroupName.Split("-");
                        groupName = groupNameSplit[0];
                    }
                    else
                    {
                        groupName = valueGroupName;
                    }
                }

                if (valueFieldName != null)
                {
                    if (valueFieldName.Contains("-"))
                    {
                        string[] valueSplit = valueFieldName.Split("-");
                        int numbersSplit = int.Parse(Regex.Replace(valueSplit[1], "[^0-9]", ""));
                        for (int i = 1; i <= numbersSplit; i++)
                        {
                            filterDto = new VsiriusFilterDto();
                            string stringSplit = Regex.Replace(valueSplit[0], "[^a-zA-Z_]", "");
                            stringSplit = stringSplit + i;

                            filterDto.GroupName = valueGroupName;
                            filterDto.FieldName = stringSplit;
                            filterDto.LabelName = valueLableName;
                            listFilterDataDefinitions.Add(filterDto);
                        }
                    }
                    else
                    {
                        filterDto.GroupName = valueGroupName;
                        filterDto.FieldName = valueFieldName;
                        filterDto.LabelName = valueLableName;
                        listFilterDataDefinitions.Add(filterDto);
                    }
                }
            }
            #endregion

            string groupNameValid = "";

            #region Get Value Sheet Valid Values
            for (int row = 1; row <= rowCountValidValues; row++)
            {
                List<string> rowData = new List<string>();
                for (int column = 1; column <= columnCountValidValues; column++)
                {
                    var value = worksheetValidValues.Cells[row, column].Value?.ToString();
                    if (value != null) rowData.Add(value);
                }

                if (rowData.Count == 1)
                {
                    groupNameValid = rowData[0].ToString();
                }

                for (int i = 1; i < rowData.Count; i++)
                {
                    VsiriusFilterDto filterDto = new VsiriusFilterDto();
                    string labelName = "";
                    string itemType = "";

                    if (rowData[0].Contains("-"))
                    {
                        string[] labelNameSplit = rowData[0].Split(" -");
                        labelName = labelNameSplit[0];

                        var itemTypeReplace = labelNameSplit[1].Replace(" [ ", "").Replace(" ]", "");
                        if (!string.IsNullOrEmpty(itemTypeReplace))
                        {
                            itemType = itemTypeReplace;
                        }
                    }
                    else
                    {
                        labelName = rowData[0];
                    }

                    var listLabelName = listFilterDataDefinitions.Where(x => x.LabelName == labelName &&
                                                                             x.GroupName.Contains(groupNameValid)).ToList();

                    if (listLabelName.Count == 1)
                    {
                        filterDto.LabelName = labelName;
                        filterDto.ProductType = itemType;
                        filterDto.FieldName = listLabelName[0].FieldName;
                        filterDto.GroupName = groupNameValid;
                        filterDto.Value = rowData[i];
                        filterDto.CreatedDate = DateTime.Now;
                        filterDto.IsDelete = false;
                        filterDto.CreatedBy = "Admin";
                        filterDto.UpdatedDate = DateTime.Now;
                        if(filterDto.FieldName == "feed_product_type" ||
                            filterDto.FieldName == "item_type" ||
                            filterDto.FieldName == "metal_type")
                            filterDto.IsActive = true;
                        else 
                            filterDto.IsActive = false;

                        filterDtos.Add(filterDto);
                    }
                    else
                    {
                        for (int f = 0; f < listLabelName.Count; f++)
                        {
                            filterDto = new VsiriusFilterDto()
                            {
                                LabelName = labelName,
                                ProductType = itemType,
                                FieldName = listLabelName[f].FieldName,
                                GroupName = groupNameValid,
                                Value = rowData[i],
                                CreatedDate = DateTime.Now,
                                IsDelete = false,
                                CreatedBy = "Admin",
                                UpdatedDate = DateTime.Now,
                                IsActive = false,
                            };
                            if (filterDto.FieldName == "feed_product_type" ||
                                filterDto.FieldName == "item_type" ||
                                filterDto.FieldName == "metal_type")
                                filterDto.IsActive = true;
                            else
                                filterDto.IsActive = false;

                            filterDtos.Add(filterDto);
                        }
                    }
                }
            }
            #endregion

            try
            {
                _vsiriusFilterRepository.RemoveMultiple( await _vsiriusFilterRepository.FindAll().ToListAsync());
                _vsiriusValidProductRepository.RemoveMultiple(await _vsiriusValidProductRepository.FindAll().ToListAsync());

                _vsiriusFilterRepository.AddMultiple(_mapper.Map<List<VsiriusFilter>>(filterDtos));
                _vsiriusValidProductRepository.AddMultiple(_mapper.Map<List<VsiriusValidProduct>>(filterDtos));
                await _vsiriusFilterRepository.SaveChangesAsync();

                return new OperationResult { Success = true, Message = "Add Data Filter Successfully" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> AddFilter(VsiriusFilterDto dto)
        {
            if (dto.FileImage == null && dto.FieldName == "feed_product_type")
            {
                return new OperationResult { Success = false, Message = "No file uploaded." };
            }

            var result = _mapper.Map<VsiriusFilter>(dto);
            if (dto.FileImage != null)
            {
                string extension = Path.GetExtension(dto.FileImage.FileName);
                string?[] arrNameFile = new string?[] {
                    ".png", ".jpg",".jpeg", ".svg"
                };
                if (!arrNameFile.Contains(extension.ToLower()))
                {
                    return new OperationResult(false, "Not file image");
                }
                result.ImageURL = ReadFileImage(dto.FileImage, dto.FieldName, dto.Value);
            }

            result.IsDelete = false;
            result.IsActive = false;
            result.CreatedDate = DateTime.Now;
            result.UpdatedDate = null;
            try
            {
                _vsiriusFilterRepository.Add(result);
                await _vsiriusFilterRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Add Data Filter Successfully" };
            }
            catch(Exception ex) 
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message=ex.Message };
            }
        }

        public async Task<OperationResult> UpdateFilter(VsiriusFilterDto dto)
        {
            if (dto.FileImage == null && dto.FieldName == "feed_product_type")
            {
                return new OperationResult { Success = false, Message = "No file uploaded." };
            }

            var data = await _vsiriusFilterRepository.FindSingle(x => x.FilterId == dto.FilterId);
            if (data == null) return new OperationResult { Success = false, Message = "No Data" };
            DeleteImageFile(data.ImageURL);

            var result = _mapper.Map<VsiriusFilter>(dto);
            if(dto.FileImage != null)
            {
                string extension = Path.GetExtension(dto.FileImage.FileName);
                string?[] arrNameFile = new string?[] {
                    ".png", ".jpg",".jpeg", ".svg"
                };
                if (!arrNameFile.Contains(extension.ToLower()))
                {
                    return new OperationResult(false, "Not file image");
                }
                result.ImageURL = ReadFileImage(dto.FileImage, dto.FieldName, dto.Value);
            }
            result.UpdatedDate = DateTime.Now;

            try
            {
                _vsiriusFilterRepository.Update(result);
                await _vsiriusFilterRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Update Data Successfully" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> DeleteFilter(int filterId)
        {
            var data = await _vsiriusFilterRepository.FindSingle(x => x.FilterId == filterId);
            if (data == null) return new OperationResult { Success = false, Message = "No Data" };

            try
            {
                var result = _mapper.Map<VsiriusFilter>(data);
                result.IsDelete = true;
                DeleteImageFile(data.ImageURL);
                _vsiriusFilterRepository.Update(result);
                await _vsiriusFilterRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Delete Data Successfully" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        private string ReadFileImage(IFormFile image, string fieldName, string value)
        {
            string wwwRootPath = _hostingEnvironment.WebRootPath;

            string fileName = "";
            string contentType = image.ContentType;
            if (!string.IsNullOrEmpty(contentType))
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images/Categories");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string[] extension = contentType.Split("/");
                if (string.IsNullOrEmpty(value))
                {
                    fileName = "//Images" + "//Categories" + "//" + fieldName + "." + extension[1];
                }
                else
                {
                    fileName = "//Images" + "//Categories" + "//" + fieldName + "_" + value + "." + extension[1];
                }

                string filePath = Path.Combine(wwwRootPath + fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }

                return fileName;
            }
            return string.Empty;
        }

        public void DeleteImageFile(string filePath)
        {
            string wwwRootPath = _hostingEnvironment.WebRootPath;
            filePath = Path.Combine(wwwRootPath + filePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
