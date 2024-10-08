using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Parameters.ListingsByShop;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class ListingService : IListingService
    {
        private readonly ILoggerCustom _logService;
        private readonly IConfiguration _config;
        private readonly IAuthenticationEtsyService _authenticationEtsyService;
        private readonly IProductEtsyService _productEtsyService;
        private readonly IListingImageService _listingImageService;
        private readonly ITopOrderService _topOrderService;
        private readonly IProductEtsyRepository _productEstyRepository;
        private readonly bool _isProduction;
        private readonly string _urlCreateDraftListing;
        private readonly string _urlFindAllListings;
        private readonly string _xApiKey;
        private readonly string _contentType = "application/x-www-form-urlencoded";

        public ListingService(ILoggerCustom logService, IConfiguration config,
            IAuthenticationEtsyService authenticationEtsyService,
            IProductEtsyService productEtsyService, IListingImageService listingImageService,
            ITopOrderService topOrderService, IProductEtsyRepository productEstyRepository)
        {
            _logService = logService;
            _config = config;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
            _urlCreateDraftListing = $"{_config.GetSection("Etsy:OpenAPI").Value}/shops/{_config.GetSection("Etsy:ShopId").Value}/listings";
            _urlFindAllListings = $"{_config.GetSection("Etsy:OpenAPI").Value}/shops/{_config.GetSection("Etsy:ShopId").Value}/listings";
            _xApiKey = $"{_config.GetSection("Etsy:KeysString").Value}";
            _authenticationEtsyService = authenticationEtsyService;
            _productEtsyService = productEtsyService;
            _contentType = "application/x-www-form-urlencoded";
            _listingImageService = listingImageService;
            _topOrderService = topOrderService;
            _productEstyRepository = productEstyRepository;
        }

        public async Task<ProductEtsyDto> CreateDraftListing(string authorization, ProductEtsyDto dto)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
                client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                var jsonString = JsonConvert.SerializeObject(dto);
                var dict = new Dictionary<string, string>();

                foreach (var kvp in JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString))
                {
                    if (kvp.Key != nameof(ProductEtsyDto.user_id) && kvp.Key != nameof(ProductEtsyDto.listing_id) &&
                        kvp.Key != nameof(ProductEtsyDto.shop_id) && kvp.Key != nameof(ProductEtsyDto.item_sku) &&
                        kvp.Key != nameof(ProductEtsyDto.createdDate) && kvp.Key != nameof(ProductEtsyDto.updatedDate))
                    {
                        if (kvp.Key == nameof(ProductEtsyDto.price))
                        {
                            var data = ((JObject)kvp.Value).ToObject<PriceDto>();
                            dict[kvp.Key] = data.amount.ToString();
                        }
                        else if (kvp.Key == nameof(ProductEtsyDto.image_ids))
                        {
                            dict[kvp.Key] = null;
                        }
                        else if (kvp.Key == nameof(ProductEtsyDto.tags) || kvp.Key == nameof(ProductEtsyDto.styles) || kvp.Key == nameof(ProductEtsyDto.materials))
                        {
                            if (kvp.Value is IEnumerable<object> array)
                            {
                                dict[kvp.Key] = string.Join(",", array.Select(obj => obj?.ToString()));
                            }
                        }
                        else if (kvp.Value != null)
                        {
                            dict[kvp.Key] = kvp.Value?.ToString();
                        }
                    }
                }
                dict.Remove("image_ids");
                dict = dict.Where(f => !string.IsNullOrWhiteSpace(f.Value)).ToDictionary(x => x.Key, x => x.Value);

                var requestContent = new FormUrlEncodedContent(dict);
                HttpResponseMessage response = await client.PostAsync(_urlCreateDraftListing, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ProductEtsyDto result = JsonConvert.DeserializeObject<ProductEtsyDto>(responseBody)!;
                    result.item_sku = dto.item_sku;
                    await _productEtsyService.UpdateListingIdProduct(result);
                    return result;
                }

            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: ListingService, Action: CreateDraftListing", ex.ToString());
                return null;
            }
            return null;
        }

        public async Task<OperationResult> CreateListDraftAndPublishListing(int limits)
        {
            var refreshToken = await _authenticationEtsyService.RefreshToken(null);
            if (refreshToken == null)
            {
                await _logService.CreateLogError($"Service: ListingService, Action: CreateListDraftListing. No Authorization.");
                return new OperationResult { Success = false, Message = "No Authorization" };
            }
            string authorization = $"Bearer {refreshToken.Access_token}";

            var dataEtsy = await GetAllListingsByShop(authorization);
            var dataDB = await _productEtsyService.GetAllData(null, true, false);
            if (dataDB == null)
            {
                await _logService.CreateLogWarning("CreateListDraftAndPublishListing No data in Database");
                return new OperationResult { Success = false, Message = "No data in Database" };
            }


            List<long> skuUpload = dataDB.Select(x => x.listing_id).ToList();
            if (dataEtsy != null)
            {
                List<long> checkDataEtsy = dataEtsy.Select(x => x.listing_id).ToList();
                skuUpload = skuUpload.Except(checkDataEtsy).ToList();
            }

            dataDB = dataDB.Where(x => skuUpload.Contains(x.listing_id)).ToList();
            // top Order
            var dataTop = await _topOrderService.GetTopListProduct();
            if (dataTop != null)
            {
                var dataDBTmp = dataDB.Where(x => dataTop.Contains(x.item_sku)).ToList();
                if (dataDBTmp.Count > 0)
                {
                    dataDB = dataDBTmp;
                }
            }

            if (dataDB == null || dataDB.Count == 0)
            {
                return new OperationResult { Success = false, Message = "No for CreateListDraft" };
            }

            bool checkToken = false;

            int count = 1;
            foreach (var item in dataDB)
            {
                try
                {
                    count++;
                    var checkDto = await CreateDraftListing(authorization, item);
                    if (checkDto == null && !checkToken)
                    {
                        var refreshTokenAgain = await _authenticationEtsyService.RefreshToken(refreshToken.Refresh_token);
                        authorization = $"Bearer {refreshTokenAgain.Access_token}";
                        checkToken = true;
                    }
                    else
                    {
                        var checkImg = await _listingImageService.UploadListingImage(authorization, checkDto.listing_id);
                        if (_isProduction && checkImg != null)
                        {
                            await UpdateListing(authorization, checkDto);
                        }
                    }
                    if (count >= limits)
                        break;
                }
                catch (Exception ex)
                {
                    if (count >= limits)
                        break;
                }

            }
            return new OperationResult { Success = true };
        }

        public Task<List<ProductEtsyDto>> GetListingsById(GetListingParams search)
        {
            //Call API
            return null;
        }

        public async Task<List<ProductEtsyDto>> GetAllListingsByShop(string authorization)
        {
            List<ProductEtsyDto> results = new List<ProductEtsyDto>();
            try
            {
                int limit = 100;
                var dataFirstCall = await GetPaginationListingsByShop(authorization, limit, 0, "active");
                if (dataFirstCall != null)
                {
                    results.AddRange(dataFirstCall.results);
                    int totalPage = dataFirstCall.count / limit;
                    if (totalPage > 0)
                    {
                        for (int i = 1; i <= totalPage; i++)
                        {
                            var data = await GetPaginationListingsByShop(authorization, limit, i * limit, "active");
                            results.AddRange(data.results);
                        }
                    }
                }
                return results;
            }
            catch (Exception)
            {
                await _logService.CreateLogError($"Service: ListingService, Action: GetAllListingsByShop");
                return results;
            }
        }
        public async Task<AllListings> GetPaginationListingsByShop(string authorization, int limit, int offset, string state)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
            client.DefaultRequestHeaders.Add("Authorization", authorization);
            string url = _urlFindAllListings + $"?state={state}&limit={limit}&offset={offset}";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    AllListings results = JsonConvert.DeserializeObject<AllListings>(responseBody)!;
                    return results;
                }

                return null;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: ListingService, action: GetPaginationListingsByShop, {ex.ToString()}");
                return null;
            }
        }

        public async Task<ProductEtsyDto> UpdateListing(string authorization, ProductEtsyDto dto)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
                client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                var jsonString = JsonConvert.SerializeObject(dto);
                var dict = new Dictionary<string, string>();

                //Update Listing cũng có thể update lại các field giống như hàm CreateDraftListing();
                dict.Add("state", "active");
                var requestContent = new FormUrlEncodedContent(dict);
                HttpResponseMessage response = await client.PatchAsync($"{_urlCreateDraftListing}/{dto.listing_id}", requestContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ProductEtsyDto result = JsonConvert.DeserializeObject<ProductEtsyDto>(responseBody)!;
                    await _productEtsyService.UpdateListingIdProduct(result);
                    return result;
                }
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: ListingService, Action: UpdateListing", ex.ToString());
                return null;
            }
            return null;
        }
        public async Task<OperationResult> SyncEtsyAndDatabase()
        {
            var refreshToken = await _authenticationEtsyService.RefreshToken(null);
            if (refreshToken == null)
            {
                await _logService.CreateLogError($"Service: ListingService, Action: CreateListDraftListing. No Authorization.");
                return new OperationResult { Success = false, Message = "No Authorization" };
            }
            string authorization = $"Bearer {refreshToken.Access_token}";

            var dataEtsy = await GetAllListingsByShop(authorization);
            if (dataEtsy == null)
            {
                return new OperationResult { Success = false, Message = "No data in Etsy" };
            }
            var dataDB = await _productEstyRepository.FindAll(x => x.listing_id == 0).ToListAsync();
            if (dataDB == null)
            {
                await _logService.CreateLogWarning("SyncEtsyAndDatabase No data in Database");
                return new OperationResult { Success = false, Message = "No data in Database" };
            }
            List<ProductEtsy> lstUpdate = new List<ProductEtsy>();
            foreach (var item in dataEtsy)
            {
                var dataAdd = dataDB.FirstOrDefault(s => s.title == item.title);
                if (dataAdd != null)
                {
                    dataAdd.listing_id = item.listing_id;
                    lstUpdate.Add(dataAdd);
                }
            }
            try
            {
                _productEstyRepository.UpdateMultiple(lstUpdate);
                await _productEstyRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "SyncEtsyAndDatabase done" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogWarning($"SyncEtsyAndDatabase error: {ex.ToString()}");
                return new OperationResult { Success = false, Message = "SyncEtsyAndDatabase failed" };
            }
        }
    }
}
