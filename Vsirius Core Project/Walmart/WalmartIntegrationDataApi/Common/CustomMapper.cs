
using WalmartIntegrationDataApi.Dtos;
using WalmartIntegrationDataApi.DtosOrderStuller;
using WalmartIntegrationDataApi.Models;

namespace StullerOrderProcessAPI.Common
{
    public static class CustomMapper
    {
        public static StullerOrderDTO? ConvertOrderEntityToDto(StullerOrder? stullerOrder)
        {
            if (stullerOrder == null) return null;
            return new StullerOrderDTO() 
            {
                OrderID = stullerOrder.OrderID,
                Status = stullerOrder.Status,
                Account = stullerOrder.Account,
                Address1 = stullerOrder.Address1,
                Billing = stullerOrder.Billing,
                BuyerName = stullerOrder.BuyerName,
                EstTotalPrice = stullerOrder.EstTotalPrice,
                Confirmation = stullerOrder.Confirmation,
                OrderDate = stullerOrder.OrderDate,
                PO = stullerOrder.PO,
                ShippingMethod = stullerOrder.ShippingMethod,
                Shipping = stullerOrder.Shipping,
                UrlDetail = stullerOrder.UrlDetail,
                ZipCode = stullerOrder.ZipCode,
                Carrier = stullerOrder.TrackOrder[0]?.Carrier,
                DeliveryDate = stullerOrder.TrackOrder[0]?.DeliveryDate,
                Invoice = stullerOrder.TrackOrder[0]?.Invoice,
                InvoiceAmount = stullerOrder.TrackOrder[0]?.InvoiceAmount,
                ShipDate = stullerOrder.TrackOrder[0]?.ShipDate,
                ShippingUpdates = stullerOrder.TrackOrder[0]?.ShippingUpdates,
                Tracking = stullerOrder.TrackOrder[0]?.Tracking,
                Items = stullerOrder.Items,
                Marketplace = stullerOrder.Marketplace,
                MarketplaceOrderId = stullerOrder.MarketplaceOrderId,
            };
        }
    }
}
