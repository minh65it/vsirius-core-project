using eBay.Service.Core.Sdk;
using Newtonsoft.Json;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text;
using eBay.Service.Call;
using EbayCoreApi.JobServices.Interfaces;
using EbayCoreApi.DataContext;
using EbayCoreApi.Common.Helpers;
using eBay.Service.Core.Soap;
using EbayCoreApi.Models;
using Microsoft.EntityFrameworkCore;
using EbayCoreApi.Common.Configurations;
using EbayCoreApi.Dtos;

namespace EbayCoreApi.JobServices.Services
{
    public class GetOrderEbayService : IGetOrderEbayService
    {
        private readonly EbayContext _context;

        public GetOrderEbayService(EbayContext context)
        {
            _context = context;
        }

        public async Task GetOrderFromEbay()
        {
            UtilHelper.WriteLog("Get Ebay Order Start");
            try
            {
                GetOrdersCall apiCall = new (ApiEbayContextHelper.GetApiContext());

                DateTime fromDate = DateTime.Now.AddDays(-5);
                DateTime toDate = DateTime.Now;

                var orders = apiCall
                    .GetOrders(
                    fromDate,
                    toDate,
                    TradingRoleCodeType.Seller,
                    OrderStatusCodeType.All);

                UtilHelper.WriteLog($"{orders.Count} orders");

                foreach (OrderType order in orders)
                {
                    var orderId = order.OrderID;
                    var orderEntity = new Order
                    {
                        OrderId = orderId,
                        Address1 = order.ShippingAddress.Street1,
                        Address2 = order.ShippingAddress.Street2,
                        State = order.ShippingAddress.StateOrProvince,
                        ZipCode = order.ShippingAddress.PostalCode,
                        City = order.ShippingAddress.CityName,
                        CustomerName = order.ShippingAddress.Name,
                        Country = order.ShippingAddress.Country.ToString(),
                        TrackingNo = $"{order.ShippingDetails?.ShipmentTrackingDetails?.FirstOrDefault()?.ShipmentTrackingNumber}",
                        CustomerPhone = order.ShippingAddress.Phone,
                        CustomerId = order.BuyerUserID,
                        CustomerEmail = $"{order.TransactionArray?.FirstOrDefault().Buyer?.Email}",
                        OrderDate = (DateTime)order.CreatedTime,
                        StatusOrder = order.OrderStatus.ToString(),
                        TotalPrice = order.Subtotal.Value,
                        IsSubmit = false,
                        OrderDetails = new List<OrderDetail>()
                    };

                    foreach (TransactionType detail in order.TransactionArray)
                    {
                        ItemType item = detail.Item;
                        orderEntity.OrderDetails.Add(new OrderDetail()
                        {
                            OrderDetailId = detail.OrderLineItemID,
                            Price = detail.TransactionPrice.Value,
                            Quantity = (int)detail.QuantityPurchased,
                            OrderId = order.OrderID,
                            Order = orderEntity,
                            Sku = item.SKU,
                            ItemID = item.ItemID,
                            TransactionID = detail.TransactionID,
                        });
                    }

                    var existingOrder = await _context.Orders.
                        Include(o => o.OrderDetails).AsNoTracking()
                        .FirstOrDefaultAsync(o => o.OrderId == orderId);

                    //Update
                    if (existingOrder != null)
                    {
                        orderEntity.IsSubmit = existingOrder.IsSubmit;
                        _context.Orders.Update(orderEntity);
                    }
                    else
                    {
                        _context.Orders.Add(orderEntity);
                    }

                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                UtilHelper.WriteLog(ex.Message);
            }
            UtilHelper.WriteLog("Get Ebay Order End");
        }
    }
}