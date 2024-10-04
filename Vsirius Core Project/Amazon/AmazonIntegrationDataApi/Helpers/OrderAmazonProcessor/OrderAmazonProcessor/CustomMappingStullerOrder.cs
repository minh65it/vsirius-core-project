using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;
using FikaAmazonAPI.ConstructFeed.Messages;
using System.Drawing;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor
{
    public static class CustomMappingStullerOrder
    {
        public static List<AmazonOrderDto> MapStullerToAmazon(List<AmazonOrderDto> listOrderAmazon, List<StullerOrderDto> listOrderStuller)
        {
            return listOrderAmazon.Join(listOrderStuller, x => x.AmazonOrderId, y => y.MarketplaceOrderId, (x, y) => new AmazonOrderDto
            {
                AmazonOrderId = x.AmazonOrderId,
                Supplier = "Stuller",
                SupplierOrderId = y.OrderID,
                BuyerName = x.BuyerName,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                AddressLine3 = x.AddressLine3,
                City = x.City,
                CountryCode = x.CountryCode,
                EarliestDeliveryDate = x.EarliestDeliveryDate,
                EarliestShipDate = x.EarliestShipDate,
                FulfillmentChannel = x.FulfillmentChannel,
                LatestDeliveryDate = x.LatestDeliveryDate,
                LatestShipDate = x.LatestShipDate,
                MarketplaceId = x.MarketplaceId,
                OrderStatus = x.OrderStatus,
                OrderType = x.OrderType,
                PaymentMethod = x.PaymentMethod,
                Phone = x.Phone,
                PurchaseDate = x.PurchaseDate,
                SalesChannel = x.SalesChannel,
                ShipmentServiceLevelCategory = x.ShipmentServiceLevelCategory,
                ShipServiceLevel = x.ShipServiceLevel,
                StateOrRegion = x.StateOrRegion,
                Total = x.Total,
                PostalCode = x.PostalCode,
                IsSubmitted = true,
                OrderItemList = x.OrderItemList,
            }).ToList();
        }

        public static List<ItemAmazon> MapItemOrderAmazon(List<OrderItemList>? orderItemList = null)
        {
            var result = new List<ItemAmazon>();

            if (orderItemList != null)
            {
                foreach (var item in orderItemList)
                {
                    result.Add(new ItemAmazon()
                    {
                        ASIN = item.ASIN,
                        ItemPrice = item.ItemPrice.Amount,
                        OrderItemId = item.OrderItemId,
                        QuantityOrdered = item.QuantityOrdered,
                        SellerSKU = item.SellerSKU,
                        ShippingPrice = item.ShippingPrice.Amount,
                        Title = item.Title
                    });
                }
            }

            return result;
        }
    }
}
