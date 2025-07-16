using AutoMapper;
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
        private readonly PimApi._Services.Interfaces.IOrderService _pimOrderService;
        private readonly ExcludedProductAPI._Services.Interfaces.IExcludedProductService _excludedProductService;
        private readonly PimApi._Services.Interfaces.IVariationService _variationService;
        private readonly IMapper _mapper;

        public MapPimToEbayProductService(
            EbayContext context,
            PimApi._Services.Interfaces.IOrderService pimOrderService,
            ExcludedProductAPI._Services.Interfaces.IExcludedProductService excludedProductService,
            PimApi._Services.Interfaces.IVariationService variationService,
            IMapper mapper,
            IConfiguration config)
        {
            _context = context;
            _config = config;
            _pimOrderService = pimOrderService;
            _excludedProductService = excludedProductService;
            _variationService = variationService;
            _mapper = mapper;
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
                List<VariationDto> lstVariation = await GetLstVariation_RFT();
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

        private async Task<List<VariationDto>> GetLstVariation_RFT()
        {
            var rs = new List<VariationDto>();
            try
            {
                var data = await _variationService.GetAll();

                foreach ( var item in data)
                {
                    var dto = _mapper.Map<VariationDto>(item);
                    rs.Add(dto);
                }
                UtilHelper.WriteLog($"GetLstVariation success");
                return rs;
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"GetLstVariation failed with err: {ex.Message}");
                return rs;
            }
        }

        private async Task<List<ExcludedProductDto>> GetExcludedProducts()
        {
            try
            {
                List<ExcludedProductDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationExcludedProductAsync_RFT(1, pageSize);
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


                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    var getData = await GetPaginationExcludedProductAsync_RFT(pageNumber, pageSize);

                    foreach (var result in getData.Result)
                    {
                        if (getData.Result != null)
                        {
                            allResults.AddRange(getData.Result);
                        }
                        else
                        {
                            UtilHelper.WriteLog($"Failed when add record");
                        }
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

        private async Task<PaginationUtil<ExcludedProductDto>> GetPaginationExcludedProductAsync_RFT(int pageNumber, int pageSize)
        {
            PaginationUtil<ExcludedProductDto> result = new PaginationUtil<ExcludedProductDto>(null, 1, 1, 1, 1);
            try
            {
                var pagination = new ExcludedProductAPI.Helpers.PaginationParam();
                pagination.PageNumber = pageNumber;
                pagination.PageSize = pageSize;

                var search = new ExcludedProductAPI.DTOs.ExcludedProductParam();
                search.Marketplace = "Walmart";

                var data = await _excludedProductService.GetAll(pagination, search);

                result.Result = new List<ExcludedProductDto>();
                foreach (var item in data.Result)
                {
                    var dto = _mapper.Map<ExcludedProductDto>(item);
                    result.Result.Add(dto);
                }
                UtilHelper.WriteLog($"Get excluded product success at page {pageNumber}");

                result.Pagination.PageNumber = data.Pagination.PageNumber;
                result.Pagination.PageSize = data.Pagination.PageSize;
                result.Pagination.TotalCount = data.Pagination.TotalCount;
                result.Pagination.Skip = data.Pagination.Skip;

                return result;
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"Get excluded product failed with exception {ex.Message} at page {pageNumber}");
                return result;
            }

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
                var dataFirstCall = await GetPaginationTopResultAsync_RFT(1, pageSize);
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

                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    var getData = await GetPaginationTopResultAsync_RFT(pageNumber, pageSize);

                    foreach (var result in getData.Result)
                    {
                        if (getData.Result != null)
                        {
                            allResults.AddRange(getData.Result);
                        }
                        else
                        {
                            UtilHelper.WriteLog($"Failed when add record");
                        }
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

        private async Task<PaginationUtil<PimProductDto>> GetPaginationTopResultAsync_RFT(int pageNumber, int pageSize)
        {
            PaginationUtil<PimProductDto> result = new PaginationUtil<PimProductDto>(null, 1, 1, 1, 1);
            try
            {
                var pagination = new PimApi.Helpers.Utilities.PaginationParam();
                pagination.PageNumber = pageNumber;
                pagination.PageSize = pageSize;

                var search = new PimApi.Dtos.OrderDto.SearchOrderParams();
                search.isPagging = true;
                search.isSortQuantity = true;
                search.isGroupQuantity = true;

                var data = await _pimOrderService.GetProductTop(pagination, search);

                result.Result = new List<PimProductDto>();
                foreach (var item in data.Result)
                {
                    var dto = _mapper.Map<PimProductDto>(item);
                    result.Result.Add(dto);
                }

                result.Pagination.PageNumber = data.Pagination.PageNumber;
                result.Pagination.PageSize = data.Pagination.PageSize;
                result.Pagination.TotalCount = data.Pagination.TotalCount;
                result.Pagination.Skip = data.Pagination.Skip;

                return result;
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"Get top product from pim failed: {ex.ToString()}");
                return result;
            }
            
        }
    }
}
