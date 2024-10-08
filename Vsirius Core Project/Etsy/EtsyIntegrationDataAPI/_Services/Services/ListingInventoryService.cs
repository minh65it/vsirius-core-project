using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Utilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class ListingInventoryService : IListingInventoryService
    {
        private readonly ILoggerCustom _logService;
        private readonly IConfiguration _config;
        private readonly IAuthenticationEtsyService _authenticationEtsyService;
        private readonly IProductEtsyService _productEtsyService;
        private readonly IListingService _listingService;
        private readonly IProductEtsyRepository _productEtsyRepository;
        private readonly bool _isProduction;
        private readonly string _updateListingInventory;
        private readonly string _xApiKey;
        private readonly string _contentType = "application/json";

        public ListingInventoryService(ILoggerCustom logService, IConfiguration config, IAuthenticationEtsyService authenticationEtsyService,
            IProductEtsyService productEtsyService, IListingService listingService,
            IProductEtsyRepository productEtsyRepository)
        {
            _logService = logService;
            _config = config;
            _authenticationEtsyService = authenticationEtsyService;
            _productEtsyService = productEtsyService;
            _listingService = listingService;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
            _updateListingInventory = $"{_config.GetSection("Etsy:OpenAPI").Value}/listings";
            _xApiKey = $"{_config.GetSection("Etsy:KeysString").Value}";
            _contentType = "application/json";
            _productEtsyRepository = productEtsyRepository;
        }


        public async Task<OperationResult> UpdateAllListingInventory()
        {
            try
            {
                var refreshToken = await _authenticationEtsyService.RefreshToken(null);
                if (refreshToken == null)
                {
                    await _logService.CreateLogError($"Service: ListingInventoryService, Action: UpdateAllListingInventory. No Authorization.");
                    return new OperationResult() { Success = false, Message = "Update Inventory, price No Authorization." };
                }
                string authorization = $"Bearer {refreshToken.Access_token}";

                List<ProductEtsyDto> data = await _listingService.GetAllListingsByShop(authorization);
                if (data == null || data.Count() == 0)
                {
                    return new OperationResult() { Success = false, Message = "No date in Etsy" };
                }

                bool checkToken = false;
                foreach (var item in data)
                {
                    var check = await UpdateListingInventory(authorization, item.listing_id);
                    if (check != null && !check.Success && !checkToken)
                    {
                        var refreshTokenAgain = await _authenticationEtsyService.RefreshToken(refreshToken.Refresh_token);
                        authorization = $"Bearer {refreshTokenAgain.Access_token}";
                        checkToken = true;
                    }
                }

                return new OperationResult() { Success = true, Message = "Update Inventory, price done" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: ListingInventoryService, action: UpdateAllListingInventory, {ex.ToString()}");
                return new OperationResult() { Success = false, Message = $"{ex.ToString()}" };
            }
        }

        public async Task<OperationResult> UpdateListingInventory(string authorization, long listing_id)
        {
            try
            {
                if (!_isProduction)
                {
                    return new OperationResult() { Success = false };
                }
                var data = await _productEtsyRepository.FindSingle(x => x.listing_id == listing_id);
                ListingInventoryDto listingInventory = new ListingInventoryDto()
                {
                    products = new List<Products>()
                {
                    new Products()
                    {
                        sku = data?.item_sku,
                        offerings = new List<Offering>
                        {
                            new Offering()
                            {
                                is_enabled = data?.quantity > 0 ? true : false,
                                price = data.price,
                                quantity = data != null ? data.quantity : 0,
                            }
                        }
                    }
                }
                };

                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
                client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                var jsonString = JsonConvert.SerializeObject(listingInventory);
                StringContent httpContent = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync($"{_updateListingInventory}/{listing_id}/inventory", httpContent);
                if (response.IsSuccessStatusCode)
                {
                    //string responseBody = await response.Content.ReadAsStringAsync();
                    //ListingInventoryResponseDto result = JsonConvert.DeserializeObject<ListingInventoryResponseDto>(responseBody)!;
                    return new OperationResult() { Success = true };
                }
                return new OperationResult() { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: ListingInventoryService, action: UpdateListingInventory, {ex.ToString()}");
                return new OperationResult() { Success = false };
            }

        }
    }
}
