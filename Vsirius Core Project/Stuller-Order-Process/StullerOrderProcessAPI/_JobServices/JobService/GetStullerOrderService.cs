using AutoMapper;
using Azure;
using MongoDB.Driver;
using Newtonsoft.Json;
using StullerOrderProcessAPI._JobServices.Interfaces;
using StullerOrderProcessAPI._Repositories.Interfaces;
using StullerOrderProcessAPI.Common.Helpers;
using StullerOrderProcessAPI.Dtos;
using StullerOrderProcessAPI.Models;
using StullerOrderProcessAPI.Models.MongoCollection;
using System.Text;

namespace StullerOrderProcessAPI._JobServices.JobService
{
    public class GetStullerOrderService : IGetStullerOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly IStullerOrderRepository _orderStullerRepository;
        private readonly AmazonIntegrationDataApi._Services.Interfaces.IOrderAmazonProcessor _amazonProcessor;
        private readonly WalmartIntegrationDataApi._Services.Interfaces.IOrderWalmartService _walmartOrder;
        private readonly IMapper _mapper;

        public GetStullerOrderService(
            IConfiguration configuration,
            AmazonIntegrationDataApi._Services.Interfaces.IOrderAmazonProcessor amazonProcessor,
            WalmartIntegrationDataApi._Services.Interfaces.IOrderWalmartService walmartOrder,
            IMapper mapper,
            IStullerOrderRepository orderStullerRepository)
        {
            _configuration = configuration;
            _amazonProcessor = amazonProcessor;
            _orderStullerRepository = orderStullerRepository;
            _walmartOrder = walmartOrder;
            _mapper = mapper;
        }

        public async Task GetStullerOrder()
        {
            try
            {
                List<UnshipOrderDto> amazonOrders = await GetAmazonOrders_RFT();
                List<UnshipOrderDto> walmartOrders = await GetWalmartOrders_RFT();

                List<UnshipOrderDto> lstStullerPoFromMarketplace = new();
                lstStullerPoFromMarketplace.AddRange(amazonOrders);
                lstStullerPoFromMarketplace.AddRange(walmartOrders);

                var lstStringPO = lstStullerPoFromMarketplace.Select(x => x.CustomerOrderId).ToList();

                var lstLocalOrderStuller = await _orderStullerRepository
                    .FindAll(NameCollection.OrderStuller,
                    Builders<StullerOrder>.Filter
                    .Where(x => lstStringPO.Contains(x.PO))
                    );

                List<OrderStatusStuller> lstOrderFromAPI = await GetStullerOrderFromApi(lstStringPO);         

                if (lstOrderFromAPI != null && lstOrderFromAPI.Any())
                {
                    lstOrderFromAPI = lstOrderFromAPI.GroupBy(item => item.PurchaseOrderNumber)
                          .SelectMany(group =>
                                         group.Any(item => item.Status != "Booked") ?
                                         group.Where(item => item.Status != "Booked") :
                                         group).ToList();

                    foreach (var itemApi in lstOrderFromAPI)
                    {
                        var orderLocal = lstLocalOrderStuller.FirstOrDefault(x => x.PO == itemApi.PurchaseOrderNumber);
                        var objOrderMarketplace = lstStullerPoFromMarketplace.FirstOrDefault(x => x.CustomerOrderId == itemApi.PurchaseOrderNumber);
                        if (orderLocal != null)
                        {
                            if(orderLocal.Status != itemApi.Status)
                            {
                                StullerOrder entity = Utils.ConvertResponseStullerOrderToEntity(itemApi, objOrderMarketplace);
                                entity.Marketplace = orderLocal.Marketplace;
                                entity.MarketplaceOrderId = orderLocal.MarketplaceOrderId;

                                await _orderStullerRepository.ReplaceOne(
                                    NameCollection.OrderStuller,
                                    entity,
                                    Builders<StullerOrder>.Filter.Eq(x => x.PO, itemApi.PurchaseOrderNumber)
                                    );
                            }                         
                        }
                        else
                        {
                            StullerOrder entity = Utils.ConvertResponseStullerOrderToEntity(itemApi, objOrderMarketplace);
                            entity.Marketplace = itemApi.PurchaseOrderNumber!.Contains('-') ? "Walmart" : "Amazon";
                            entity.MarketplaceOrderId = itemApi.PurchaseOrderNumber;
                            await _orderStullerRepository.InsertOne(entity, NameCollection.OrderStuller);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLog($"GetStullerOrder failed: {ex.Message}");
            }
        }

        private async Task<List<OrderStatusStuller>> GetStullerOrderFromApi(List<string?> lstPO)
        {
            try
            {
                var httpClient = new HttpClient();
                var _userStuller = _configuration.GetSection("Stuller:user").Value;
                var _passStuller = _configuration.GetSection("Stuller:password").Value;
                string credentials =
                       Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userStuller}:{_passStuller}"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
                var jsonContent = new StringContent(
                                        JsonConvert.SerializeObject(
                                            new { PurchaseOrderNumber = lstPO }
                                            ),
                                        Encoding.UTF8,
                                        "application/json"
                                        );
                var url = $"{_configuration["Stuller:OrdersApi"]}";
                var response = await httpClient.PostAsync(url, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<StullerOrderResponseDto>(content)!;

                    return rs.OrderStatuses!;
                }
                else
                {
                    Utils.WriteLog($"GetWalmartOrders failed: {response.StatusCode}, {response.ReasonPhrase}");
                    return new();
                }

            }
            catch (Exception ex)
            {
                Utils.WriteLog($"GetWalmartOrders {ex.Message}");
                return new();
            }
        }

        private async Task<List<UnshipOrderDto>> GetWalmartOrders()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration["WalmartAPI:GetOrders"]}?Provider=Stuller&FromDate={DateTime.Now.AddDays(-7).ToShortDateString()}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<UnshipOrderDto>>(content)!;
                }
                else
                {
                    Utils.WriteLog($"GetWalmartOrders failed: {response.StatusCode}, {response.ReasonPhrase}");
                    return new();
                }

            }
            catch (Exception ex)
            {
                Utils.WriteLog($"GetWalmartOrders {ex.Message}");
                return new();
            }
        }

        private async Task<List<UnshipOrderDto>> GetWalmartOrders_RFT()
        {
            var param = new WalmartIntegrationDataApi.Dtos.Order.UnshippedOrderIdsParam();
            var data = await _walmartOrder.GetUnshippedOrderIds(param);

            var result = new List<UnshipOrderDto>();
            foreach ( var item in data)
            {
                var dto = _mapper.Map<UnshipOrderDto>(item);
                result.Add(dto);
            }
            return result;
        }

        private async Task<List<UnshipOrderDto>> GetAmazonOrders_RFT()
        {
            try
            {
                var param = new AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor.UnshippedOrderIdsParam();
                param.Provider = "Stuller";
                param.FromDate = DateTime.Now.AddDays(-7).ToShortDateString();

                var data = await _amazonProcessor.GetUnshippedOrderIds(param);
                var rs = new List<UnshipOrderDto>();
                foreach (var item in data)
                {
                    var dto = _mapper.Map<UnshipOrderDto>(item);
                    rs.Add(dto);
                }
                return rs;
            }
            catch (Exception ex)
            {
                Utils.WriteLog($"GetAmazonOrders failed: {ex.ToString()}");
                return new();
            }
        }

        private async Task<List<UnshipOrderDto>> GetAmazonOrders()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration["AmazonAPI:GetOrders"]}?Provider=Stuller&FromDate={DateTime.Now.AddDays(-7).ToShortDateString()}";
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<UnshipOrderDto>>(content)!;
                }
                else
                {
                    Utils.WriteLog($"GetAmazonOrders failed: {response.StatusCode}, {response.ReasonPhrase}");
                    return new();
                }

            }
            catch (Exception ex)
            {
                Utils.WriteLog($"GetAmazonOrders {ex.Message}");
                return new();
            }
        }
        
    }
}
