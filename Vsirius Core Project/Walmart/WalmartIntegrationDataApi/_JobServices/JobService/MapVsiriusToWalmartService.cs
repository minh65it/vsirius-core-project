using AutoMapper;
using Newtonsoft.Json;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Common.Mappers;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Dtos.Vsirius;
using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class MapVsiriusToWalmartService : IMapVsiriusToWalmartService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IProductWalmartService _productWalmartService;
        private readonly PimApi._Services.Interfaces.IProductService _productService;
        private readonly PimApi._Services.Interfaces.IVariationService _variationService;
        private readonly ExcludedProductAPI._Services.Interfaces.IExcludedProductService _excludedProductService;

        public MapVsiriusToWalmartService(
            IConfiguration configuration,
            IProductWalmartService productWalmartService,
            PimApi._Services.Interfaces.IProductService productService,
            PimApi._Services.Interfaces.IVariationService variationService,
            ExcludedProductAPI._Services.Interfaces.IExcludedProductService excludedProductService,
            IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
            _productWalmartService = productWalmartService;
            _productService = productService;
            _excludedProductService = excludedProductService;
            _variationService = variationService;
        }


        public async Task MapVsiriusToWalmart()
        {
            Util.WriteLog("----------------------------------Start Map Vsirius product To Walmart---------------------------------------------");

            try
            {
                //Pull data amazon
                Util.WriteLog("Pull Data Start pull data");
                List<VsiriusProductDto>? lstData = await PullDataFromVsiriusAsync();
                Util.WriteLog($"Data pull : {lstData?.Count} records.");
                //Filter listData
                lstData = lstData!
                   .Where(x => !x.item_name.Contains("Howard", StringComparison.OrdinalIgnoreCase)).ToList();
                Util.WriteLog("Pull Data End pull data");

                Util.WriteLog("Start get excluded data");
                List<ExcludedProductDto>? lstExcludedProduct = await GetExcludedProducts();
                Util.WriteLog($"End get excluded data : {lstExcludedProduct?.Count} records.");

                if (lstExcludedProduct != null && lstExcludedProduct.Any())
                {
                    foreach (var item in lstExcludedProduct)
                    {
                        var obj = lstData!.FirstOrDefault(x => x.item_sku == item.MarketplaceSKU);
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
                /*
                //Util.WriteLog("Start Call API get mapping SKU");
                //List<QgoldWalmartSKUParam> lstSKUMap = await GetSKUMapping();

                //if (lstSKUMap.Any())
                //{
                //    Parallel.ForEach(lstSKUMap, item =>
                //    {
                //        var obj = lstData?.FirstOrDefault(x => Util.SKUQgold(x.item_sku).Equals(item.SKUQgold));
                //        if (obj != null)
                //        {
                //            obj.item_sku = item.SKUWalmart!;
                //            obj.external_product_id_type = "";
                //            obj.external_product_id = "";
                //        }
                //    });
                //}
                //lstSKUMap.Clear();
                //Util.WriteLog("End Call API get mapping SKU");
                */
                Util.WriteLog("Start delete old Data");
                lstData = await ListDataAfterDelete(lstData);
                Util.WriteLog("End delete old Data");

                #region Bulk insert update to local API Walmart

                List<VariationDto> lstVariation = await GetLstVariation_RFT();
                Util.WriteLog("Push Data Start Bulk insert update to local API Walmart");
                await PushItemToLocalWalmartApi(lstData, lstVariation);
                Util.WriteLog("Push Data End Bulk insert update to local API Walmart");
                #endregion

                lstData = null;
                lstExcludedProduct = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog("Run process Error:" + ex.Message);
            }
            Util.WriteLog("----------------------------------End Map Vsirius product To Walmart---------------------------------------------");

        }

        private async Task<List<VariationDto>> GetLstVariation()
        {
            var rs = new List<VariationDto>();
            try
            {
                var client = new HttpClient();
                var url = $"{_configuration["PIMAPI:GetVariation"]}";
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    rs = JsonConvert.DeserializeObject<List<VariationDto>>(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"GetLstVariation failed with err: {ex.Message}");
            }
            return rs;
        }

        private async Task<List<VariationDto>> GetLstVariation_RFT()
        {
            var rs = new List<VariationDto>();
            try
            {
                var data = await _variationService.GetAll();
                foreach (var item in data)
                {
                    var dto = _mapper.Map<VariationDto>(item);
                    rs.Add(dto);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
            return rs;
        }

        private async Task PushItemToLocalWalmartApi(List<VsiriusProductDto> lstData, List<VariationDto> lstVariation)
        {
            try
            {
                var lstUpload = new List<Walmart>();

                Util.WriteLog("Mapper Start");

                Parallel.ForEach(lstData, item =>
                {
                    try
                    {
                        var walmart = CustomMapping.TransformVsiriusProductToWalmart(item);
                        if (walmart.SKU.StartsWith("Q"))
                        {
                            var variation = lstVariation.FirstOrDefault(x => Util.GetOriginSKU(x.ItemSku) == Util.GetOriginSKU(walmart.SKU));
                            if (variation != null)
                            {
                                walmart.variantGroupId = $"{walmart.SKU[..walmart.SKU.IndexOf('-')]}-{Util.GetOriginSKU(variation.VariationId)}";
                                walmart.variantAttributeNames = Util.ToCamelCase(variation.VariationName);
                            }
                        }
                        lstUpload.Add(walmart);
                    }
                    catch (Exception ex)
                    {
                        Util.WriteLog($"Mapper Error: {ex.Message} at sku: {item.item_sku} ");
                    }
                });

                Util.WriteLog("Mapper End");

                var batchSize = 3000;
                var totalItems = lstUpload.Count;
                var numBatches = (int)Math.Ceiling((double)totalItems / batchSize);

                for (int i = 0; i < numBatches; i++)
                {
                    var startIndex = i * batchSize;
                    var batchItems = lstUpload.Skip(startIndex).Take(batchSize).ToList();
                    try
                    {
                        await _productWalmartService.BulkInsertUpdate(_mapper.Map<List<WalmartDTO>>(batchItems));
                    }
                    catch (Exception ex)
                    {
                        Util.WriteLog($"BulkInsertUpdate failed: {ex.Message}");
                        continue;
                    }
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"PushItemToLocalWalmartApi failed. Exception: {ex.Message}");
            }
        }

        private async Task<List<VsiriusProductDto>> ListDataAfterDelete(List<VsiriusProductDto> lstUpload)
        {
            var lstAfterDelete = new List<VsiriusProductDto>();

            if (lstUpload == null || lstUpload.Count == 0)
            {
                return lstAfterDelete;
            }
            else
            {
                try
                {
                    List<string> lstSkuLocal = (List<string>)(await _productWalmartService.GetAllId()).Data;

                    var lstSkuUploadOrigin = lstUpload.Select(x => Util.GetOriginSKU(x.item_sku)).ToList();

                    var lstSkuDelete = lstSkuLocal.Where(x => !lstSkuUploadOrigin.Contains(Util.GetOriginSKU(x))).ToList();

                    if (lstSkuDelete != null && lstSkuDelete.Any())
                    {
                        await _productWalmartService.Delete(lstSkuDelete);
                        lstAfterDelete = lstUpload.Where(x => !lstSkuDelete!.Contains(x.item_sku)).ToList();
                    }
                    else
                    {
                        lstAfterDelete = lstUpload;
                    }

                }
                catch (Exception ex)
                {
                    Util.WriteLog($"Delete API failed with exception {ex.Message}");
                    throw;
                }
            }

            return lstAfterDelete;
        }

        private async Task<List<QgoldWalmartSKUParam>> GetSKUMapping()
        {
            var client = new HttpClient();
            var url = _configuration.GetValue<string>("SkuMappingAPI:GetSkuWalmartFromQgold") + "?IsPaging=false";

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<PaginationUtility<QgoldWalmartSKUParam>>(content)!;

                return data.Result;
            }

            Util.WriteLog($"Error code: {response.StatusCode}");
            return new List<QgoldWalmartSKUParam>();
        }

        private async Task<List<ExcludedProductDto>?> GetExcludedProducts()
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
                    Util.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    var getData = await GetPaginationExcludedProductAsync_RFT(pageNumber, pageSize);
                    if (getData.Result != null)
                    {
                        allResults.AddRange(getData.Result);
                    }
                    else
                    {
                        Util.WriteLog($"Failed when add record");
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"GetExcludedProducts Failed with exception: {ex.Message}");
                return null;
            }
        }

        private async Task<PaginationUtility<ExcludedProductDto>?> GetPaginationExcludedProductAsync(int pageNumber, int pageSize)
        {
            string url = _configuration.GetValue<string>("APIExcludedProduct:PullData")!;

            PaginationUtility<ExcludedProductDto>? result = null;

            string apiUrl = $"{url}?Marketplace=Walmart&PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<ExcludedProductDto>>(json);
                    Util.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    Util.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        private async Task<PaginationUtility<ExcludedProductDto>?> GetPaginationExcludedProductAsync_RFT(int pageNumber, int pageSize)
        {
            var result = new PaginationUtility<ExcludedProductDto>(null, 1, 1, 1, 1);
            try
            {
                var pagination = new ExcludedProductAPI.Helpers.PaginationParam();
                pagination.PageSize = pageSize;
                pagination.PageNumber = pageNumber;

                var search = new ExcludedProductAPI.DTOs.ExcludedProductParam();
                search.Marketplace = "Walmart";

                var data = await _excludedProductService.GetAll(pagination, search);
                result.Result = new List<ExcludedProductDto>();

                result.Pagination.TotalCount = data.Pagination.TotalCount;
                result.Pagination.PageNumber = data.Pagination.PageNumber;
                result.Pagination.PageSize = data.Pagination.PageSize;
                result.Pagination.Skip = data.Pagination.Skip;
                foreach (var item in data.Result)
                {
                    var dto = _mapper.Map<ExcludedProductDto>(item);
                    result.Result.Add(dto);
                }
                Util.WriteLog($"Get data from Excluded success at page {pageNumber}");
                return result;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Get data from Excluded failed with exception {ex.Message} at page {pageNumber}");
                return result;
            }

        }

        private async Task<List<VsiriusProductDto>?> PullDataFromVsiriusAsync()
        {
            try
            {
                List<VsiriusProductDto> allResults = new();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationResultAsync_RFT(1, pageSize);
                var totalPages = 0;
                List<VsiriusProductDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData!);
                    Util.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    var getData = await GetPaginationResultAsync_RFT(pageNumber, pageSize);
                    if (getData.Result != null)
                    {
                        allResults.AddRange(getData.Result);
                    }
                    else
                    {
                        Util.WriteLog($"Failed when add record");
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Pull Data Failed with exception: {ex.Message}");
                return null;
            }
        }

        private async Task<PaginationUtility<VsiriusProductDto>?> GetPaginationResultAsync(int pageNumber, int pageSize)
        {
            string url = _configuration.GetValue<string>("PIMAPI:GetData")!;

            PaginationUtility<VsiriusProductDto>? result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}";

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<VsiriusProductDto>>(json);
                    Util.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    Util.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        private async Task<PaginationUtility<VsiriusProductDto>?> GetPaginationResultAsync_RFT(int pageNumber, int pageSize)
        {
            PaginationUtility<VsiriusProductDto> result = new PaginationUtility<VsiriusProductDto>(null, 1, 1, 1, 1);
            try
            {
                var pagination = new PimApi.Helpers.Utilities.PaginationParam();
                pagination.PageSize = pageSize;
                pagination.PageNumber = pageNumber;

                var search = new PimApi.Dtos.SearchParams();

                var data = await _productService.GetData(pagination, search, true);
                result.Result = new List<VsiriusProductDto>();

                result.Pagination.PageNumber = data.Pagination.PageNumber;
                result.Pagination.PageSize = data.Pagination.PageSize;
                result.Pagination.TotalCount = data.Pagination.TotalCount;
                result.Pagination.Skip = data.Pagination.Skip;
                foreach (var item in data.Result)
                {
                    var dto = _mapper.Map<VsiriusProductDto>(item);
                    result.Result.Add(dto);
                }
                Util.WriteLog($"Get data from Pim success at page {pageNumber}");
                return result;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Get data from Pim failed with exception {ex.Message} at page {pageNumber}");
                return result;
            }

        }
    }
}
