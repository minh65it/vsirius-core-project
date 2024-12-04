using AutoMapper;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using EbayCoreApi.Common.Configurations;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.DataContext;
using EbayCoreApi.Dtos;
using EbayCoreApi.JobServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace EbayCoreApi.JobServices.Services
{
    public class UpdateTrackingOrderService : IUpdateTrackingOrderService
    {
        private readonly EbayContext _context;
        private readonly OrderProcessApi._Services.IServices.Qgold.IQgoldProcessService _serviceQgoldProcess;
        private readonly StullerOrderProcessAPI._Services.Interfaces.IOrderStullerService _orderStullerService;
        private readonly IMapper _mapper;


        public UpdateTrackingOrderService(EbayContext context, 
            OrderProcessApi._Services.IServices.Qgold.IQgoldProcessService serviceQgoldProcess,
            StullerOrderProcessAPI._Services.Interfaces.IOrderStullerService orderStullerService,
            IMapper mapper)
        {
            _context = context;
            _serviceQgoldProcess = serviceQgoldProcess;
            _orderStullerService = orderStullerService;
            _mapper = mapper;
        }

        public async Task UpdateTrackingOrder()
        {
            UtilHelper.WriteLog("Update Tracking Ebay Order Start");
            try
            {
                var lstOrderNeedUpdateTracking =
                    await _context.Orders
                    .AsNoTracking()
                    .Include(x => x.OrderDetails)
                    .Where(x => string.IsNullOrEmpty(x.TrackingNo)
                    && x.IsSubmit == true)
                    .ToListAsync();

                List<OrderQgoldDto> lstOrderQgold = await GetQgoldOrder_RFT();
                lstOrderQgold = lstOrderQgold.Where(x => !string.IsNullOrEmpty(x.Tracking)).ToList();

                List<OrderStullerDto> lstOrderStuller = await GetStullerOrder_RFT();

                var lstTracking = new List<TrackingDto>();

                foreach (var ebay in lstOrderNeedUpdateTracking)
                {
                    if (ebay.OrderDetails.Any(x => x.Sku.StartsWith("Q")))
                    {
                        var qgold = lstOrderQgold.FirstOrDefault(
                            q => q.Address1.TrimStart()[0].ToString().ToLower() == ebay.Address1.TrimStart()[0].ToString().ToLower()
                            && q.BuyerName.TrimStart()[0].ToString().ToLower() == ebay.CustomerName.TrimStart()[0].ToString().ToLower()
                            && q.ZipCode.TrimStart()[0].ToString().ToLower() == ebay.ZipCode.TrimStart()[0].ToString().ToLower()
                            && q.Items.Select(x => x.Style).Any(x => ebay.OrderDetails.Where(e => e.Sku.StartsWith("Q"))
                            .Select(e => UtilHelper.GetOriginSKU(e.Sku)).Contains(x)));
                        if (qgold != null)
                        {
                            lstTracking.Add(new TrackingDto()
                            {
                                OrderId = ebay.OrderId,
                                DetailItems = ebay.OrderDetails.Select(x => new TrackingDetailDto()
                                {
                                    LineItemId = x.Sku,
                                    Quantity = x.Quantity,
                                    TransactionID = x.TransactionID,
                                    ItemID = x.ItemID,
                                }),
                                TrackingNo = qgold.Tracking,
                                ShipVia = qgold.ShipVia,
                            });
                        }
                    }
                    if (ebay.OrderDetails.Any(x => x.Sku.StartsWith("S")))
                    {
                        var stuller = lstOrderStuller.FirstOrDefault(s => s.po == ebay.OrderId);
                        if (stuller != null)
                        {
                            lstTracking.Add(new TrackingDto()
                            {
                                OrderId = ebay.OrderId,
                                DetailItems = ebay.OrderDetails.Select(x => new TrackingDetailDto()
                                {
                                    LineItemId = x.OrderDetailId,
                                    Quantity = x.Quantity,
                                    TransactionID = x.TransactionID,
                                    ItemID = x.ItemID,
                                }),
                                TrackingNo = stuller.tracking,
                                ShipVia = stuller.carrier,
                            });
                        }
                    }
                }

                if (lstTracking.Count > 0)
                {
                    CallApiUpdateTrackingEbay(lstTracking);
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }
            UtilHelper.WriteLog("Update Tracking Ebay Order End");
        }

        private void CallApiUpdateTrackingEbay(List<TrackingDto> lstTracking)
        {
            foreach (var tracking in lstTracking)
            {
                foreach (var item in tracking.DetailItems)
                {
                    CompleteSaleCall completeSale = new (ApiEbayContextHelper.GetApiContext())
                    {
                        ItemID = item.ItemID, 
                        TransactionID = item.TransactionID, 
                        Shipment = new ShipmentType
                        {
                            ShipmentTrackingDetails = new()
                            {
                                new ShipmentTrackingDetailsType
                                {
                                    ShippingCarrierUsed = tracking.ShipVia, 
                                    ShipmentTrackingNumber = tracking.TrackingNo,
                                }
                            }
                        }
                    };

                    try
                    {
                        completeSale.Execute();
                        UtilHelper.WriteLog("Tracking number added successfully.");
                    }
                    catch (Exception ex)
                    {
                        UtilHelper.WriteLog("Error: " + ex.Message);
                    }
                }
            }
        }

        private async Task<List<OrderStullerDto>> GetStullerOrder()
        {
            var client = new HttpClient();
            var url = ConfigHolder.Configuration["StullerApi:GetOrder"]
                + $"?PageSize=300&FromDate={DateTime.Now.AddDays(-5):yyyy-MM-dd}&ToDate={DateTime.Now:yyyy-MM-dd}";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PaginationUtil<OrderStullerDto>>(content).Result;
            }
            return default;
        }

        private async Task<List<OrderStullerDto>> GetStullerOrder_RFT()
        {
            var result = new List<OrderStullerDto>();
            try
            {
                var pagination = new StullerOrderProcessAPI.Common.Helpers.PaginationParam();
                pagination.PageSize = 300;

                var search = new StullerOrderProcessAPI.Dtos.SearchOrderStullerParam();
                search.FromDate = $"{DateTime.Now.AddDays(-5):yyyy-MM-dd}";
                search.ToDate = $"{DateTime.Now:yyyy-MM-dd}";

                var data = await _orderStullerService.GetOrders(pagination, search);

                foreach (var item in data.Result)
                {
                    var dto = _mapper.Map<OrderStullerDto>(item);
                    result.Add(dto);
                }

                return result;
            }
            catch (Exception)
            {
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
                search.FromDate = $"{DateTime.Now.AddDays(-5):yyyy-MM-dd}";
                search.ToDate = $"{DateTime.Now:yyyy-MM-dd}";

                var data = await _serviceQgoldProcess.GetOrders(pagination, search);

                foreach (var item in data.Data)
                {
                    var dto = _mapper.Map<OrderQgoldDto>(item);
                    result.Add(dto);
                }
                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
    }
}