using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.BulkExtensions;
using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos;
using EtsyIntegrationDataAPI.Dtos.OrderEtsy;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Helpers;
using EtsyIntegrationDataAPI.Helpers.OrderEtsy;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Parameters.OrderParmas;
using EtsyIntegrationDataAPI.Utilities;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using NetTopologySuite.Index.HPRtree;
using Newtonsoft.Json;
using System.Linq;
using System.Net.Http.Headers;
using VSiriusSystemLog.WriteLog;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly DataContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IAuthenticationEtsyService _authenticationEtsyService;
        private readonly IInsertOrder _insertOrder;
        private readonly IMapper _mapper;
        private readonly ILoggerCustom _logService;
        private readonly IConfiguration _config;
        private readonly IQgoldApiClient _qgoldApiClient;
        private readonly IStullerApiClient _stullerApiClient;
        private readonly bool _isProduction;
        private readonly string _urlShopReceipts;
        private readonly string _xApiKey;
        private readonly string _contentType = "application/x-www-form-urlencoded";

        public OrderService(MapperConfiguration mapperConfiguration, IOrderRepository orderRepository,
            IMapper mapper, ILoggerCustom logService,
            IConfiguration config, IAuthenticationEtsyService authenticationEtsyService,
            DataContext context, IInsertOrder insertOrder, IQgoldApiClient qgoldApiClient,
            IStullerApiClient stullerApiClient)
        {
            _mapperConfiguration = mapperConfiguration;
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logService = logService;
            _config = config;
            _urlShopReceipts = $"{_config.GetSection("Etsy:OpenAPI").Value}/shops/{_config.GetSection("Etsy:ShopId").Value}/receipts";
            _authenticationEtsyService = authenticationEtsyService;
            _xApiKey = $"{_config.GetSection("Etsy:KeysString").Value}";
            _context = context;
            _insertOrder = insertOrder;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
            _qgoldApiClient = qgoldApiClient;
            _stullerApiClient = stullerApiClient;
        }

        public async Task<OrderDto> GetOrderDetail(long orderId)
        {
            var firstData = await _orderRepository.FindSingle(f => f.receipt_id == orderId,
                s => s.transactions,
                t => t.shipments,
                fo => fo.refunds);
            var listProduct = firstData.transactions.Select(s => s.sku);
            bool checkStuller = listProduct.Any(item => item!.StartsWith("S"));
            bool checkQgold = listProduct.Any(item => item!.StartsWith("Q"));
            
            var data = _mapper.Map<OrderDto>(firstData);
            if (checkStuller && checkQgold)
            {
                data.supplier = "Stuller, Qgold";
            }
            else
            {
                data.supplier = checkStuller == true ? "Stuller" : "Qgold";
            }
            return data;
        }

        public PaginationUtil<OrderDto> GetAllOrderEtsy(SearchOrderParam searchParam, PaginationParam pagination)
        {
            var firstData = _orderRepository.FindAll(f => f.transactions);
            var rs = new List<OrderDto>();
            var fastCheck = typeof(SearchOrderParam).GetProperties().All(p => p.GetValue(searchParam) == null);
            if (!fastCheck)
            {
                if (!string.IsNullOrWhiteSpace(searchParam.ids))
                {
                    firstData = firstData.Where(x => x.receipt_id.ToString().Contains(searchParam.ids));
                }
                try
                {
                    if(!string.IsNullOrEmpty(searchParam.name))
                    {
                            firstData = firstData.Where(n => n.name!.Contains(searchParam.name));
                    }
                    if(!string.IsNullOrEmpty(searchParam.fromDate) && !string.IsNullOrEmpty(searchParam.toDate))
                    {
                        DateTime fromDate = DateTime.Parse(searchParam.fromDate);
                        DateTime toDate = DateTime.Parse(searchParam.toDate);

                        long fromTimestamp = (long)(fromDate - new DateTime(1970, 1, 1)).TotalSeconds;
                        long toTimestamp = (long)(toDate - new DateTime(1970, 1, 1)).TotalSeconds;

                        firstData = firstData.Where(c => c.created_timestamp >= fromTimestamp && c.created_timestamp <= toTimestamp);
                    }

                    if(!string.IsNullOrEmpty(searchParam.buyer_email))
                    {
                        firstData = firstData.Where(b => b.buyer_email!.Contains(searchParam.buyer_email));
                    }
                    if (!string.IsNullOrEmpty(searchParam.address))
                    {
                        firstData = firstData.Where(b => b.formatted_address!.Contains(searchParam.address));
                    }
                    if (searchParam.is_paid != null || searchParam.is_shipped != null)
                    {
                        firstData = firstData.Where(i =>
                            (searchParam.is_paid == null || i.is_paid == searchParam.is_paid) &&
                            (searchParam.is_shipped == null || i.is_shipped == searchParam.is_shipped));
                    }
                }
                catch (Exception)
                {
                    return PaginationUtil<OrderDto>.Create(
                        rs.OrderBy(o => o.create_timestamp).ToList(),
                        pagination.PageNumber,
                        pagination.PageSize,
                        searchParam != null
                     );
                }
            }

            foreach (var item in firstData)
            {
                OrderDto o = _mapper.Map<OrderDto>(item);

                var listProduct = item.transactions.Select(s => s.sku) ;
                if(!listProduct.IsNullOrEmpty())
                {
                    bool checkStuller = listProduct.Any(item => item!.StartsWith("S"));
                    bool checkQgold = listProduct.Any(item => item!.StartsWith("Q"));
                    if (checkStuller && checkQgold)
                    {
                        o.supplier = "Stuller, Qgold";
                    }
                    else
                    {
                        o.supplier = checkStuller == true ? "Stuller" : "Qgold";
                    }
                }
                
                rs.Add(o);
            }
            

            return PaginationUtil<OrderDto>.Create(
                        rs,
                        pagination.PageNumber,
                        pagination.PageSize,
                        searchParam != null
                     );
        }


        public async Task<List<OrderDto>> GetShopReceipts(GetShopReceiptsParams? queries, int? pageNumber)
        {
            var refreshToken = await _authenticationEtsyService.RefreshToken(null);
            if (refreshToken == null)
            {
                await _logService.CreateLogError($"Service: OrderService, Action: GetShopReceipts. No Authorization.");
                return null;
            }
            string authorization = $"Bearer {refreshToken.Access_token}";
            List<OrderDto> results = new List<OrderDto>();
            int limit = 100;
            if (queries == null)
            {
                queries = new GetShopReceiptsParams();
            }
            var dataFirstCall = await GetPaginationShopReceipts(authorization, queries);
            if (dataFirstCall != null && dataFirstCall.results.Count() > 0)
            {
                results.AddRange(dataFirstCall.results);
                long totalPage = dataFirstCall.count / limit;
                if (pageNumber != null)
                {
                    totalPage = totalPage > pageNumber ? pageNumber.Value : totalPage;
                }
                if (totalPage > 0)
                {
                    for (int i = 1; i < totalPage; i++)
                    {
                        queries.offset = i;
                        var data = await GetPaginationShopReceipts(authorization, queries);
                        if (data == null)
                        {
                            //refreshToken = await _authenticationEtsyService.RefreshToken(null);
                            //if (refreshToken == null)
                            //{
                            //    await _logService.CreateLogError($"Service: OrderService, Action: GetShopReceipts. No Authorization.");
                            //    return null;
                            //}
                            //authorization = $"Bearer {refreshToken.Access_token}";
                        }
                        results.AddRange(data.results);
                    }
                }
            }
            return results;
        }
        public async Task<ShopReceiptsDto> GetPaginationShopReceipts(string authorization, GetShopReceiptsParams queries)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
            client.DefaultRequestHeaders.Add("Authorization", authorization);
            string url = _urlShopReceipts + $"?min_created={queries.min_created}&max_created={queries.max_created}&min_last_modified={queries.min_last_modified}&max_last_modified={queries.max_last_modified}&limit={queries.limit}&offset={queries.offset}&sort_on={queries.sort_on}&sort_order={queries.sort_order}";
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    ShopReceiptsDto results = JsonConvert.DeserializeObject<ShopReceiptsDto>(responseBody)!;
                    return results;
                }

                return null;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: OrderService, action: GetPaginationShopReceipts, {ex.ToString()}");
                return null;
            }
        }

        public async Task<List<OrderDto>> GetNewOrderEtsy()
        {
            GetShopReceiptsParams queries = new GetShopReceiptsParams()
            {
                was_shipped = false,
            };
            var data = await GetShopReceipts(queries, 2);

            if (data?.Count > 0)
            {
                //Insert data
                var checkOrder = await _insertOrder.InsertUpdateOrder(data);
                var checkShipment = await _insertOrder.InsertUpdateShipment(data);
                var checkTransaction = await _insertOrder.InsertUpdateTransaction(data);
                if (checkOrder.Success && checkShipment.Success && checkTransaction.Success)
                {
                    return data;
                }
            }
            return data;
        }

        public async Task<OperationResult> SubmitOrderQgold()
        {
            //get data Qgold for submit
            var dataOrder = await _context.Order.Include(s => s.transactions).Where(x => x.is_submit != true).ToListAsync();
            var listtingId = dataOrder.SelectMany(x => x.transactions).Select(x => x.listing_id).ToList();
            var listProduct = await _context.ProductEtsy.Where(x => listtingId.Contains(x.listing_id) && x.item_sku.StartsWith("Q-")).ProjectTo<ProductEtsyDto>(_mapperConfiguration).ToListAsync();

            //Convert order
            var dataSubmit = MapperOrder.ConvertOrderQgold(ref dataOrder, listProduct);

            //Call API QGold
            if (dataSubmit.Any())
            {
                if (_isProduction)
                {
                    var check = await _qgoldApiClient.SubmitOrder(dataSubmit, false);
                    if (check)
                    {
                        //Save update Order
                        try
                        {
                            dataOrder = dataOrder.Where(x => x.is_submit == true).ToList();
                            await _context.BulkUpdateAsync(dataOrder);
                            return new OperationResult() { Success = true };
                        }
                        catch (Exception ex)
                        {
                            await _logService.CreateLogError($"Service: OrderService, action: SubmitOrderQgold, {ex.ToString()}");
                            return new OperationResult() { Success = false, Message = ex.ToString() };
                        }
                    }
                    else
                    {
                        await _logService.CreateLogError($"Service: OrderService, action: SubmitOrderQgold call api false");
                        return new OperationResult() { Success = false, Message = $"Service: OrderService, action: SubmitOrderQgold call api false" };
                    }
                }

            }
            return new OperationResult() { Success = true, Message = "No data Submit Qgold" };
        }

        public async Task<OperationResult> SubmitOrderStuller()
        {
            await _logService.CreateLogError($"Submit Order Stuller Processor START =====================================================");
            //Get
            var dataOrder = await _context.Order.Include(s => s.transactions).Where(x => x.is_submit != true).ToListAsync();
            var listtingId = dataOrder.SelectMany(x => x.transactions).Select(x => x.listing_id).ToList();
            var listProduct = await _context.ProductEtsy.Where(x => listtingId.Contains(x.listing_id) && x.item_sku.StartsWith("S-")).ProjectTo<ProductEtsyDto>(_mapperConfiguration).ToListAsync();

            //Convert Order
            var dataSubmit = MapperOrder.ConvertOrderStuller(ref dataOrder, listProduct);

            //Call Api Submit
            if (dataSubmit.Any())
            {
                var check = false;
                if (_isProduction)
                {
                    foreach (var item in dataSubmit)
                    {
                        check = await _stullerApiClient.SubmitOrder(item, false);
                    }
                }
                if (check)
                {
                    //Save update Order
                    try
                    {
                        dataOrder = dataOrder.Where(x => x.is_submit == true).ToList();
                        await _context.BulkUpdateAsync(dataOrder);
                        return new OperationResult() { Success = true };
                    }
                    catch (Exception ex)
                    {
                        await _logService.CreateLogError($"Service: OrderService, action: SubmitOrderStuller, {ex.ToString()}");
                        return new OperationResult() { Success = false, Message = ex.ToString() };
                    }
                }
                else
                {
                    await _logService.CreateLogError($"Service: OrderService, action: SubmitOrderStuller call api false");
                    return new OperationResult() { Success = false, Message = $"Service: OrderService, action: SubmitOrderStuller call api false" };
                }
            }
            return new OperationResult() { Success = true, Message = "No data Submit Stuller" };
        }

        public async Task<OperationResult> UpdateTrackingOrder()
        {
            await _logService.CreateLogError($"Update tracking to Etsy START //==========================================");
            List<ReceiptShipment> data = new List<ReceiptShipment>();
            List<ReceiptShipment> dataQ = new List<ReceiptShipment>();
            List<ReceiptShipment> dataS = new List<ReceiptShipment>();
            //=============================get order form Qgold ======================
            try
            {
                dataQ = await OrderQgoldFulfillment();
                await _logService.CreateLogError($"Qgold has {dataQ?.Count} for submit //==========================================");
                if (dataQ != null)
                {
                    data.AddRange(dataQ);
                }
            }
            catch (Exception e)
            {
                await _logService.CreateLogError($"Service: OrderService, action: UpdateTrackingOrder call api false {e.ToString()}");
            }
            //=============================get order form Stuller ======================
            try
            {
                dataS = await OrderStullerFulfillment();
                await _logService.CreateLogError($"Stuller has {dataS?.Count} for submit //==========================================");
                if (dataS != null)
                {
                    data.AddRange(dataS);
                }

            }
            catch (Exception e)
            {
                await _logService.CreateLogError($"OrderStullerFulfillment error: {e.ToString()}");
            }

            //=============================submit order ==================================================
            if (data != null)
            {
                if (_isProduction)
                {
                    //update order to Etsy
                    var refreshToken = await _authenticationEtsyService.RefreshToken(null);
                    if (refreshToken == null)
                    {
                        await _logService.CreateLogError($"Service: OrderService, Action: UpdateTrackingOrder. No Authorization.");
                        return null;
                    }
                    string authorization = $"Bearer {refreshToken.Access_token}";

                    foreach (var item in data)
                    {
                        await CreateReceiptShipment(authorization, item);
                    }

                    //update order to Database
                    var dataUpdate = await _orderRepository.FindAll(x => data.Select(x => x.receipt_id).Contains(x.receipt_id)).ToListAsync();
                    foreach (var entity in dataUpdate)
                    {
                        entity.is_shipped = true;
                    }
                    _orderRepository.UpdateMultiple(dataUpdate);
                    await _orderRepository.SaveChangesAsync();

                }
                await _logService.CreateLogError($"Update {data.Count()} orders");
            }
            else
            {
                await _logService.CreateLogError($"No data Update tracking to Etsy DONE //==========================================");
            }
            return new OperationResult { Success = true };
        }

        private async Task<List<QgoldOrderDto>> GetOrderQgold()
        {
            try
            {
                var data = await _qgoldApiClient.GetOrder(45);
                if (data != null)
                {
                    data = data.Where(x => x.OrderStatus == "Invoiced").ToList();
                    return data;
                }
                return new List<QgoldOrderDto>();
            }
            catch (Exception ex)
            {

                await _logService.CreateLogError($"GetOrderQgold error: {ex.ToString()}");
                return new List<QgoldOrderDto>();
            }
        }
        private async Task<List<ReceiptShipment>> OrderQgoldFulfillment()
        {
            //Get order from Qgold
            var dataQgold = await GetOrderQgold();
            if (dataQgold is null) return null;
            //Get order
            var dataOrder = await _orderRepository.FindAll(x => x.is_shipped != true).ToListAsync();

            List<ReceiptShipment> resultTmp = new List<ReceiptShipment>();

            foreach (var i in dataOrder)
            {
                try
                {
                    //check Buyer name
                    char NameA = i.name.ToLower().ToList()[0];
                    char AddressLine1A = i.first_line.ToLower().ToList()[0];
                    bool PC = i.zip.Contains("-");
                    string PostalCode = PC ? i.zip.Split("-")[0] : i.zip;

                    List<QgoldOrderDto> checkQgold = dataQgold.FindAll(x =>
                        NameA == x.BuyerName?.ToLower().ToList()[0]
                        && AddressLine1A == x.Address1?.ToLower().ToList()[0]
                        && PostalCode.Contains(x.ZipCode)).ToList();

                    if (checkQgold.Any())
                    {
                        foreach (var iQ in checkQgold)
                        {
                            List<string> checkItems = iQ.Items.Select(s => s.Style).ToList();
                            if (checkItems.Any())
                            {
                                //check các item trong order này có giống với list style trong checkItems trên hay không
                                var listtings = i.transactions.Select(s => s.listing_id).ToList();
                                var dataProduct = _context.ProductEtsy.Where(x => listtings.Contains(x.listing_id)).ToList();
                                var checkItems2 = dataProduct.Where(x => checkItems.Contains(x.item_sku.Replace("Q-", ""))).ToList();
                                if (checkItems.Count == checkItems2.Count)
                                {
                                    string trackingID = iQ.Tracking;
                                    string ShippingMethod = iQ.ShipVia;
                                    string CarrierName = ShippingMethod.Split(" ")[0];
                                    if (ShippingMethod.Contains("CaPost"))
                                    {
                                        CarrierName = "CanadaPost";
                                    }
                                    ReceiptShipment receiptShipment = new ReceiptShipment()
                                    {
                                        receipt_id = i.receipt_id,
                                        carrier_name = CarrierName,
                                        note_to_buyer = "",
                                        send_bcc = false,
                                        tracking_code = trackingID

                                    };
                                    resultTmp.Add(receiptShipment);
                                    dataQgold.Remove(iQ);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await _logService.CreateLogError($"GetOrderQgoldToEsty error: {ex.ToString()}");
                }
            }
            List<ReceiptShipment> result = resultTmp.GroupBy(x => x.tracking_code).Select(g => g.First()).ToList();
            return result;
        }
        private async Task<List<StullerOrderDTO>> GetOrderStuller()
        {
            StullerSearchParam? paramSearch = new StullerSearchParam();
            paramSearch.FromDate = DateTime.Now.AddDays(-15).ToString("yyyy-MM-dd");
            paramSearch.ToDate = DateTime.Now.ToString("yyyy-MM-dd");
            var data = await _stullerApiClient.GetOrderMarketplace(paramSearch);
            return data;
        }
        private async Task<List<ReceiptShipment>> OrderStullerFulfillment()
        {
            //Get order from Qgold
            var dataStuller = await GetOrderStuller();
            if (dataStuller is null) return null;
            //Get order
            var dataOrder = await _orderRepository.FindAll(x => x.is_shipped != true).ToListAsync();

            List<ReceiptShipment> resultTmp = new List<ReceiptShipment>();

            foreach (var i in dataOrder)
            {
                try
                {
                    //check receipt_id lúc submit đi có giống với id đang có trong database không
                    var checkStuller = dataStuller.FirstOrDefault(x => x.PO == i.receipt_id.ToString());
                    if (checkStuller is not null)
                    {
                        ReceiptShipment receiptShipment = new ReceiptShipment()
                        {
                            receipt_id = i.receipt_id,
                            carrier_name = checkStuller.Carrier.Contains("USPS") ? "USPS" : checkStuller.Carrier,
                            note_to_buyer = "",
                            send_bcc = false,
                            tracking_code = checkStuller.Tracking,

                        };
                        var checkTime = DateTime.Parse(checkStuller.ShipDate).ToString("yyyy-MM-dd'T'HH:mm:ss.fffK");
                        if (!checkTime.Contains("0001-01-01"))
                            resultTmp.Add(receiptShipment);
                    }
                }
                catch (Exception ex)
                {
                    await _logService.CreateLogError($"GetOrderQgoldToEsty error: {ex.ToString()}");
                }
            }
            List<ReceiptShipment> result = resultTmp.GroupBy(x => x.tracking_code).Select(g => g.First()).ToList();
            return result;
        }

        public async Task<OrderDto> CreateReceiptShipment(string authorization, ReceiptShipment dto)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
            client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
            client.DefaultRequestHeaders.Add("Authorization", authorization);
            string url = _urlShopReceipts + $"{dto.receipt_id}/tracking";

            var dict = new Dictionary<string, string>();

            //Update Listing cũng có thể update lại các field giống như hàm CreateDraftListing();
            dict.Add("tracking_code", dto.tracking_code);
            dict.Add("carrier_name", dto.carrier_name);
            dict.Add("note_to_buyer", dto.note_to_buyer);
            if (dto.send_bcc)
            {
                dict.Add("send_bcc", "true");
            }
            var requestContent = new FormUrlEncodedContent(dict);

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, requestContent);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    OrderDto results = JsonConvert.DeserializeObject<OrderDto>(responseBody)!;
                    return results;
                }

                return null;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: OrderService, action: CreateReceiptShipment, {ex.ToString()}");
                return null;
            }
        }
    }
}
