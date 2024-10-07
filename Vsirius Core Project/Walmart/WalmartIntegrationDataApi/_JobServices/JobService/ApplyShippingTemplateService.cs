using Newtonsoft.Json;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.Dtos.SellerItem;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class ApplyShippingTemplateService : IApplyShippingTemplateService
    {
        private readonly IConfiguration _config;
        private readonly IInteractWalmartSellerCommonService _interactWalmart;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _uploadShippingPath = Path.Combine(
                   AppDomain.CurrentDomain.BaseDirectory,
                   "Storage",
                   "Shipping",
                   DateTime.Now.ToString("yyyyMMddhh"));
        public ApplyShippingTemplateService(
            IConfiguration config,
            IInteractWalmartSellerCommonService interactWalmart,
            IServiceProvider serviceProvider)
        {
            _config = config;
            _interactWalmart = interactWalmart;
            _serviceProvider = serviceProvider;
        }

        public async Task ApplyShippingTemplate()
        {
            Util.WriteLog("----------------------------------Start Apply Shipping Template In Walmart---------------------------------------------");
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
                Util.WriteLog("Total data from local: " + lstLocal.Count);
                Util.WriteLog("Get data from WalmartDB End");


                await ApplyShippingTemplateToSeller(lstSeller, lstLocal);
                lstLocal = null;
                lstSeller = null;
                //lstSkuLocal = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Apply Shipping Template In Walmart Process error: {ex.Message}");
            }

            Util.WriteLog("----------------------------------End Apply Shipping Template In Walmart---------------------------------------------");
        }

        private async Task ApplyShippingTemplateToSeller(List<Item> lstSeller, List<WalmartPriceInventoryDTO> lstLocal)
        {
            if (lstSeller == null || lstSeller.Count == 0)
            {
                Util.WriteLog("No data to ApplyShippingTemplate");
                return;
            }
            Util.CreateFolder(_uploadShippingPath);

            int batchSize = 10000;
            int totalItems = lstSeller.Count;
            int totalBatches = (int)Math.Ceiling((double)totalItems / batchSize);

            for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
            {
                var batch = lstSeller.Skip(batchSize * batchIndex).Take(batchSize).ToList();
                var model = new TemplateMapShipping()
                {
                    ItemFeedHeader = new ItemFeedHeader()
                    {
                        sellingChannel = "precisedelivery",
                        locale = "en",
                        version = "1.0",
                    },
                    Item = new List<ItemAdd>()
                };

                foreach (var item in batch)
                {
                    var shippingItem = new ItemAdd();
                    var obj = lstLocal.FirstOrDefault(x => x.SKU.Equals(item.sku));
                    if (obj != null)
                    {
                        if (Double.Parse(obj.price!) > 7.5)
                        {
                            shippingItem.PreciseDelivery = new PreciseDelivery()
                            {
                                sku = item.sku,
                                actionType = "Add",
                                shippingTemplateId = _config.GetValue<string>("Walmart:shippingTemplateId")!,
                                fulfillmentCenterId = _config.GetValue<string>("Walmart:fulfillmentCenterId")!
                            };
                        }
                        else
                        {
                            shippingItem.PreciseDelivery = new PreciseDelivery()
                            {
                                sku = item.sku,
                                actionType = "Remove",
                                shippingTemplateId = _config.GetValue<string>("Walmart:shippingTemplateId")!,
                                fulfillmentCenterId = _config.GetValue<string>("Walmart:fulfillmentCenterId")!
                            };
                        }
                    }
                    else
                    {
                        shippingItem.PreciseDelivery = new PreciseDelivery()
                        {
                            sku = item.sku,
                            actionType = "Add",
                            shippingTemplateId = _config.GetValue<string>("Walmart:shippingTemplateId")!,
                            fulfillmentCenterId = _config.GetValue<string>("Walmart:fulfillmentCenterId")!
                        };
                    }
                    model.Item.Add(shippingItem);
                }
                if (batchIndex == 9 && totalBatches > 10)
                {
                    await Task.Delay(TimeSpan.FromMinutes(35));
                }
                await WriteJsonFileToShipping(model, batchIndex);
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

        }

        private async Task WriteJsonFileToShipping(TemplateMapShipping model, int batchIndex)
        {
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(model);
                string filePath = Path.Combine(_uploadShippingPath, $"Shipping_{batchIndex}.json");
                await File.WriteAllTextAsync(filePath, json);
                var fileInfo = new FileInfo(filePath);

                await UploadShipping(fileInfo);

            }
            catch (Exception ex)
            {
                Util.WriteLog($"Error WriteJsonFileToShipping: {ex.Message}");
            }
        }
        private async Task UploadShipping(FileInfo fileInfo)
        {
            try
            {
                TokenResult tokenResult = await _interactWalmart.GetTokenFromWalmartSeller();

                if (!string.IsNullOrEmpty(tokenResult.AccessToken))
                {
                    var url = $"{_config.GetValue<string>("Walmart:BulkItemWalmart")}?feedType=SKU_TEMPLATE_MAP";
                    using HttpClient client = new();
                    client.DefaultRequestHeaders.Add("accept", "application/json");
                    client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                    client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);
                    client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", $"multipart/form-data; boundary={tokenResult.CorrelationId}");

                    var formData = new MultipartFormDataContent();
                    var fileContent = new StreamContent(fileInfo.OpenRead());
                    fileContent.Headers.Add("type", "application/json");
                    formData.Add(fileContent, "file", fileInfo.Name);

                    HttpResponseMessage response = await client.PostAsync(url, formData);
                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var result = JsonConvert.DeserializeObject<ResultFeed>(responseBody);
                        string feedId = result!.FeedId;
                        Util.WriteLog($"ApplyShippingTemplate Success. FeedId: {feedId}");
                    }
                    else
                    {
                        Util.WriteLog($"ApplyShippingTemplate Failed. Err: {response.Content}. Phrase: {responseBody}");
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"ApplyShippingTemplate Failed. Err: {fileInfo.Name}. Err: {ex.Message}");
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
                Util.WriteLog($"Total pages: {totalPageFromFirstExportResult}");

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
