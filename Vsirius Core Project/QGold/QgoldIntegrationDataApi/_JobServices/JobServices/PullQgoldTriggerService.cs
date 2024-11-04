using AutoMapper;
using CsvHelper.Configuration;
using CsvHelper;
using QgoldIntegrationDataApi._JobServices.Interfaces;
using QgoldIntegrationDataApi._Services.Interfaces;
using QgoldIntegrationDataApi.Helpers;
using QgoldIntegrationDataApi.Helpers.AutoMapper;
using QgoldIntegrationDataApi.Models;
using System.Globalization;
using System.Net;
using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Helpers.Ultilities;
using Newtonsoft.Json;
using QgoldIntegrationDataApi.Dtos.ProductAI;
using QgoldIntegrationDataApi.Dtos.Order;

namespace QgoldIntegrationDataApi._JobServices.JobServices
{
    public class PullQgoldTriggerService : IPullQgoldTriggerService
    {

        private const string _userid = "58772";
        private const string _password = "DaTa4VSiRiUs";
        private const string _fileUrl = @"ftp://ftp.qgold.com/58772_Extract.csv";
        private readonly IQgoldCsvDataService _serviceAPI;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IConfiguration _skuConfig;
        public PullQgoldTriggerService(
            IQgoldCsvDataService serviceAPI,
            IMapper mapper,
            IConfiguration config)
        {
            _mapper = mapper;
            _serviceAPI = serviceAPI;
            _config = config;

            IConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory
                , "Storage"
                , "skugenerate.json"));

            _skuConfig = builder.Build();
        }

        public async Task PullQgold()
        {
            Common.WriteLog("----------------------------- Start Pull Data Qgold Process! ------------------------------------------------");
            try
            {
                Common.WriteLog($"Start run pull data Qgold");
                List<QgoldProduct> dataQgold = DownloadCsvAndConvertToObject();
                Common.WriteLog("End pull data qgold success!");

                //await PushToProductAI(dataQgold);
                await CreateUpdateToQgoldDb(dataQgold);
            }
            catch (Exception ex)
            {
                Common.WriteLog(ex.Message);
            }
            Common.WriteLog("----------------------------- Done Pull Data Qgold ! ------------------------------------------------");
        }

        private async Task PushToProductAI(List<QgoldProduct> dataQgold)
        {
            var lstAllSkuProductAI = await GetAllSkuProductAI();
            var i = 1;

            var lstSkuInConfig = _skuConfig.GetValue<string>("sku").Split('|').Distinct().ToList();
            List<string> lstSkuOrderAmazon = await GetSkuSellerInAmazon();
            List<string> lstSkuOrderWalmart = await GetSkuSellerInWalmart();
            lstSkuInConfig.AddRange(lstSkuOrderAmazon);
            lstSkuInConfig.AddRange(lstSkuOrderWalmart);

            lstSkuInConfig = lstSkuInConfig.Distinct().ToList();

            List<SkuPriceItem> lstSkuPrice = (from qgold in dataQgold
                                              join lstSku in lstSkuInConfig on qgold.Item equals lstSku
                                              select new SkuPriceItem()
                                              {
                                                  Sku = lstSku,
                                                  Price = double.Parse(qgold.Price)
                                              })
                                              .OrderBy(x => x.Price >= 40 && x.Price <= 2000)
                                              .ThenBy(x => x.Price).ToList();

            foreach (var sku in lstSkuPrice)
            {
                if (lstAllSkuProductAI.Any(x => x.Equals(sku.Sku)))
                {
                    continue;
                }
                else
                {
                    var item = dataQgold.FirstOrDefault(x => x.Item.Equals(sku.Sku));

                    if (item != null)
                    {
                        if (item.ListOfSpecsText.Contains("Licensed By", StringComparison.OrdinalIgnoreCase) || item.Qty_Avail == "0")
                        {
                            continue;
                        }
                        if (i < 20)
                        {
                            i++;
                        }
                        else
                        {
                            break;
                        }
                        bool IsSuccessInsert = await CreateProductAI(item);
                        if (!IsSuccessInsert)
                        {
                            break;
                        }
                        else
                        {
                            lstAllSkuProductAI.Add(sku.Sku);
                        }
                    }
                }
            }
            dataQgold = dataQgold
                .OrderBy(x => double.Parse(x.Price) >= 40 && double.Parse(x.Price) <= 2000)
                .ThenBy(x => x.Price).ToList();
            foreach (var item in dataQgold)
            {
                if (item.ListOfSpecsText.Contains("Licensed By", StringComparison.OrdinalIgnoreCase) || item.Qty_Avail == "0")
                {
                    continue;
                }
                if (lstAllSkuProductAI.Any(x => x.Equals(item.Item)))
                {
                    continue;
                }
                else
                {
                    if (i < 20)
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                    bool IsSuccessInsert = await CreateProductAI(item);
                    if (!IsSuccessInsert) break;
                }
            }

        }

        private async Task<List<string>> GetSkuSellerInWalmart()
        {
            var lstAllSku = new List<OrderWalmartDto>();
            try
            {
                var pageSize = 3000;
                PaginationUtility<OrderWalmartDto> dataFirstCall = await GetPaginationResulSkuSellerInWalmartAsync(1, pageSize);
                var totalPages = 0;
                List<OrderWalmartDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    lstAllSku.AddRange(firstData!);
                    Common.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return new List<string>();
                }

                //List rest data
                List<Task<PaginationUtility<OrderWalmartDto>>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationResulSkuSellerInWalmartAsync(pageNumber, pageSize));

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
                                lstAllSku.AddRange(result.Result);
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
                Common.WriteLog("GetAllSkuProductAI failed. Err:" + ex.Message);
            }

            List<string> uniqueSellerSKUs = lstAllSku
            .SelectMany(order => order.items.Select(item => Common.SKUQgold(item.sku)))
            .Distinct()
            .ToList();
            return uniqueSellerSKUs;
        }

        private async Task<PaginationUtility<OrderWalmartDto>> GetPaginationResulSkuSellerInWalmartAsync(int pageNumber, int pageSize)
        {
            string url = _config.GetValue<string>("WalmartApi:GetOrders")!;

            PaginationUtility<OrderWalmartDto> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<OrderWalmartDto>>(json);
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

        private async Task<List<string>> GetSkuSellerInAmazon()
        {
            var lstAllSku = new List<OrderAmazonDto>();
            try
            {
                var pageSize = 3000;
                PaginationUtility1<OrderAmazonDto> dataFirstCall = await GetPaginationResulSkuSellerInAmazontAsync(1, pageSize);
                var totalPages = 0;
                List<OrderAmazonDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Data;
                    totalPages = dataFirstCall.PageCount;
                    lstAllSku.AddRange(firstData);
                    Common.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return new List<string>();
                }

                //List rest data
                List<Task<PaginationUtility1<OrderAmazonDto>>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationResulSkuSellerInAmazontAsync(pageNumber, pageSize));

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
                                lstAllSku.AddRange(result.Data);
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
                Common.WriteLog("GetAllSkuProductAI failed. Err:" + ex.Message);
            }
            List<string> uniqueSellerSKUs = lstAllSku
            .SelectMany(order => order.orderItemList.Select(item => item.sellerSKU))
            .Distinct()
            .ToList();
            return uniqueSellerSKUs;
        }

        private async Task<PaginationUtility1<OrderAmazonDto>> GetPaginationResulSkuSellerInAmazontAsync(int pageNumber, int pageSize)
        {
            string url = _config.GetValue<string>("AmazonApi:GetOrders")!;

            PaginationUtility1<OrderAmazonDto> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility1<OrderAmazonDto>>(json);
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

        private async Task<bool> CreateProductAI(QgoldProduct item)
        {
            var param = new CreateUpdateProductAiParam()
            {
                Brand = "Vsirius",
                Description = item.ListOfSpecsText,
                SKU = item.Item,
                ImgUrl = item.Image1Link
            };

            try
            {
                var url = _config.GetValue<string>("ProductAiApi:Create");
                var httpClient = new HttpClient();
                var jsonContent = JsonContent.Create(param);
                HttpResponseMessage response = await httpClient.PostAsync(url, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OperationResult1<ProductAiDto>>(json);
                    return result.Success;
                }
                else
                {
                    Common.WriteLog($"Call Api failed: {url} with error internal error: {response.StatusCode}");
                    return false;
                }

            }
            catch (Exception ex)
            {
                Common.WriteLog("CreateProductAI failed. Exception: " + ex.Message);
                return false;
            }
        }

        private async Task<List<string>> GetAllSkuProductAI()
        {
            var lstAllSkuProductAI = new List<string>();

            try
            {
                var pageSize = 3000;
                PaginationUtility<string> dataFirstCall = await GetPaginationResultAsync(1, pageSize);
                var totalPages = 0;
                List<string> firstData = new();
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
                List<Task<PaginationUtility<string>>> tasks = new();
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
                Common.WriteLog("GetAllSkuProductAI failed. Err:" + ex.Message);
            }

            return lstAllSkuProductAI;
        }

        private async Task<PaginationUtility<string>> GetPaginationResultAsync(int pageNumber, int pageSize)
        {
            string url = _config.GetValue<string>("ProductAiApi:GetAllSku")!;

            PaginationUtility<string> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<string>>(json);
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

        private async Task CreateUpdateToQgoldDb(List<QgoldProduct> dataQgold)
        {
            Common.WriteLog("Start Create, update db!");
            try
            {
                foreach (var item in dataQgold)
                {
                    var price = decimal.Parse(item.Price);
                    if (price == 0)
                    {
                        item.Qty_Avail = "0";
                    }
                    if (item.Item_Type.Contains("Gift", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Accessory: Binoculars", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Accessory: Card Holders", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Accessory: Leather Attachments", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Accessory", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Accessory: Wallets", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Watch", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Packaging & Displays", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Watch: Children's", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Tools & Supplies", StringComparison.OrdinalIgnoreCase)
                    || item.Item_Type.Equals("Watch: Wrist", StringComparison.OrdinalIgnoreCase)
                    )
                    {
                        if(item.Description.Contains("Zippo", StringComparison.OrdinalIgnoreCase))
                        {
                            item.Price = (price + 3).ToString();
                        }
                        else
                        {
                            item.Price = (price + 16).ToString();
                        }
                        
                    }
                    var image = item.Image[..item.Image.LastIndexOf('.')].Replace('_', '/');
                    if (!image.Contains("--")
                    && !image.Contains("NOIMAGE")
                    && item.Status != "Discontinued")
                    {
                        if (string.IsNullOrEmpty(item.Image1Link)
                        && string.IsNullOrEmpty(item.Image2Link)
                        && string.IsNullOrEmpty(item.Image3Link)
                        && string.IsNullOrEmpty(item.Image4Link)
                        && string.IsNullOrEmpty(item.Image5Link)
                        && string.IsNullOrEmpty(item.Image6Link)
                        && string.IsNullOrEmpty(item.Image7Link)
                        && string.IsNullOrEmpty(item.Image8Link)
                        && string.IsNullOrEmpty(item.Image9Link)) { }
                        else
                        {
                            if (!item.Item.Contains("AAA")
                            && !item.Item.Contains("NOV")
                            && !item.Item.Contains("SEP")
                            && !item.Item.Contains("APR")
                            && !item.Item.Contains("JUN")
                            && !item.Item.Contains("OCT")
                            && !item.Item.Contains("DEC")
                            && !item.Item.Contains("MAY")
                            && !item.Item.Contains("JUL")
                            && !item.Item.Contains("MAR")
                            && !item.Item.Contains("FEB")
                            && !item.Item.Contains("AUG")
                            && !item.Item.Contains("JAN")
                            && !item.Item.Contains("UAL")
                            && !item.Item.Contains("PSU")
                            )
                            {
                                var skuTemp = GetSKU(item.Item);
                                var imgTemp = GetSKU(image);

                                if (!IsSimilar(skuTemp, imgTemp))
                                {
                                    //lstSkusubstitute.Add(new Substitute()
                                    //{
                                    //    ImgReplace = image,
                                    //    Item = item.Item,
                                    //    IsExistInListData = dataQgold.Any(x => x.Item.Equals(image, StringComparison.OrdinalIgnoreCase))
                                    //});
                                    item.Qty_Avail = "0";
                                }
                            }
                        }
                    }
                    var lstImg = new List<string>()
                    {
                        item.Image1Link,
                        item.Image2Link,
                        item.Image3Link,
                        item.Image4Link,
                        item.Image5Link,
                        item.Image6Link,
                        item.Image7Link,
                        item.Image8Link,
                        item.Image9Link
                    }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                    if (lstImg.Count > 0)
                    {
                        for (int i = 0; i < lstImg.Count; i++)
                        {
                            var propertyName = $"Image{i + 1}Link";
                            item.GetType().GetProperty(propertyName)?.SetValue(item, lstImg[i]);
                        }
                    }
                    else
                    {
                        item.Image1Link = item.ImageLink_1000;
                    }
                }

                var dtoQgold = _mapper.Map<List<QgoldData>>(dataQgold);

                var batchs = Common.SplitListIntoBatches(dtoQgold, 3000);

                bool deleteQgold = await DeleteItemQgold(dtoQgold);

                foreach (var item in batchs)
                {
                    var createUpdateResult = await _serviceAPI.CreateAndUpdateDataQgold(item);
                    Common.WriteLog($"Json result:{JsonConvert.SerializeObject(createUpdateResult)}");
                }

            }
            catch (Exception ex)
            {
                Common.WriteLog("End Create, update db with error: " + ex.Message);
            }
        }



        //Function Extensions
        private async Task<bool> DeleteItemQgold(List<QgoldData> dto)
        {
            try
            {
                var listItemQgold = await _serviceAPI.GetDataQgoldItem();

                var lstItemQgoldDTO = dto.Select(x => x.Item).ToList();

                var itemsNotInDto = listItemQgold.Except(lstItemQgoldDTO).ToList();

                if (itemsNotInDto.Count > 0)
                {
                    return (await _serviceAPI.DeleteDataQgold(itemsNotInDto)).IsSuccess;
                }
                return true;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not DeleteItemQgold " + e.Message);
                return false;
            }
        }

        private List<QgoldProduct> DownloadCsvAndConvertToObject()
        {
            #region Mở ra khi chạy Develop
            //Comemet filePath, csvContent khi chạy thực tế
            //3 dòng dưới để đoc file test. (Hàm DownloadCsv() khá lâu) 
            string filePath = AppDomain.CurrentDomain.BaseDirectory + @$"\ExportExcel\FileQgold\58772_Extract.txt";
            string csvContent = File.ReadAllText(filePath);
            csvContent = csvContent.Replace(", ", " ");
            #endregion


            #region Mở ra khi chạy Production
            ////thực tế để pull data từ Qgold
            //string csvContent = DownloadCsv();
            //csvContent = csvContent.Replace(", ", " ");
            //// luu file
            //string filePath = LocationExcelQgoldData();
            //// Lưu chuỗi vào file
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    writer.Write(csvContent);
            //}
            #endregion

            TextReader reader = new StringReader(csvContent);

            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ",",
                BadDataFound = null
            };
            var csv = new CsvReader(reader, configuration);
            csv.Context.RegisterClassMap<QgoldCsvDataMap>();
            List<QgoldProduct> objects = new();
            while (csv.Read())
            {
                try
                {
                    if (csv.TryGetField<string>(68, out var column68) == false)
                    {
                        QgoldProduct data = csv.GetRecord<QgoldProduct>();
                        if (data != null)
                        {
                            objects.Add(data);
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            csv.Dispose();
            return objects;
        }

        private string DownloadCsv()
        {
            try
            {
                // Get the object used to communicate with the server.
                FtpWebRequest request = WebRequest.Create(_fileUrl) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(_userid, _password);
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                var ret = reader.ReadToEnd();
                reader.Close();
                response.Close();

                return ret;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not call api ftp.qgold.com: " + e.ToString());
                throw;
            }
        }

        private string LocationExcelQgoldData()
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fileDirectory = Path.Combine(baseDirectory, "Storage", "ExportExcel", "FileQgold");

                if (!Directory.Exists(fileDirectory))
                    Directory.CreateDirectory(fileDirectory);

                string filePath = fileDirectory + "\\58772_Extract.txt";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath).Close();
                }
                else
                {
                    File.Delete(filePath);
                }
                return filePath;
            }
            catch (Exception e)
            {
                Common.WriteLog("Could not file 58772_Extract.txt " + e.ToString());
                throw;
            }
        }

        private bool IsSimilar(string skuTemp, string imgTemp)
        {

            if (skuTemp.Contains(imgTemp, StringComparison.OrdinalIgnoreCase))
                return true;
            if (imgTemp.Contains(skuTemp, StringComparison.OrdinalIgnoreCase))
                return true;

            if (imgTemp[2..].Contains(skuTemp[2..], StringComparison.OrdinalIgnoreCase))
                return true;
            if (skuTemp[2..].Contains(imgTemp[2..], StringComparison.OrdinalIgnoreCase))
                return true;

            if (imgTemp.Length > 2
                && skuTemp.Length > 2
                && imgTemp[..^2].Contains(skuTemp[..^2], StringComparison.OrdinalIgnoreCase))
                return true;

            if (imgTemp.Length > 4
                && skuTemp.Length > 4
                && skuTemp.Contains(imgTemp[..^2], StringComparison.OrdinalIgnoreCase))
                return true;

            if (imgTemp.Length > 4
                && skuTemp.Length > 4
                && skuTemp.Contains(imgTemp[2..], StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private string GetSKU(string item)
        {
            int index = item.IndexOfAny(new char[] { '-', '/' });
            if (index != -1)
            {
                return item.Substring(0, index);
            }
            else
            {
                return item;
            }
        }
    }
}
