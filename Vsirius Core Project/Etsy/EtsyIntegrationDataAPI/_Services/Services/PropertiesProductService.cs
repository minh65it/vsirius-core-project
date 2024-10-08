using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Dtos.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Dtos.PropertiesByTaxonomy;
using EtsyIntegrationDataAPI.Dtos.PropertiesEtsy;
using EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Net.Http.Headers;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class PropertiesProductService : IPropertiesProductService
    {
        private readonly IPropertiesByTaxonomyRepository _propertiesByTaxonomyRepository;
        private readonly IPropertiesProductRepository _propertiesProductRepository;
        private readonly IAuthenticationEtsyService _authenticationEtsyService;
        private readonly IProductEtsyRepository _productEtsyRepository;
        private readonly IListingService _listingService;
        private readonly ILoggerCustom _logService;
        private readonly IConfiguration _config;
        private readonly bool _isProduction;
        private readonly string _urlGet;
        private readonly string _urlPutProperties;
        private readonly string _xApiKey;
        private readonly string _contentType = "application/x-www-form-urlencoded";

        public PropertiesProductService(IPropertiesByTaxonomyRepository propertiesByTaxonomyRepository, ILoggerCustom logService,
            IConfiguration config, IPropertiesProductRepository propertiesProductRepository,
            IAuthenticationEtsyService authenticationEtsyService, IListingService listingService, IProductEtsyRepository productEtsyRepository)
        {
            _propertiesByTaxonomyRepository = propertiesByTaxonomyRepository;
            _logService = logService;
            _config = config;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
            _urlGet = $"{_config.GetSection("Etsy:OpenAPI").Value}/seller-taxonomy/nodes/";
            _urlPutProperties = $"{_config.GetSection("Etsy:OpenAPI").Value}/shops/{_config.GetSection("Etsy:ShopId").Value}/listings/";
            _xApiKey = $"{_config.GetSection("Etsy:KeysString").Value}";
            _propertiesProductRepository = propertiesProductRepository;
            _authenticationEtsyService = authenticationEtsyService;
            _listingService = listingService;
            _contentType = "application/x-www-form-urlencoded";
            _productEtsyRepository = productEtsyRepository;
        }

        public async Task<OperationResult> SeedDataPropertiesByTaxonomy(int taxonomy_id)
        {
            try
            {
                if (taxonomy_id == null)
                    return new OperationResult() { Success = false, Message = "No taxonomy_id" };
                var dataSeed = await CallAPIGetProperties(taxonomy_id);
                if (dataSeed != null)
                {
                    var data = _propertiesByTaxonomyRepository.FindAll().ToList();

                    _propertiesByTaxonomyRepository.RemoveMultiple(data);
                    _propertiesByTaxonomyRepository.AddMultiple(dataSeed);

                    await _propertiesByTaxonomyRepository.SaveChangesAsync();
                    return new OperationResult() { Success = true };
                }
                return new OperationResult() { Success = false, Message = "No data" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: PropertiesByTaxonomyService, Action: SeedData", ex.ToString());
                return new OperationResult() { Success = false, Message = ex.ToString() };
            }
        }

        public async Task<OperationResult> UpdateListingPropertiesEtsy()
        {
            var refreshToken = await _authenticationEtsyService.RefreshToken(null);
            if (refreshToken == null)
            {
                await _logService.CreateLogError($"Service: ListingService, Action: CreateListDraftListing. No Authorization.");
                return new OperationResult { Success = false, Message = "No Authorization" };
            }
            string authorization = $"Bearer {refreshToken.Access_token}";

            if (true)
            {
                var dataEtsy = await _listingService.GetAllListingsByShop(authorization);
                if (dataEtsy.Any())
                {
                    var dataProduct = _productEtsyRepository.FindAll(x => dataEtsy.Select(s => s.listing_id).ToList().Contains(x.listing_id));
                    var dataProperties = _propertiesProductRepository.FindAll();
                    var dataUpdate = await dataProperties.Join(dataProduct,
                        x => x.item_sku, y => y.item_sku, (x, y) => new PropertiesProductDto()
                        {
                            item_sku = x.item_sku,
                            property_id = x.property_id,
                            value_id = x.value_id,
                            name = x.name,
                            scale_id = x.scale_id,
                            listing_id = y.listing_id
                        }).ToListAsync();
                    foreach (var item in dataEtsy)
                    {
                        var checkData = dataUpdate.Where(s => s.listing_id == item.listing_id).ToList();
                        if (checkData.Any())
                        {
                            await CallAPIPutProperties(authorization, checkData);
                        }
                    }
                    return new OperationResult() { Success = true, Message = "Update done" };
                }
            }
            return new OperationResult() { Success = false, Message = "No data" };
        }

        public async Task<OperationResult> UpdatePropertiesProduct(List<ProductPIMDto> products)
        {
            try
            {
                if (products.Any())
                {
                    List<PropertiesProduct> lstAdd = new List<PropertiesProduct>();
                    var dataRemove = await _propertiesProductRepository.FindAll(x => products.Select(s => s.item_sku).ToList().Contains(x.item_sku)).ToListAsync();
                    _propertiesProductRepository.RemoveMultiple(dataRemove);

                    var dataPropertiesByTaxonomy = _propertiesByTaxonomyRepository.FindAll().ToList();

                    //Check products
                    foreach (var item in products)
                    {
                        int taxonomy = ConvertPIMToEtsyDB.TaxonomyId(item?.feed_product_type);
                        //add ring size
                        var dataAdd = dataPropertiesByTaxonomy.FirstOrDefault(x => x.taxonomy_id == taxonomy && x.display_name == "Ring size" && x.name == item.ring_size);
                        if (dataAdd != null)
                        {
                            lstAdd.Add(new PropertiesProduct()
                            {
                                item_sku = item.item_sku,
                                name = dataAdd.name,
                                property_id = dataAdd.property_id,
                                scale_id = dataAdd.scale_id,
                                taxonomy_id = taxonomy,
                                value_id = dataAdd.value_id,

                            });
                        }
                    }
                    _propertiesProductRepository.AddMultiple(lstAdd);
                    await _propertiesProductRepository.SaveChangesAsync();
                    return new OperationResult() { Success = true };

                }
                return new OperationResult() { Success = false, Message = "No data" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: PropertiesByTaxonomyService, Action: UpdatePropertiesProduct", ex.ToString());
                return new OperationResult() { Success = false, Message = ex.ToString() };
            }


        }

        private async Task<bool> CallAPIPutProperties(string authorization, List<PropertiesProductDto> properties)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
                client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
                client.DefaultRequestHeaders.Add("Authorization", authorization);
                var dict = new Dictionary<string, string>();

                dict.Add("value_ids", string.Join(",", properties.Select(s => s.value_id).ToList()));
                dict.Add("values", string.Join(",", properties.Select(s => s.name).ToList()));
                var scale_id = properties.Where(x => x.scale_id != null).Select(s => s.scale_id).ToList();
                dict.Add("scale_id", string.Join(",", scale_id));
                var requestContent = new FormUrlEncodedContent(dict);
                HttpResponseMessage response = await client.PutAsync($"{_urlPutProperties}/{properties[0].listing_id}/properties/{properties[0].property_id}", requestContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: PropertiesByTaxonomyService, Action: CallAPIPutProperties", ex.ToString());
                return false;
            }
        }
        private async Task<List<PropertiesByTaxonomy>> CallAPIGetProperties(long taxonomy_id)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
                var url = _urlGet + $"{taxonomy_id}/properties";
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    PropertiesEtsyDto result = JsonConvert.DeserializeObject<PropertiesEtsyDto>(responseBody)!;
                    List<PropertiesByTaxonomy> res = new List<PropertiesByTaxonomy>();
                    foreach (var item in result.results)
                    {
                        if (item != null && item.possible_values != null)
                        {
                            foreach (var value in item.possible_values)
                            {
                                res.Add(new PropertiesByTaxonomy()
                                {
                                    taxonomy_id = taxonomy_id,
                                    property_id = item.property_id,
                                    display_name = item.display_name,
                                    name = value.name,
                                    scale_id = value.scale_id,
                                    value_id = value.value_id,

                                });
                            }
                        }
                    }
                    return res;
                }
                return null;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: PropertiesByTaxonomyService, Action: CallAPI", ex.ToString());
                return null;
            }
        }
    }
}
