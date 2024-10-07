using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.Dtos.SellerItem;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class UpdatePriceService : IUpdatePriceService
    {
        private readonly IConfiguration _config;
        private readonly IInteractWalmartSellerCommonService _interactWalmart;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _uploadsPath = Path.Combine(
                   AppDomain.CurrentDomain.BaseDirectory,
                   "Storage",
                   "Uploads",
                   "Price",
                   DateTime.Now.ToString("yyyyMMdd_hh"));

        public UpdatePriceService(IConfiguration config, IInteractWalmartSellerCommonService interactWalmart, IServiceProvider serviceProvider)
        {
            _config = config;
            _interactWalmart = interactWalmart;
            _serviceProvider = serviceProvider;
        }

        public async Task UpdatePrice()
        {
            Util.WriteLog("---------------------------Update Price Process Start-------------------------------------");
            try
            {
                Util.WriteLog("Pull updated Price data Start");
                //Pull all data in walmart (seller)
                Util.WriteLog("GetDataSeller Start");
                List<Item> lstSeller = await _interactWalmart.GetItemsFromSeller(true, true);
                Util.WriteLog($"Total Item when get finish: {lstSeller.Count}");
                Util.WriteLog("GetDataSeller End");


                //Get all data in Walmart (Database)
                Util.WriteLog("Get data from WalmartDB Start");
                List<WalmartPriceInventoryDTO> lstLocal = await GetListPriceData();
                List<WalmartPriceInventoryDTO> lstDoubleLocal = new();
                lstLocal.ForEach(x =>
                {
                    if (x.SKU.StartsWith("QU-"))
                    {
                        WalmartPriceInventoryDTO clone = Util.CloneObject(x);
                        clone.SKU = "Q-" + Util.SKUQgold(x.SKU);
                        lstDoubleLocal.Add(clone);
                    }
                });
                Util.WriteLog($"Total Item when get finish: {lstLocal.Count}");
                lstLocal.AddRange(lstDoubleLocal);
                //var lstSkuLocal = lstLocal.Select(x => x.SKU).ToList();
                Util.WriteLog("Get data from WalmartDB End");

                List<WalmartPriceInventoryDTO> lUp = new ();

                Parallel.ForEach(lstSeller, item =>
                {
                    var check = lstLocal.FirstOrDefault(x => x.SKU == item.sku);
                    if (check != null)
                    {
                        var price = double.Parse(check.price);
                        if (price <= 7.5)
                        {
                            price += 8.5;
                        }
                        lUp.Add(new WalmartPriceInventoryDTO()
                        {
                            SKU = item.sku,
                            price = price.ToString(),
                            quantity = check.quantity,
                        });
                    }
                });

                Util.WriteLog("Filter sku from seller for update End");

                await WriteFileJSONUploadPrice(lUp);
                Util.WriteLog("Write File Json Upload Price End");

                await UpPriceDataToWalmart();
                Util.WriteLog("Up Price Data ToWalmart End");

                lstSeller = null;
                lstLocal = null;
                lUp = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Update price process. Exception: {ex.Message}");
            }

            Util.WriteLog("---------------------------Update Price Process Start-------------------------------------");
        }

        private async Task UpPriceDataToWalmart()
        {
            try
            {
                var fileInfos = GetNewFileToUpload();
                if (fileInfos == null)
                {
                    Util.WriteLog($"Can not find file to upload bulk item");
                }
                else
                {
                    for (int i = 0; i < fileInfos.Count; i++)
                    {
                        var item = fileInfos[i];

                        await UploadSingleFileAsync(item);

                        if (i == 9 && fileInfos.Count > 10)
                        {
                            await Task.Delay(TimeSpan.FromMinutes(35));
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"UpPriceDataToWalmart failed Exception: {ex.Message}");
            }

        }

        private async Task UploadSingleFileAsync(FileInfo fileInfo)
        {
            try
            {
                TokenResult tokenResult = await _interactWalmart.GetTokenFromWalmartSeller();

                if (!string.IsNullOrEmpty(tokenResult.AccessToken))
                {
                    var url = $"{_config.GetValue<string>("Walmart:BulkItemWalmart")}?feedType=price";
                    using HttpClient client = new();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                    client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);
                    client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                        $"multipart/form-data; boundary={tokenResult.CorrelationId}");

                    var formData = new MultipartFormDataContent();
                    var fileContent = new StreamContent(fileInfo.OpenRead());
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    formData.Add(fileContent, "file", fileInfo.Name);
                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ResultFeed>(responseBody);
                        string feedId = result!.FeedId;
                        Util.WriteLog($"Success: File {fileInfo.Name}.FeedId: {feedId}");
                    }
                    else
                    {
                        Util.WriteLog($"Failed: File {fileInfo.Name}.Reason:{responseBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Failed File {fileInfo.Name}.Err: {ex.Message}");

            }
        }

        private List<FileInfo>? GetNewFileToUpload()
        {
            
            List<FileInfo>? excelFiles = new DirectoryInfo(_uploadsPath).GetFiles()
                .Where(f => Regex.IsMatch(f.Name, @"^.+\.(json)$")).ToList();

            return excelFiles;
        }

        private async Task WriteFileJSONUploadPrice(List<WalmartPriceInventoryDTO> items)
        {
            try
            {
                Util.CreateFolder(_uploadsPath);
                int batchSize = 10000;
                int totalItems = items.Count;
                int totalBatches = (int)Math.Ceiling((double)totalItems / batchSize);
                for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
                {
                    var batch = items.Skip(batchSize * batchIndex).Take(batchSize).ToList();

                    WalmartPriceUpload model = new WalmartPriceUpload()
                    {
                        Price = new List<PriceUpload>() { },
                        PriceHeader = new PriceHeader() { version = "1.7" }
                    };
                    foreach (var item in batch)
                    {
                        PriceUpload price = new PriceUpload()
                        {
                            sku = item.SKU,
                            pricing = new List<pricing>()
                            {
                                new pricing()
                                {
                                    currentPrice =new currentPrice()
                                    {
                                        amount = decimal.Parse(item.price),
                                        currency = "USD"
                                    }
                                }
                            }
                        };
                        model.Price.Add(price);
                    }
                    var json = System.Text.Json.JsonSerializer.Serialize(model);
                    string filePath = Path.Combine(_uploadsPath, $"PriceWalmart_{batchIndex}.json");
                    await File.WriteAllTextAsync(filePath, json);
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"WriteFileUploadInventory failed Err: {ex.Message}");
            }
        }

        private async Task<List<WalmartPriceInventoryDTO>> GetListPriceData()
        {
            List<WalmartPriceInventoryDTO> inventory = new List<WalmartPriceInventoryDTO>();
            try
            {
                int curPage = 1;
                int pageSize = 5000;
                var data = await CallApiGetPrice(curPage, pageSize);
                inventory.AddRange(data.Result);
                int totalPageFromFirstExportResult = data.Pagination.TotalPage;
                Util.WriteLog($"Total pages {pageSize}");

                if (totalPageFromFirstExportResult != 0)
                {
                    List<Task<PaginationUtility<WalmartPriceInventoryDTO>>> tasks = new();
                    int tasksPerThread = 0;
                    for (int i = 2; i <= totalPageFromFirstExportResult; i++)
                    {
                        tasks.Add(CallApiGetPrice(i, pageSize));

                        if (tasksPerThread < 5 && i != totalPageFromFirstExportResult)
                        {
                            tasksPerThread++;
                        }
                        else
                        {
                            tasksPerThread = 0;
                            var results = await Task.WhenAll(tasks);
                            foreach (var item in results)
                            {
                                inventory.AddRange(item.Result);
                            }
                            tasks.RemoveAll(x => x.IsCompleted);
                        }
                    }
                }
                return inventory;

            }
            catch (Exception ex)
            {
                Util.WriteLog($"GetFileExcelFromAPI Walmart Local failed Delete API failed with exception {ex.Message}");
                return inventory;
            }
        }

        private async Task<PaginationUtility<WalmartPriceInventoryDTO>> CallApiGetPrice(int curPage, int pageSize)
        {
            PaginationUtility<WalmartPriceInventoryDTO>? result = new PaginationUtility<WalmartPriceInventoryDTO>();
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<IProductWalmartService>();
                    result = await service.GetPriceInventory(new PaginationParam()
                    {
                        PageNumber = curPage,
                        PageSize = pageSize
                    });
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"CallApiExportAndSave failed page failed:{curPage}; with exception: {ex.Message}");
            }

            return result;
        }
    }
}
