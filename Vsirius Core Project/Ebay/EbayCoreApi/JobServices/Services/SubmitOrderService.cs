using AutoMapper;
using EbayCoreApi.Common.Configurations;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.DataContext;
using EbayCoreApi.Dtos;
using EbayCoreApi.JobServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Mathematics;
using Newtonsoft.Json;
using System.Text;

namespace EbayCoreApi.JobServices.Services
{
    public class SubmitOrderService : ISubmitOrderService
    {
        private readonly EbayContext _context;
        private readonly OrderProcessApi._Services.IServices.Qgold.IQgoldProcessService _qgoldProcessService;
        private readonly OrderProcessApi._Services.IServices.Qgold.ISubmitOrderQgoldService _submitOrderQgoldProcessService;
        private readonly IMapper _mapper;

        public SubmitOrderService(
            OrderProcessApi._Services.IServices.Qgold.IQgoldProcessService qgoldProcessService,
            OrderProcessApi._Services.IServices.Qgold.ISubmitOrderQgoldService submitOrderQgoldProcessService,
            IMapper mapper,
            EbayContext context)
        {
            _context = context;
            _qgoldProcessService = qgoldProcessService;
            _mapper = mapper;
            _submitOrderQgoldProcessService = submitOrderQgoldProcessService;
        }

        public async Task SubmitOrderEbay()
        {
            UtilHelper.WriteLog("Start Submit Ebay Order");
            try
            {
                var lstEbayOrder = await _context.Orders
                    .AsNoTracking()
                    .Include(x => x.OrderDetails)
                    .Where(x => x.IsSubmit == false)
                    .ToListAsync();

                UtilHelper.WriteLog("Get Order From Qgold DB in 5 days Start");
                List<OrderQgoldDto> lstOrderQgold = await GetQgoldOrder_RFT();
                UtilHelper.WriteLog($"Amount Order Qgold in DB: {lstOrderQgold.Count}");

                var lstSubmit = new List<SubmitQgoldOrderDto>();
                foreach (var ebay in lstEbayOrder)
                {
                    if (lstOrderQgold.Any(
                        q => q.Address1.TrimStart()[0].ToString().ToLower() == ebay.Address1.TrimStart()[0].ToString().ToLower()
                        && q.BuyerName.TrimStart()[0].ToString().ToLower() == ebay.CustomerName.TrimStart()[0].ToString().ToLower()
                        && q.ZipCode.TrimStart()[0].ToString().ToLower() == ebay.ZipCode.TrimStart()[0].ToString().ToLower()
                        && q.Items.Select(x => x.Style).Any(x => ebay.OrderDetails.Where(e => e.Sku.StartsWith("Q"))
                        .Select(e => UtilHelper.GetOriginSKU(e.Sku)).Contains(x))
                        ))
                    {
                        continue;
                    }
                    else
                    {
                        var submitOrder = new SubmitQgoldOrderDto()
                        {
                            orderId = null,
                            format = "5",
                            customer = "58772",
                            timeStamp = DateTime.Now,
                            shipToName = ebay.CustomerName,
                            shipToAddress1 = ebay.Address1,
                            shipToAddress2 = ebay.Address2,
                            shipToCity = ebay.City,
                            shipToState_Province = ebay.State,
                            shipToZip = ebay.ZipCode,
                            shipToCountryCode = ebay.Country,
                            shipToPhone = ebay.CustomerPhone,
                            ship_Method_Code = "1102",
                            ship_Option_Code = "100",
                            amz_Order_ID = ebay.OrderId,
                            marketplace = "Ebay",
                            item = new List<ItemQgoldParam>()
                        };

                        foreach (var ebayDetail in ebay.OrderDetails)
                        {
                            submitOrder.item.Add(new ItemQgoldParam()
                            {
                                item = UtilHelper.GetOriginSKU(ebayDetail.Sku),
                                price = ebayDetail.Price.ToString(),
                                qty = ebayDetail.Quantity.ToString(),
                                size = ""
                            });
                        }

                        lstSubmit.Add(submitOrder);
                    };
                }

                if (lstSubmit.Any())
                {
                    UtilHelper.WriteLog($"Starting submit order with {lstSubmit.Count} order");
                    bool isSuccess = await CallSubmittedApi_RFT(lstSubmit);
                    if (isSuccess)
                    {
                        var lstEntityUpdateOrderSubmit = 
                            await _context.Orders
                            .Where(x => lstSubmit.Select(s => s.amz_Order_ID).Contains(x.OrderId)
                            && x.IsSubmit == false)
                            .ToListAsync();
                        foreach (var submit in lstEntityUpdateOrderSubmit)
                        {
                            submit.IsSubmit = true;
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }
            UtilHelper.WriteLog("End Submit Ebay Order");
        }

        private async Task<bool> CallSubmittedApi(List<SubmitQgoldOrderDto> lstSubmit)
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("clientId", $"{ConfigHolder.Configuration["QgoldOrderApi:clientId"]}");
                httpClient.DefaultRequestHeaders.Add("clientSecret", $"{ConfigHolder.Configuration["QgoldOrderApi:clientSecret"]}");

                var jsonContent = JsonConvert.SerializeObject(lstSubmit);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(ConfigHolder.Configuration["QgoldOrderApi:url"].ToString(), content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    if (result != null)
                    {
                        return JsonConvert.DeserializeObject<bool>(result);
                    }
                    else
                    {
                        return false;
                    }
                }
                UtilHelper.WriteLog($"CallSubmittedApi Err: {JsonConvert.SerializeObject(response)}");
                return false;

            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"CallSubmittedApi Err: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> CallSubmittedApi_RFT(List<SubmitQgoldOrderDto> lstSubmit)
        {
            bool result = false;
            try
            {
                var listSubmit = new List<OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject>();
                foreach (var item in lstSubmit)
                {
                    var dto = _mapper.Map<OrderProcessDomain.AmazonSeller.QgoldFtpOrderObject>(item);
                    listSubmit.Add(dto);
                }
                result = await _submitOrderQgoldProcessService.SubmitQgold(listSubmit, null);

                return result;
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"Submit order failed with err: {ex.ToString}");
                return result;
            }
        }

        private async Task<List<OrderQgoldDto>> GetQgoldOrder()
        {
            var client = new HttpClient();
            var url = ConfigHolder.Configuration["QgoldOrderApi:GetOrder"]
                + $"?PageSize=300&FromDate={DateTime.Now.AddDays(-5):yyyy-MM-dd}&ToDate={DateTime.Now:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PaginationDto<OrderQgoldDto>>(content).Data;
            }
            return default;
        }

        private async Task<List<OrderQgoldDto>> GetQgoldOrder_RFT()
        {
            var result = new List<OrderQgoldDto>();
            try
            {
                var pagination = new OrderProcessApi.DTOs.PaginationParam();
                pagination.PageSize = 300;

                var search = new OrderProcessApi.DTOs.SearchQgoldProcessParam();
                search.FromDate = $"{DateTime.Now.AddDays(-5):yyyy - MM - dd}";
                search.ToDate = $"{DateTime.Now:yyyy-MM-dd}";

                var data = await _qgoldProcessService.GetOrders(pagination, search);

                foreach (var item in data.Data)
                {
                    var dto = _mapper.Map<OrderQgoldDto>(item);
                    result.Add(dto);
                }
                return result;
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog($"Get order from order process Err: {ex.ToString}");
                return result;
            }
        }
    }
}