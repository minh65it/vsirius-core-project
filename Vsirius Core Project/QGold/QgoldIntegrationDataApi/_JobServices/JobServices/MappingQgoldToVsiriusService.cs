using AutoMapper;
using Azure;
using Newtonsoft.Json;
using OfficeOpenXml;
using QgoldIntegrationDataApi._JobServices.Interfaces;
using QgoldIntegrationDataApi._Services.Interfaces;
using QgoldIntegrationDataApi.Dtos.Marketplace;
using QgoldIntegrationDataApi.Dtos.ProductAI;
using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Helpers;
using QgoldIntegrationDataApi.Helpers.AutoMapper;
using QgoldIntegrationDataApi.Helpers.Ultilities;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace QgoldIntegrationDataApi._JobServices.JobServices
{
    public class MappingQgoldToVsiriusService : IMappingQgoldToVsiriusService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IQgoldCsvDataService _serviceAPI;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly PimApi._Services.Interfaces.IProductService _servicePim;

        public MappingQgoldToVsiriusService(
            IQgoldCsvDataService serviceAPI,
            IConfiguration configuration,
            IMapper mapper,
            IServiceProvider serviceProvider,
            PimApi._Services.Interfaces.IProductService servicePim)
        {
            _mapper = mapper;
            _serviceAPI = serviceAPI;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            this._servicePim = servicePim;
        }

        public async Task MappingQgoldToVsirius()
        {
            Common.WriteLog("----------------------------- Start Mapper Qgold to Vsirius Process! ------------------------------------------------");
            try
            {
                Common.WriteLog($"Map data Qgold to Web");

                //List<ProductAiDto> lstProductAI = await GetProductAI();

                IEnumerable<QgoldCsvData_DTO> listQgold = await GetListQgoldApi();

                List<VsiriusProduct_DTO3> listVsirius = new();

                if (listQgold != null)
                {
                    Parallel.ForEach(listQgold, qgold =>
                    {
                        try
                        {
                            var vsirius = CustomMapper.MapperQgoldToVsiriusV3(qgold);
                            listVsirius.Add(vsirius);
                        }
                        catch (Exception)
                        {

                        }
                    });

                    //foreach (var qgold in listQgold)
                    //{
                    //    var vsirius = CustomMapper.MapperQgoldToVsiriusV3(qgold);

                    //    //var aiProduct = lstProductAI.FirstOrDefault(x => x.SKU == qgold.Item && x.IsReview == true);
                    //    //if (aiProduct != null)
                    //    //{
                    //    //    vsirius.item_name = aiProduct.ProductName;
                    //    //    vsirius.generic_keywords = aiProduct.Keywords;
                    //    //    vsirius.bullet_point1 = aiProduct.BulletPoint1;
                    //    //    vsirius.bullet_point2 = aiProduct.BulletPoint2;
                    //    //    vsirius.bullet_point3 = aiProduct.BulletPoint3;
                    //    //    vsirius.bullet_point4 = aiProduct.BulletPoint4;
                    //    //    vsirius.bullet_point5 = aiProduct.BulletPoint5;
                    //    //    vsirius.bullet_point6 = aiProduct.BulletPoint6;
                    //    //    vsirius.bullet_point7 = aiProduct.BulletPoint7;
                    //    //    vsirius.bullet_point8 = aiProduct.BulletPoint8;
                    //    //    vsirius.bullet_point9 = aiProduct.BulletPoint9;
                    //    //    vsirius.brand_name = aiProduct.Brand;
                    //    //}

                    //}

                    //Export workbook
                    //List<ExcelInfo> lstWorkbook = ExportListExcelQgoldToVsirius(listVsirius);
                    ExportListExcelQgoldToVsirius(listVsirius);
                    Common.WriteLog("Export data done!");
                    //Call api import data Vsirius
                    List<string> listSkuVsirius = await GetItemSkuVsirius_RFT();

                    //var checkImportExcelApi = await callApiVsiriusWeb.ImportExcelApi(workbookQgolds);
                    Common.WriteLog("Start Import Data to Vsirius");
                    bool checkImportExcelApi = await ImportExcelApi_RFT();
                    Common.WriteLog("End Import Data to Vsirius");

                    Common.WriteLog("Start Delete Item Vsirius");
                    await DeleteItemVsirius_RFT(listSkuVsirius, listVsirius);
                    Common.WriteLog("End Delete Item Vsirius");
                }

                //lstProductAI.Clear();
                listQgold = null;
                listVsirius.Clear();
            }
            catch (Exception ex)
            {
                Common.WriteLog($"MappingQgoldToVsirius error: {ex.Message}");
            }
            Common.WriteLog("----------------------------- End Mapper Qgold to Vsirius Process! ------------------------------------------------");
        }

        private async Task<List<ProductAiDto>> GetProductAI()
        {
            var lstAllSkuProductAI = new List<ProductAiDto>();

            try
            {
                var pageSize = 3000;
                PaginationUtility<ProductAiDto> dataFirstCall = await GetPaginationResultAsync(1, pageSize);
                var totalPages = 0;
                List<ProductAiDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    lstAllSkuProductAI.AddRange(firstData!);
                    Common.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtility<ProductAiDto>>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationResultAsync(pageNumber, pageSize));

                    if (amountRunTask < 3 && i != totalPages)
                    {
                        amountRunTask++;
                    }
                    else
                    {
                        amountRunTask = 0;
                        var results = await Task.WhenAll(tasks);
                        foreach (var result in results)
                        {
                            if (result != null)
                            {
                                lstAllSkuProductAI.AddRange(result.Result);
                            }
                            else
                            {
                                Common.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.WriteLog("Get All ProductAI failed. Err:" + ex.Message);
            }

            return lstAllSkuProductAI;
        }

        private async Task<PaginationUtility<ProductAiDto>> GetPaginationResultAsync(int pageNumber, int pageSize)
        {
            string url = _configuration.GetValue<string>("ProductAiApi:GetAll")!;

            PaginationUtility<ProductAiDto> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<ProductAiDto>>(json);
                    Common.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    Common.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Common.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        private async Task<bool> DeleteItemVsirius(List<string> listSkuVsirius, List<VsiriusProduct_DTO3> listVsirius)
        {
            try
            {
                var skuNotInDto = listSkuVsirius.Except(listVsirius.Select(x => x.item_sku)).ToList();

                if (skuNotInDto.Count > 0)
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string token = await LoginAuth();
                        if (string.IsNullOrWhiteSpace(token))
                        {
                            return false;
                        }

                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                        client.Timeout = TimeSpan.FromHours(5);
                        var json = JsonConvert.SerializeObject(skuNotInDto);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage response = await client
                            .PutAsync($"{_configuration.GetSection("ApiPIM:removeMultipleProduct").Value}", content);

                        Common.WriteLog($"Result when DeleteItemVsirius: {await response.Content.ReadAsStringAsync()}");

                        if (response.IsSuccessStatusCode)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not DeleteItem Web " + e.Message);
                return false;
            }
        }

        private async Task<bool> DeleteItemVsirius_RFT(List<string> listSkuVsirius, List<VsiriusProduct_DTO3> listVsirius)
        {
            try
            {
                var skuNotInDto = listSkuVsirius.Except(listVsirius.Select(x => x.item_sku)).ToList();
                var rs = await _servicePim.RemoveMultipleProduct(skuNotInDto);
                Common.WriteLog($"Result when DeleteItemVsirius: " + rs.Message);
                if (rs.Success)
                {
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<bool> ImportExcelApi()
        {
            try
            {
                string token = await LoginAuth();
                if (string.IsNullOrWhiteSpace(token))
                {
                    Common.WriteLog("Login to Vsirius failed");
                    return false;
                }
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var _exportPathVsirius = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Storage",
                        "ExportExcel",
                        $"DataQgoldtoVsirius_{DateTime.Now:ddMMyyyy}");
                Common.CreateFolder(_exportPathVsirius);
                List<FileInfo> fileInfos = new DirectoryInfo(_exportPathVsirius).GetFiles()
                .Where(f => Regex.IsMatch(f.Name, @"^.+\.(xlsx|xls|csv)$")).OrderBy(f => f.CreationTime).ToList();

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    httpClient.Timeout = TimeSpan.FromHours(5);

                    foreach (var file in fileInfos)
                    {
                        using (var stream = new MemoryStream())
                        {
                            using (FileStream fileStream = file.OpenRead())
                            {
                                await fileStream.CopyToAsync(stream);
                            }
                            stream.Seek(0, SeekOrigin.Begin);

                            using (var content = new MultipartFormDataContent())
                            {
                                using (var fileContent = new StreamContent(stream))
                                {
                                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");

                                    content.Add(fileContent, "fileExcel", file.Name);

                                    var response = await httpClient.PostAsync($"{_configuration.GetSection("ApiPIM:importExcelApi").Value}", content);
                                    var responseData = await response.Content.ReadAsStringAsync();
                                    Common.WriteLog($"API request. Response from import file {file.Name}:" + responseData);

                                    if (!response.IsSuccessStatusCode)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not ImportExcel to Web " + e.Message);
                return false;
            }
        }

        private async Task<bool> ImportExcelApi_RFT()
        {
            try
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var _exportPathVsirius = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Storage",
                        "ExportExcel",
                        $"DataQgoldtoVsirius_{DateTime.Now:ddMMyyyy}");
                Common.CreateFolder(_exportPathVsirius);
                List<FileInfo> fileInfos = new DirectoryInfo(_exportPathVsirius).GetFiles()
                .Where(f => Regex.IsMatch(f.Name, @"^.+\.(xlsx|xls|csv)$")).OrderBy(f => f.CreationTime).ToList();

                foreach (var file in fileInfos)
                {
                    IFormFile formFile = CreateFormFileFromSystemFile(file);
                    var rs = await _servicePim.ImportFile(formFile);
                    Common.WriteLog($"API request. Response from import file {file.Name}:" + rs.Message);

                    if (!rs.Success)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not ImportExcel to Web " + e.Message);
                return false;
            }
        }
        private async Task<List<string>> GetItemSkuVsirius()
        {
            try
            {
                string token = await LoginAuth();
                if (string.IsNullOrWhiteSpace(token))
                {
                    return new List<string>();
                }
                var url = $"{_configuration.GetSection("ApiPIM:getItemSku").Value}?item_sku_category=q";
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    httpClient.Timeout = TimeSpan.FromHours(5);

                    HttpResponseMessage response = await httpClient.GetAsync(url);
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<string>>(responseBody);
                }

            }
            catch (Exception e)
            {
                Common.WriteLog("Could not get item SKU " + e.Message);
                return new List<string>();
            }
        }

        private async Task<List<string>> GetItemSkuVsirius_RFT()
        {
            try
            {
                var data = await _servicePim.GetAllItemSKU("q");
                return data;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not get item SKU " + e.Message);
                return new List<string>();
            }
            
        }

        private async Task<string> LoginAuth()
        {
            using (var httpClient = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "email", _configuration.GetSection("IdentityApi:email").Value },
                    { "password", _configuration.GetSection("IdentityApi:password").Value }
                };

                var json = JsonConvert.SerializeObject(parameters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient
                    .PostAsync($"{_configuration.GetSection("IdentityApi:loginAuthApi").Value}", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    dynamic token = JsonConvert.DeserializeObject(result);
                    return token.token;
                }
                return null;
            }
        }

        private List<ExcelInfo> ExportListExcelQgoldToVsirius(List<VsiriusProduct_DTO3> listVsirius)
        {
            try
            {
                List<ExcelInfo> packages = new();
                int batchSize = 10000;

                int totalData = listVsirius.Count;
                int totalWorkbooks = (int)Math.Ceiling((double)totalData / batchSize);

                var _filePathOriginAmazon = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "ExportExcel",
                    "FileExcel",
                    "OriginAmazon",
                    "AmazonOrigin.xlsx"
                    );

                for (int i = 0; i < totalWorkbooks; i++)
                {
                    ExcelPackage package = new(new FileInfo(_filePathOriginAmazon));
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                    int rowIndex = 4;
                    int columnIndex = 0;

                    int startIndex = i * batchSize;
                    int endIndex = Math.Min(startIndex + batchSize, totalData);

                    for (int j = startIndex; j < endIndex; j++)
                    {
                        var data = listVsirius[j];
                        columnIndex = 1;

                        foreach (var property in typeof(VsiriusProduct_DTO3).GetProperties())
                        {
                            var value = property.GetValue(data, null);
                            worksheet.Cells[rowIndex, columnIndex].Value = value;
                            columnIndex++;
                        }
                        rowIndex++;
                    }
                    var _exportPathVsirius = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Storage",
                        "ExportExcel",
                        $"DataQgoldtoVsirius_{DateTime.Now:ddMMyyyy}");
                    Common.CreateFolder(_exportPathVsirius);
                    var exportPaths = Path.Combine(_exportPathVsirius, $"ExportFile_{i}.csv");
                    package.SaveAs(new FileInfo(exportPaths));
                    packages.Add(new ExcelInfo() { ExcelPackage = package, FileName = Path.GetFileNameWithoutExtension(exportPaths) });

                }
                return packages;
            }
            catch (Exception ex)
            {
                Common.WriteLog(ex.Message);
                return null;
            }
        }

        //Extension Function
        private async Task<IEnumerable<QgoldCsvData_DTO>> GetListQgoldApi1()
        {
            try
            {
                List<QgoldCsvData_DTO> listQgoldTotal = new List<QgoldCsvData_DTO>();
                int totalPage = 0;
                int pageNumber = 1;
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<IQgoldCsvDataService>();
                    var firstGetData = await service.GetData(
                    new PaginationParam() { PageNumber = 1, PageSize = 3000 },
                    null
                    );

                    List<QgoldCsvData_DTO> listQgold = firstGetData.Result;
                    listQgoldTotal.AddRange(listQgold);
                    totalPage = firstGetData.Pagination.TotalPage;
                    pageNumber = firstGetData.Pagination.PageNumber;
                }

                for (int i = 2; i <= totalPage; i++)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var service = scope.ServiceProvider.GetService<IQgoldCsvDataService>();
                        var result = await service.GetData(
                        new PaginationParam() { PageNumber = i, PageSize = 3000 },
                        null
                        );

                        listQgoldTotal.AddRange(result.Result);
                    }
                }

                return listQgoldTotal;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not GetListQgoldApi. Err: " + e.Message);
                return new List<QgoldCsvData_DTO>();
            }
        }

        private async Task<IEnumerable<QgoldCsvData_DTO>> GetListQgoldApi()
        {
            var lstData = new List<QgoldCsvData_DTO>();
            try
            {
                var pageSize = 5000;
                var dataFirstCall = await GetPaginationLocalResult(1, pageSize);
                var totalPages = 0;
                List<QgoldCsvData_DTO> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    lstData.AddRange(firstData!);
                    Common.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return lstData;
                }

                //List rest data
                List<Task<PaginationUtility<QgoldCsvData_DTO>?>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationLocalResult(pageNumber, pageSize));

                    if (amountRunTask < 5 && i != totalPages)
                    {
                        amountRunTask++;
                    }
                    else
                    {
                        amountRunTask = 0;
                        var results = await Task.WhenAll(tasks);
                        foreach (var result in results)
                        {
                            if (result != null)
                            {
                                lstData.AddRange(result.Result);
                            }
                            else
                            {
                                Common.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }

            }
            catch (Exception ex)
            {
                Common.WriteLog($"GetDataFromLocal. Error :{ex.Message}");
            }
            return lstData;
        }

        private async Task<PaginationUtility<QgoldCsvData_DTO>> GetPaginationLocalResult(int pageNumber, int pageSize)
        {
            PaginationUtility<QgoldCsvData_DTO> result = null;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<IQgoldCsvDataService>();
                    result = await service.GetData( new PaginationParam()
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }, null);
                }
            }
            catch (Exception ex)
            {
                Common.WriteLog($"Get product walmart. Exception: {ex.Message}");
                throw;
            }

            return result;
        }

        public IFormFile CreateFormFileFromSystemFile(FileInfo fileInfo)
        {
            var fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);

            return new FormFile(fileStream, 0, fileStream.Length, "file", fileInfo.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream"
            };
        }
    }
}
