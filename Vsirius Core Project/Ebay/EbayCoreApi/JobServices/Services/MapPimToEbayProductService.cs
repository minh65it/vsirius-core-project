using EbayCoreApi.Common.Configurations;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.Common.Mappers;
using EbayCoreApi.DataContext;
using EbayCoreApi.Dtos;
using EbayCoreApi.JobServices.Interfaces;
using EbayCoreApi.Models;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EbayCoreApi.JobServices.Services
{
    public class MapPimToEbayProductService : IMapPimToEbayProductService
    {
        private readonly EbayContext _context;
        private readonly IConfiguration _config;

        public MapPimToEbayProductService(
            EbayContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task MapperPimProductToEbayProduct()
        {
            UtilHelper.WriteLog("-----------------Start Mapping Pim Product To Ebay Local -----------------");
            try
            {
                UtilHelper.WriteLog($"Start Pull Data");
                List<PimProductDto> lstData = await PullDataFromTopSaleProductAsync();
                UtilHelper.WriteLog($"End Pull Data, total list: {lstData.Count}");

                UtilHelper.WriteLog("Start get excluded data");
                List<ExcludedProductDto> lstExcludedProduct = await GetExcludedProducts();
                UtilHelper.WriteLog($"End get excluded data : {lstExcludedProduct?.Count} records.");

                if (lstExcludedProduct != null && lstExcludedProduct.Any())
                {
                    foreach (var item in lstExcludedProduct)
                    {
                        var obj = lstData!
                            .FirstOrDefault(
                            x => UtilHelper.GetOriginSKU(x.item_sku) == UtilHelper.GetOriginSKU(item.MarketplaceSKU));
                        if (obj != null)
                        {
                            if (item.ExclusionType == ExclusionType.Deleted.ToString())
                            {
                                lstData!.Remove(obj);
                            }
                            else
                            {
                                obj.quantity = "0";
                            }
                        }
                    }
                }
                UtilHelper.WriteLog("Start Delete old Data");
                lstData = await ListDataAfterDelete(lstData);
                UtilHelper.WriteLog("End Delete old Data");

                UtilHelper.WriteLog("Push Data Start Bulk insert update to local API Walmart");
                List<VariationDto> lstVariation = await GetLstVariation();
                await PushItemToDbLocal(lstData, lstVariation);
                UtilHelper.WriteLog("Push Data End Bulk insert update to local API Walmart");

                lstData.Clear();
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog("Process Mapper Error:" + ex.Message);
            }

            UtilHelper.WriteLog("-----------------End Mapping Pim Product To Ebay Local------------------");
        }

        private async Task<List<VariationDto>> GetLstVariation()
        {
            var rs = new List<VariationDto>();
            try
            {
                var client = new HttpClient();
                var url = $"{ConfigHolder.Configuration["PimApi:GetVariation"]}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    rs = JsonConvert.DeserializeObject<List<VariationDto>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"GetLstVariation failed with err: {ex.Message}");
            }
            return rs;
        }

        private async Task<List<ExcludedProductDto>> GetExcludedProducts()
        {
            try
            {
                List<ExcludedProductDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationExcludedProductAsync(1, pageSize);
                var totalPages = 0;
                List<ExcludedProductDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData!);
                    UtilHelper.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtil<ExcludedProductDto>>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationExcludedProductAsync(pageNumber, pageSize));

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
                                allResults.AddRange(result.Result);
                            }
                            else
                            {
                                UtilHelper.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"GetExcludedProducts Failed with exception: {ex.Message}");
                return null;
            }
        }

        private async Task<PaginationUtil<ExcludedProductDto>> GetPaginationExcludedProductAsync(int pageNumber, int pageSize)
        {
            string url = ConfigHolder.Configuration["APIExcludedProduct:PullData"]!;

            PaginationUtil<ExcludedProductDto> result = null;

            string apiUrl = $"{url}?Marketplace=Walmart&PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtil<ExcludedProductDto>>(json);
                    UtilHelper.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    UtilHelper.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        private async Task PushItemToDbLocal(List<PimProductDto> lstData, List<VariationDto> lstVariation)
        {
            try
            {
                var lstUpload = new List<Product>();

                UtilHelper.WriteLog("Mapper Start");

                var lstMap = lstData
                    .Where(item => item.item_name.Length < 81 && item.product_description.Length < 801)
                    .GroupBy(item => item.item_sku)
                    .Select(group => group.First())
                    .ToList();


                foreach (var item in lstMap)
                {
                    try
                    {
                        lstUpload.Add(CustomMapper.TransformPimProductToEbay(item));
                    }
                    catch (Exception ex)
                    {
                        UtilHelper.WriteLog($"Mapper Error: {ex.Message} at sku: {item.item_sku} ");
                    }
                }

                UtilHelper.WriteLog("Mapper End");

                var batchSize = 3000;
                var totalItems = lstUpload.Count;
                var numBatches = (int)Math.Ceiling((double)totalItems / batchSize);

                for (int i = 0; i < numBatches; i++)
                {
                    var startIndex = i * batchSize;
                    var batchItems = lstUpload.Skip(startIndex).Take(batchSize).ToList();
                    try
                    {
                        await _context.BulkInsertOrUpdateAsync(batchItems, new BulkConfig()
                        {
                            PropertiesToExclude = new List<string> { "Note" }
                        });
                    }
                    catch (Exception ex)
                    {
                        UtilHelper.WriteLog($"BulkInsertUpdate failed: {ex.Message}");
                        continue;
                    }
                }

            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"PushItemToLocalWalmartApi failed. Exception: {ex.Message}");
            }
        }

        private async Task<List<PimProductDto>> ListDataAfterDelete(List<PimProductDto> lstUpload)
        {
            var lstAfterDelete = new List<PimProductDto>();

            if (lstUpload == null)
            {
                return lstAfterDelete;
            }
            else
            {
                try
                {
                    //Get Id in local to check exists
                    List<string> lstSkuLocal = await _context.Products
                        .AsNoTracking()
                        .Where(x => x.IsDeleted == false)
                        .Select(x => x.Sku)
                        .ToListAsync();

                    var lstSkuUploadOrigin = lstUpload.Select(x => UtilHelper.GetOriginSKU(x.item_sku)).ToList();

                    var lstSkuDelete = lstSkuLocal.Where(x => !lstSkuUploadOrigin.Contains(UtilHelper.GetOriginSKU(x))).ToList();

                    if (lstSkuDelete != null && lstSkuDelete.Any())
                    {
                        await DeleteByLstSku(lstSkuDelete);
                        lstAfterDelete = lstUpload.Where(x => !lstSkuDelete.Contains(x.item_sku)).ToList();
                    }
                    else
                    {
                        lstAfterDelete = lstUpload;
                    }
                }
                catch (Exception ex)
                {
                    UtilHelper.WriteLog($"Call API Delete failed when mapping with err: {ex.Message}");
                }
            }
            return lstAfterDelete;
        }

        private async Task DeleteByLstSku(List<string> lstSkuDelete)
        {
            var batchs = UtilHelper.SplitListIntoBatches(lstSkuDelete, 100);
            foreach (var item in batchs)
            {
                await _context.Products
                    .Where(x => item.Contains(x.Sku)).Select(a => new { a.Sku, a.IsDeleted })
                    .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.IsDeleted, true));
            }
        }

        private async Task<List<PimProductDto>> PullDataFromTopSaleProductAsync()
        {
            try
            {
                List<PimProductDto> allResults = new List<PimProductDto>();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationTopResultAsync(1, pageSize);
                var totalPages = 0;
                List<PimProductDto> firstData = new List<PimProductDto>();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData);
                    UtilHelper.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtil<PimProductDto>>> tasks = new List<Task<PaginationUtil<PimProductDto>>>();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationTopResultAsync(pageNumber, pageSize));

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
                                allResults.AddRange(result.Result);
                            }
                            else
                            {
                                UtilHelper.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"Pull Data Failed with exception: {ex.Message}");
                return null;
            }
        }

        private async Task<PaginationUtil<PimProductDto>> GetPaginationTopResultAsync(int pageNumber, int pageSize)
        {
            string url = _config["PimApi:GetTopProduct"]!;

            PaginationUtil<PimProductDto> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}&isPagging=true&&isSortQuantity=true&isGroupQuantity=true";

            HttpClient client = new HttpClient();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtil<PimProductDto>>(json);
                    UtilHelper.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    UtilHelper.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }
    }
}
