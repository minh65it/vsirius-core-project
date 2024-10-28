using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using StullerOrderProcessAPI.Dtos;
using StullerOrderProcessAPI.Models;
using StullerOrderProcessAPI.Common;
using System.Text.RegularExpressions;
using StullerOrderProcessAPI.Common.Helpers;
using StullerOrderProcessAPI._Services.Interfaces;
using StullerOrderProcessAPI.Models.MongoCollection;
using StullerOrderProcessAPI._Repositories.Interfaces;
using StullerOrderProcessAPI._JobServices.JobService;

namespace StullerOrderProcessAPI._Services.Services
{
    public class OrderStullerService : IOrderStullerService
    {
        private readonly IConfiguration _configuration;
        private readonly IStullerOrderRepository _repository;

        private readonly bool _testmode;
        private readonly string? _accountStuller;
        private readonly string? _userStuller;
        private readonly string? _passStuller;

        public OrderStullerService(
            IConfiguration configuration,
            IStullerOrderRepository repository)
        {
            _configuration = configuration;

            _accountStuller = _configuration.GetSection("Stuller:accountNumber").Value;
            _userStuller = _configuration.GetSection("Stuller:user").Value;
            _passStuller = _configuration.GetSection("Stuller:password").Value;
            _testmode = bool.Parse(_configuration.GetSection("Stuller:TestMode").Value ?? "true");
            _repository = repository;
        }

        public async Task<bool> DeleteOrders(List<string> ds)
        {
            try
            {
                FilterDefinition<StullerOrder>? filter = Builders<StullerOrder>.Filter.Where(x => ds.Contains(x.OrderID));
                await _repository.DeleteMany(NameCollection.OrderStuller, filter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<StullerOrderDTO?> GetDetailOrder(string id)
        {
            FilterDefinition<StullerOrder>? filter = Builders<StullerOrder>.Filter.Where(x => x.OrderID == id);
            var dto = CustomMapper.ConvertOrderEntityToDto(await _repository.FisrtOrDefault(NameCollection.OrderStuller, filter));
            return dto;
        }

        public async Task<PaginationUtility<StullerOrderDTO>> GetOrders(
            PaginationParam paginationParam,
            SearchOrderStullerParam searchParam)
        {
            FilterDefinition<StullerOrder>? filter = null;

            if (searchParam != null)
            {
                if (!string.IsNullOrEmpty(searchParam.Status))
                {
                    Utils.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.Status == searchParam.Status)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.Marketplace))
                {
                    Utils.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.Marketplace == searchParam.Marketplace)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.MarketplaceOrderId))
                {
                    Utils.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.MarketplaceOrderId == searchParam.MarketplaceOrderId)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.OrderID))
                {
                    Utils.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.OrderID == searchParam.OrderID)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.BuyerName))
                {
                    Utils.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.BuyerName == searchParam.BuyerName)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.SKU))
                {
                    Utils.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Regex("Items.ProductSku",
                        new BsonRegularExpression(new Regex(Regex.Escape(searchParam.SKU), RegexOptions.IgnoreCase)))
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.FromDate) || !string.IsNullOrEmpty(searchParam.ToDate))
                {
                    var fromDate = DateTime.Parse(string.IsNullOrEmpty(searchParam.FromDate) ? DateTime.Now.AddDays(-30).ToString() : searchParam.FromDate);
                    var toDate = DateTime.Parse(string.IsNullOrEmpty(searchParam.ToDate) ? DateTime.Now.ToString() : searchParam.ToDate);

                    List<string> lstDate = GetTimeSegment(fromDate, toDate);

                    Utils.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => lstDate.Contains(x.OrderDate))
                        );
                }

            }

            SortDefinition<StullerOrder>? sort = Builders<StullerOrder>.Sort.Descending(x => x.UpdatedDate);

            var entityPagination = await _repository.Find(
                NameCollection.OrderStuller,
                filter,
                paginationParam.PageNumber,
                paginationParam.PageSize,
                sort);

            var result = new PaginationUtility<StullerOrderDTO>()
            {
                Pagination = new PaginationUtility<StullerOrderDTO>.PaginationResult()
                {
                    PageSize = paginationParam.PageSize,
                    PageNumber = paginationParam.PageNumber,
                    Skip = entityPagination.Pagination.Skip,
                    TotalCount = entityPagination.Pagination.TotalCount,
                    TotalPage = entityPagination.Pagination.TotalPage
                },
                Result = entityPagination.Result.Select(CustomMapper.ConvertOrderEntityToDto).ToList()
            };

            return result;
        }

        private List<string> GetTimeSegment(DateTime fromDate, DateTime toDate)
        {
            var result = new List<string>();
            for (DateTime i = fromDate; i <= toDate; i = i.AddDays(1))
            {
                result.Add(i.ToString("M/d/yyyy"));
            }
            return result;
        }

        public async Task<bool?> SubmitStuller(SubmitOrderParam param, bool isResubmit = false)
        {
            if (!isResubmit)
            {
                var isExsist = await _repository.Any(NameCollection.HistorySubmitStullerOrder,
                    Builders<SubmitStullerOrder>.Filter.Where(x => x.MarketplaceOrderId == param.MarketplaceOrderId));
                if (isExsist)
                {
                    return false;
                }
            }
            SubmitOrderDTO dto;
            SubmitStullerOrder entity = new()
            {
                StullerOrderId = string.Empty,
                MarketplaceOrderId = param.MarketplaceOrderId,
                Marketplace = param.Marketplace,
                Description = param.Description,
                Item = param.Item,
                ShipMethodCode = param.ShipMethodCode,
                ShipToAddress1 = param.ShipToAddress1,
                ShipToAddress2 = param.ShipToAddress2,
                ShipToAddress3 = param.ShipToAddress3,
                ShipToCity = param.ShipToCity,
                ShipToCountryCode = param.ShipToCountryCode,
                ShipToName = param.ShipToName,
                ShipToPhone = param.ShipToPhone,
                ShipToState = param.ShipToState,
                ShipToZip = param.ShipToZip,
                ShipToProvince = param.ShipToProvince,
                TimeStamp = DateTime.Now,
            };

            dto = new SubmitOrderDTO
            {
                OrderID = Guid.NewGuid().ToString(),
                //Version = 1.0,
                Type = "3pl",
                Account = _accountStuller,
                TestMode = _testmode,
                PurchaseOrderNumber = param.MarketplaceOrderId,
                Payment = new Payment()
                {
                    Type = "DefaultPayment"
                },
                CustomerData = new CustomerData()
                {
                    OrderNumber = param.MarketplaceOrderId,
                    OrderDate = DateTime.Now.Date,
                },
                Contact = new Contact()
                {
                    Name = "MICHAEL NGUYEN",
                    Phone = "(215) 500-2667",
                    EmailAddress = "VSIRIUS.INC@GMAIL.COM"
                },
                ShipToAddress = new ShipToAddress()
                {
                    ShipComplete = true,
                    ShipMethodType = param.ShipMethodCode,
                    RemovePricing = true,
                    SignatureRequired = false,
                    Address = new Address()
                    {
                        Name = param.ShipToName,
                        AddressLine1 = param.ShipToAddress1,
                        AddressLine2 = param.ShipToAddress2,
                        City = param.ShipToCity,
                        State = param.ShipToState,
                        PostalCode = param.ShipToZip,
                        Country = param.ShipToCountryCode,
                        Phone = param.ShipToPhone,
                        Province = param.ShipToProvince
                    }
                },
                BillToAddress = new BillToAddress()
                {
                    SameAsShipTo = false,
                    Address = new()
                    {
                        Name = "MICHAEL NGUYEN",
                        AddressLine1 = "630 CHURCH LN",
                        City = "LANSDOWNE",
                        State = "PA",
                        PostalCode = "19050-3389",
                        Country = "US"
                    }
                },

                Lines = new List<Line>()
            };
            foreach (var product in param.Item)
            {
                dto.Lines.Add(new Line()
                {
                    Item = product.Item,
                    Description = product.Item,
                    Quantity = int.Parse(product.Qty!),
                    Serial = new Serial()
                    {
                        Instructions = "",
                        Value = 0
                    }
                });
            }

            var isSucess = await CallApiStullerSubmit(dto);

            if (isSucess)
            {
                if (!isResubmit)
                {
                    await _repository.InsertOne(entity, NameCollection.HistorySubmitStullerOrder);
                }
                else
                {
                    // thuong la BO goi len de submit lai nhung order bi loi
                    await _repository.InsertOne(entity, NameCollection.ResubmitStullerOrder);
                }

                var order =
                    await _repository.FisrtOrDefault(
                        NameCollection.OrderStuller,
                        Builders<StullerOrder>.Filter.Eq(x => x.PO, dto.PurchaseOrderNumber));
                if (order == null)
                {
                    await _repository
                        .InsertOne(new StullerOrder()
                        {
                            PO = dto.PurchaseOrderNumber,
                            MarketplaceOrderId = dto.PurchaseOrderNumber,
                            Marketplace = param.Marketplace,
                            UpdatedDate = DateTime.Now.ToString("yyyyMMddhhmmssfff")
                        },
                        NameCollection.OrderStuller
                    );
                }
                else
                {
                    var collection = _repository.database.GetCollection<StullerOrder>(NameCollection.OrderStuller);
                    var filter = Builders<StullerOrder>.Filter.Where(_ => _.PO == param.MarketplaceOrderId);
                    var update = Builders<StullerOrder>.Update
                        .Set(_ => _.Marketplace, param.Marketplace)
                        .Set(_ => _.MarketplaceOrderId, param.MarketplaceOrderId);
                    var options = new FindOneAndUpdateOptions<StullerOrder>();
                    collection.FindOneAndUpdate(filter, update, options);
                }
            }

            return isSucess;
        }
        private async Task<bool> CallApiStullerSubmit(SubmitOrderDTO dto)
        {
            try
            {
                Utils.WriteLog($"Data submit: {JsonConvert.SerializeObject(dto)}");
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                string credentials =
                        Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userStuller}:{_passStuller}"));
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");
                var stringContent = JsonConvert.SerializeObject(dto);
                var content = new StringContent(stringContent, Encoding.UTF8, "application/json");

                var apiUrl = _configuration.GetSection("Stuller:SubmitApi").Value;


                var response = await httpClient.PostAsync(apiUrl, content);

                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    dynamic result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    if (result != null)
                    {
                        Utils.WriteLog($"Result submit:{JsonConvert.SerializeObject(dto)}.");
                        return (bool)result.Created;
                    }
                }
                else
                {
                    Utils.WriteLog($"Failed Submit with err: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLog($"Failed submit with err exception:{ex.Message}");
                return false;
            }

            return false;
        }

        public async Task<bool> MapOrder(List<MapOrderParam> param)
        {
            bool result = true;
            bool result2 = true;
            var filter = Builders<SubmitStullerOrder>.Filter.Where(x => x.StullerOrderId == null || x.StullerOrderId == "");
            var lstSubmit = await _repository.FindAll(NameCollection.HistorySubmitStullerOrder, filter);
            var lstReSubmit = await _repository.FindAll(NameCollection.ResubmitStullerOrder, filter);
            foreach (var item in param)
            {
                try
                {
                    var checkOrder = lstSubmit.Where(x => x.ShipToName.TrimStart().ToLower()[0] == item.ShipToName.TrimStart().ToLower()[0]
                       && x.ShipToAddress1.TrimStart().ToLower()[0] == item.ShipToAddress1.TrimStart().ToLower()[0]
                       && item.ShipToZip.Contains(x.ShipToZip, StringComparison.OrdinalIgnoreCase)
                       && (x.Item.Select(a => a.Item)).Any(b => b.Contains(SKUStullerFromUIPathCallMap(item.ItemSkus[0]), StringComparison.OrdinalIgnoreCase))
                       ).FirstOrDefault();

                    if (checkOrder is null)
                    {
                        continue;
                    }
                    checkOrder.StullerOrderId = item.OrderId;
                    await _repository.ReplaceOne(
                        NameCollection.HistorySubmitStullerOrder,
                        checkOrder,
                        Builders<SubmitStullerOrder>.Filter.Where(x => x._id == checkOrder._id));
                }
                catch (Exception ex)
                {
                    Utils.WriteLog($"MapOrder Order Via Name in SubmitHistory error: {ex}");
                    result = false;
                }
            }

            foreach (var item in param)
            {
                try
                {
                    var checkOrder = lstReSubmit.Where(x => x.ShipToName.TrimStart().ToLower()[0] == item.ShipToName.TrimStart().ToLower()[0]
                        && x.ShipToAddress1.TrimStart().ToLower()[0] == item.ShipToAddress1.TrimStart().ToLower()[0]
                        && item.ShipToZip.Contains(x.ShipToZip, StringComparison.OrdinalIgnoreCase)
                        && (x.Item.Select(a => a.Item)).Any(b => b.Contains(SKUStullerFromUIPathCallMap(item.ItemSkus[0]), StringComparison.OrdinalIgnoreCase))
                        ).FirstOrDefault();

                    if (checkOrder is null)
                    {
                        continue;
                    }
                    checkOrder.StullerOrderId = item.OrderId;
                    await _repository.ReplaceOne(
                        NameCollection.ResubmitStullerOrder,
                        checkOrder,
                        Builders<SubmitStullerOrder>.Filter.Where(x => x._id == checkOrder._id));
                }
                catch (Exception ex)
                {
                    Utils.WriteLog($"MapOrder Order Via Name in ReSubmit error: {ex.Message}");
                    result2 = false;
                }
            }

            return result && result2;
        }

        private static string SKUStullerFromUIPathCallMap(string sku)
        {
            string pattern = @"^(.*?)\r?\n";

            Match match = Regex.Match(sku, pattern);
            if (match.Success)
            {
                return match.Value.Trim();
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task<List<UnSubmittedStullerOrder>> GetOrderNeedToSubmit()
        {
            var rs = new List<UnSubmittedStullerOrder>();

            List<AmazonOrderDTO> lstAmazonOrder = await GetAmazonOrder();
            List<UnSubmittedStullerOrder> amazonToUnsubmit =
                lstAmazonOrder
                .Where(x => x.OrderItemList.Any(a => a.SellerSKU.StartsWith("S-")))
                .Select(ConvertAmazonToUnsubmitStuller).ToList();

            List<WalmartOrderDTO> lstWalmartOrder = await GetWalmartOrder();
            List<UnSubmittedStullerOrder> walmartToUnsubmit =
               lstWalmartOrder.Where(x => x.Items.Any(a => a.Sku.StartsWith("S-")))
               .GroupBy(x => new
               {
                   x.customerOrderId,
                   x.Address1,
                   x.Address2,
                   x.City,
                   x.methodShipping,
                   x.BuyerName,
                   x.State,
                   x.PostalCode
               }, x => x.Items,
               (key, group) => new UnSubmittedStullerOrder()
               {
                   PO = key.customerOrderId,
                   Address1 = key.Address1,
                   Address2 = key.Address2,
                   City = key.City,
                   FullName = key.BuyerName,
                   ShipMethod = key.methodShipping,
                   State = key.State,
                   Zip = key.PostalCode,
                   Items = group.Select(x => new UnSubmitItemStuller()
                   {
                       Sku = x.First().Sku,
                       Quantity = x.Count().ToString()
                   }).ToList()
                   //ConvertWalmartToUnsubmitStuller(group)
               }).ToList();

            List<EbayOrderDto> lstEbayOrder = await GetEbayOrder();
            List<UnSubmittedStullerOrder> ebayToUnsubmit =
               lstEbayOrder.Where(x => x.IsSubmit == false 
               && x.OrderDetails.Any(a => a.Sku.StartsWith("S-")))
               .Select(ConvertEbayToUnsubmitStuller).ToList();

            List<StullerOrder> lstStuller = await _repository.FindAll<StullerOrder>(NameCollection.OrderStuller);
            List<SubmitStullerOrder> lstSubmitStuller = await _repository
                .FindAll(NameCollection.HistorySubmitStullerOrder,
                Builders<SubmitStullerOrder>.Filter.Gt(x => x.TimeStamp, DateTime.Now.AddDays(-4)));

            List<UnSubmittedStullerOrder> lstUnSubmittedOrder = new();
            lstUnSubmittedOrder.AddRange(amazonToUnsubmit ?? new());
            lstUnSubmittedOrder.AddRange(walmartToUnsubmit ?? new());
            lstUnSubmittedOrder.AddRange(ebayToUnsubmit ?? new());

            rs = lstUnSubmittedOrder
                .Where(x => !lstStuller.Select(a => a.PO).Contains(x.PO)
                && !lstSubmitStuller.Select(a => a.MarketplaceOrderId).Contains(x.PO))
                .ToList();

            return rs;
        }

        private async Task<List<EbayOrderDto>> GetEbayOrder()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration["EbayAPI:GetOrders"]}?pagination.pageSize=100";

                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<PaginationUtility<EbayOrderDto>>(content);
                    List<EbayOrderDto> lstEbayOrder = rs?.Result ?? new();
                    return lstEbayOrder;
                }
                else
                {
                    return new();
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLog($"Error when get list submit from Amazon: {ex.Message}");
                return new();
            }
        }

        private UnSubmittedStullerOrder ConvertEbayToUnsubmitStuller(EbayOrderDto item)
        {
            return new()
            {
                PO = item.OrderId,
                Address1 = item.Address1,
                Address2 = item.Address2,
                City = item.City,
                FullName = item.CustomerName,
                ShipMethod = "Standard",
                State = item.State,
                Zip = item.ZipCode,
                Items = item.OrderDetails
                .Select(x => new UnSubmitItemStuller()
                {
                    Sku = x.Sku,
                    Quantity = x.Quantity.ToString()
                }).ToList()
            };
        }

        private UnSubmittedStullerOrder ConvertAmazonToUnsubmitStuller(AmazonOrderDTO item)
        {
            return new()
            {
                PO = item.AmazonOrderId,
                Address1 = item.AddressLine1,
                Address2 = item.AddressLine2,
                City = item.City,
                FullName = item.BuyerName,
                ShipMethod = item.ShipmentServiceLevelCategory,
                State = item.StateOrRegion,
                Zip = GetZipFromAmazon(item.PostalCode),
                Items = item.OrderItemList
                .Select(x => new UnSubmitItemStuller()
                {
                    Sku = x.SellerSKU,
                    Quantity = x.QuantityOrdered.ToString()
                }).ToList()
            };
        }

        private string GetZipFromAmazon(string? postalCode)
        {
            int indexWord = postalCode.IndexOf('-');

            if (indexWord != -1)
            {
                return postalCode[..indexWord];
            }
            else
            {
                return postalCode;
            }
        }

        private async Task<List<WalmartOrderDTO>> GetWalmartOrder()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration["WalmartAPI:GetSubmitOrders"]}?PageSize=50&OrderStatus=1";

                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<PaginationUtility<WalmartOrderDTO>>(content);

                    List<WalmartOrderDTO> lstWalmartOrder = rs?.Result ?? new();
                    return lstWalmartOrder;
                }
                else
                {
                    return new();
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLog($"Error when get list submit from Amazon: {ex.Message}");
                return new();
            }
        }

        private async Task<List<AmazonOrderDTO>> GetAmazonOrder()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = $"{_configuration["AmazonAPI:GetSubmitOrders"]}?PageSize=50";

                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var rs = JsonConvert.DeserializeObject<PaginationUtility1<AmazonOrderDTO>>(content);

                    List<AmazonOrderDTO> lstAmazonOrder = rs?.Data ?? new();

                    lstAmazonOrder = lstAmazonOrder
                        .Where(x => x.OrderStatus == "Unshipped")
                        .ToList();

                    return lstAmazonOrder;
                }
                else
                {
                    return new();
                }
            }
            catch (Exception ex)
            {
                Utils.WriteLog($"Error when get list submit from Amazon: {ex.Message}");
                return new();
            }
        }
    }
}
