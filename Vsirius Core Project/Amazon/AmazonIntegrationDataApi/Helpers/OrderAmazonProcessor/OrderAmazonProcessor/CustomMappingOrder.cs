using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;
using FikaAmazonAPI.ConstructFeed.Messages;
using System.Drawing;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.OrderAmazonProcessor
{
    public static class CustomMappingOrder
    {
        public static PaginationResult<QgoldOrderDto> ConvertToQgoldOrderDTO(
            PaginationResult<OrderQgold> pagOrderUIQgold,
            List<OrderMarketplaceDto> lstMarketplace,
            List<OrderMarketplaceDto> lostPackage,
            bool isMarket)
        {
            if (lstMarketplace != null && lstMarketplace.Any())
            {
                lstMarketplace = lstMarketplace
                .GroupBy(item => item.OrderId)
                .Select(group => group.First())
                .ToList();
            }
            var result = new PaginationResult<QgoldOrderDto>()
            {
                PageCount = pagOrderUIQgold.PageCount,
                PageNumber = pagOrderUIQgold.PageNumber,
                PageSize = pagOrderUIQgold.PageSize,
                TotalCount = pagOrderUIQgold.TotalCount,
                Data = new List<QgoldOrderDto>(),
            };
            result.Data = pagOrderUIQgold.Data.Select(x => new QgoldOrderDto()
            {
                OrderQgoldId = x.Order,
                BuyerName = x.InvoiceDetails[0].ShipToText,
                OrderStatus = x.Status,
                OrderDate = x.Date,
                ShipVia = x.InvoiceDetails[0].ShipVia,
                Tracking = x.InvoiceDetails[0].Tracking,
                Address1 = x.InvoiceDetails[0].ShipToAddress1,
                ZipCode = x.InvoiceDetails[0].ZipCode,
                Total = x.Total,
                MarketOrderId = lstMarketplace?.FirstOrDefault(a => a.OrderId != null && a.OrderId.Equals(x.Order))?.Seller_Order_ID,
                Marketplace = lstMarketplace?.FirstOrDefault(a => a.OrderId != null && a.OrderId.Equals(x.Order))?.Marketplace,
                Items = x.InvoiceDetails[0].QgoldInvoiceItems,
                IsSendMailLostPackage = lostPackage?.FirstOrDefault(a => a.OrderId != null && a.OrderId.Equals(x.Order)) is not null ? true : false,
            }).ToList();

            return result;
        }

        public static QgoldOrderDetailDto ConvertToDetailOrder(OrderQgold orderUI, OrderMarketplaceDto? orderMarketplace, OrderMarketplaceDto? lostPackage)
        {
            var result = new QgoldOrderDetailDto()
            {
                OrderQgoldId = orderUI.Order,
                POQgold = orderUI.PO,
                Date = orderUI.Date,
                Status = orderUI.Status,
                Total = orderUI.Total,
                Location = orderUI.Location,
                ShipToPhone = orderMarketplace?.ShipToPhone,
                ShipViaCode = orderMarketplace?.Ship_Method_Code,
                MarketOrderId = orderMarketplace?.Seller_Order_ID,
                Marketplace = orderMarketplace?.Marketplace,
                Tracking = orderUI.InvoiceDetails[0].Tracking,
                DocumentType = orderUI.InvoiceDetails[0]?.DocumentType,
                DueDate = orderUI.InvoiceDetails[0]?.DueDate,
                Items = orderUI.InvoiceDetails[0].QgoldInvoiceItems,
                PackagingOption = orderUI.InvoiceDetails[0].PackagingOption,
                PlacedBy = orderUI.InvoiceDetails[0].PlacedBy,
                PlacedVia = orderUI.InvoiceDetails[0].PlacedVia,
                ShipOptionCode = orderMarketplace?.Ship_Option_Code,
                ShipToAddress1 = orderUI.InvoiceDetails[0].ShipToAddress1,
                ShipToText = orderUI.InvoiceDetails[0].ShipToText,
                ShipVia = orderUI.InvoiceDetails[0].ShipVia,
                SoldToAddress1 = orderUI.InvoiceDetails[0].SoldToAddress1,
                SoldToText = orderUI.InvoiceDetails[0].SoldToText,
                SoldToAddress2 = orderUI.InvoiceDetails[0].SoldToAddress2,
                Term = orderUI.InvoiceDetails[0].Term,
                ZipCode = orderUI.InvoiceDetails[0].ZipCode,
                IsSendMailLostPackage = lostPackage is not null ? true : false,
            };

            return result;
        }

        private static List<Models.OrderAmazonProcessor.Providers.QgoldInvoiceItem> MapItem(List<OrderMarketplaceItem> items)
        {
            var result = new List<Models.OrderAmazonProcessor.Providers.QgoldInvoiceItem>();
            items.ForEach(item =>
            {
                result.Add(new Models.OrderAmazonProcessor.Providers.QgoldInvoiceItem()
                {
                    Style = item.Item,
                    Price = item.Price,
                    Qty = item.Qty,
                });
            });
            return result;
        }

        
        public static List<AmazonOrderDto> ConvertToAmazonOrderDTOShipped( 
            List<OrderAmazonSubmitted> lstSubmit)
        {
            var result = new List<AmazonOrderDto>();

            var lstSub = lstSubmit
                .Select(amazon => new AmazonOrderDto()
                {
                    AmazonOrderId = amazon.Amz_Order_ID,
                    SupplierOrderId = amazon.OrderId,
                    BuyerEmail = null,
                    BuyerName = amazon.ShipToName,
                    Phone = amazon.ShipToPhone,
                    AddressLine1 = amazon.ShipToAddress1,
                    AddressLine2 = amazon.ShipToAddress2,
                    AddressLine3 = amazon.ShipToAddress3,
                    StateOrRegion = amazon.ShipToState_Province,
                    City = amazon.ShipToCity,
                    CountryCode = amazon.ShipToCountryCode,
                    PostalCode = amazon.ShipToZip,
                    PurchaseDate = amazon.TimeStamp,
                    EarliestDeliveryDate = null,
                    EarliestShipDate = null,
                    FulfillmentChannel = null,
                    LatestDeliveryDate = null,
                    LatestShipDate = null,
                    MarketplaceId = null,
                    OrderStatus = "Shipped",
                    OrderType = $"ShipMethod:{amazon.Ship_Method_Code}",
                    PaymentMethod = "",
                    SalesChannel = $"{amazon.ShipToCountryCode}.com",
                    ShipServiceLevel = $"ShipOption:{amazon.Ship_Option_Code}",
                    ShipmentServiceLevelCategory = null,
                    Total = "",
                    IsSubmitted = true,
                    OrderItemList = MapItemOrderAmazon1(amazon.Item)
                }).ToList();
            result.AddRange(lstSub);

            return result;
        }
        public static List<AmazonOrderDto> ConvertToAmazonOrderDTOUnShipped(
            List<OrderAmazon> lstAmazon,
            List<OrderAmazonSubmitted> lstSubmit)
        {
            var result = new List<AmazonOrderDto>();

            var lstInnerJoin = (from amazon in lstAmazon
                                join submit in lstSubmit on amazon.AmazonOrderId equals submit.Amz_Order_ID
                                select new AmazonOrderDto()
                                {
                                    AmazonOrderId = amazon.AmazonOrderId,
                                    Supplier = CheckListItem(amazon.OrderItemList),
                                    SupplierOrderId = submit.OrderId,
                                    BuyerEmail = amazon.BuyerInfo.BuyerEmail,
                                    BuyerName = amazon.ShippingAddress.Name,
                                    Phone = amazon.ShippingAddress.Phone,
                                    AddressLine1 = amazon.ShippingAddress.AddressLine1,
                                    AddressLine2 = amazon.ShippingAddress.AddressLine2,
                                    AddressLine3 = amazon.ShippingAddress.AddressLine3,
                                    StateOrRegion = amazon.ShippingAddress.StateOrRegion,
                                    City = amazon.ShippingAddress.City,
                                    CountryCode = amazon.ShippingAddress.CountryCode,
                                    PostalCode = amazon.ShippingAddress.PostalCode,
                                    PurchaseDate = amazon.PurchaseDate,
                                    EarliestDeliveryDate = amazon.EarliestDeliveryDate,
                                    EarliestShipDate = amazon.EarliestShipDate,
                                    FulfillmentChannel = amazon.FulfillmentChannel,
                                    LatestDeliveryDate = amazon.LatestDeliveryDate,
                                    LatestShipDate = amazon.LatestShipDate,
                                    MarketplaceId = amazon.MarketplaceId,
                                    OrderStatus = amazon.OrderStatus,
                                    OrderType = amazon.OrderType,
                                    PaymentMethod = amazon.PaymentMethod,
                                    SalesChannel = amazon.SalesChannel,
                                    ShipServiceLevel = amazon.ShipServiceLevel,
                                    ShipmentServiceLevelCategory = amazon.ShipmentServiceLevelCategory,
                                    Total = amazon.OrderTotal.Amount + amazon.OrderTotal.CurrencyCode,
                                    IsSubmitted = true,
                                    OrderItemList = MapItemOrderAmazon(amazon.OrderItemList)
                                }).ToList();
            result.AddRange(lstInnerJoin);

            return result;
        }
        public static List<AmazonOrderDto> ConvertToAmazonOrderDTONotSubmit(List<OrderAmazon> lstAmazon)
        {
            var result = new List<AmazonOrderDto>();

            var lstAmz = lstAmazon
                .Select(amazon => new AmazonOrderDto()
                {
                    AmazonOrderId = amazon.AmazonOrderId,
                    Supplier = CheckListItem(amazon.OrderItemList),
                    SupplierOrderId = null,
                    BuyerEmail = amazon.BuyerInfo.BuyerEmail,
                    BuyerName = amazon.ShippingAddress.Name,
                    Phone = amazon.ShippingAddress.Phone,
                    AddressLine1 = amazon.ShippingAddress.AddressLine1,
                    AddressLine2 = amazon.ShippingAddress.AddressLine2,
                    AddressLine3 = amazon.ShippingAddress.AddressLine3,
                    StateOrRegion = amazon.ShippingAddress.StateOrRegion,
                    City = amazon.ShippingAddress.City,
                    CountryCode = amazon.ShippingAddress.CountryCode,
                    PostalCode = amazon.ShippingAddress.PostalCode,
                    PurchaseDate = amazon.PurchaseDate,
                    EarliestDeliveryDate = amazon.EarliestDeliveryDate,
                    EarliestShipDate = amazon.EarliestShipDate,
                    FulfillmentChannel = amazon.FulfillmentChannel,
                    LatestDeliveryDate = amazon.LatestDeliveryDate,
                    LatestShipDate = amazon.LatestShipDate,
                    MarketplaceId = amazon.MarketplaceId,
                    OrderStatus = amazon.OrderStatus,
                    OrderType = amazon.OrderType,
                    PaymentMethod = amazon.PaymentMethod,
                    SalesChannel = amazon.SalesChannel,
                    ShipServiceLevel = amazon.ShipServiceLevel,
                    ShipmentServiceLevelCategory = amazon.ShipmentServiceLevelCategory,
                    Total = amazon.OrderTotal?.Amount + amazon.OrderTotal?.CurrencyCode,
                    IsSubmitted = false,
                    OrderItemList = MapItemOrderAmazon(amazon.OrderItemList)
                }).ToList();
            result.AddRange(lstAmz);

            return result;
        }

        public static List<ItemAmazon> MapItemOrderAmazon1(List<ItemSubmitAmazon> item)
        {
            return item.Select(x => new ItemAmazon()
            {
                ASIN = "",
                ItemPrice = x.Price,
                OrderItemId = "",
                QuantityOrdered = int.Parse(x.Qty),
                SellerSKU = x.Item,
                ShippingPrice = "",
                Title = null
            }).ToList();
        }
        public static List<AmazonOrderDto> MapQgoldToAmazon(List<AmazonOrderDto> listOrderAmazon, List<OrderMarketplaceDto> listOrderQgold)
        {
            return listOrderAmazon.Join(listOrderQgold, x => x.AmazonOrderId, y => y.Seller_Order_ID, (x, y) => new AmazonOrderDto
            {
                AmazonOrderId = x.AmazonOrderId,
                Supplier = "Qgold",
                SupplierOrderId = y.OrderId,
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
            try
            {
                if (orderItemList != null)
                {
                    foreach (var item in orderItemList)
                    {
                        result.Add(new ItemAmazon()
                        {
                            ASIN = item.ASIN,
                            ItemPrice = item.ItemPrice != null ? item.ItemPrice.Amount : "0",
                            OrderItemId = item.OrderItemId,
                            QuantityOrdered = item.QuantityOrdered,
                            SellerSKU = item.SellerSKU,
                            ShippingPrice = item.ShippingPrice != null ? item.ShippingPrice.Amount: "0",
                            Title = item.Title
                        });
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }
        public static string? CheckListItem(List<OrderItemList>? orderItemList = null)
        {
            try
            {
                if (orderItemList != null)
                {
                    if (orderItemList.Where(x => x.SellerSKU.StartsWith("S-")).Count() == orderItemList.Count())
                    {
                        return "Stuller";
                    }else if (orderItemList.Where(x => x.SellerSKU.StartsWith("Q-")).Count() == orderItemList.Count())
                    {
                        return "Qgold";
                    }
                    else
                    {
                        return "Many";
                    }
                }
                return null;

            }
            catch (Exception)
            {
                return null;
            }
        }


        public static QgoldOrderDto QgoldToQgoldOrderDTO(OrderQgold data)
        {
            return new QgoldOrderDto
            {
                OrderQgoldId = data.Order,
                BuyerName = data.InvoiceDetails[0].ShipToText,
                Address1 = data.InvoiceDetails[0].ShipToAddress1,
                Items = data.InvoiceDetails[0].QgoldInvoiceItems,
                MarketOrderId = null,
                Marketplace = null,
                OrderDate = data.Date,
                OrderStatus = data.Status,
                ShipVia = data.InvoiceDetails[0].ShipVia,
                Total = data.Total,
                Tracking = data.InvoiceDetails[0].Tracking,
                ZipCode = data.InvoiceDetails[0].ZipCode
            };
        }

        public static QgoldOrderDto MapReOrderToQgoldOrderDTO(OrderMarketplaceDto data)
        {
            return new QgoldOrderDto
            {
                OrderQgoldId = data.OrderId,
                BuyerName = data.ShipToName,
                Address1 = data.ShipToAddress1,
                OrderDate = data.TimeStamp.ToString("MM/dd/yyyy"),
                MarketOrderId = data.Seller_Order_ID,
                Marketplace = data.Marketplace,
                OrderStatus = null,
                ShipVia = null,
                Total = null,
                Tracking = null,
                ZipCode = null,
                Items = MapItem(data.Item),
            };
        }
        public static List<AmazonOrderDto> ConvertToAmazonOrderDTO(
            List<OrderAmazon> lstAmazon,
            List<OrderAmazonSubmitted> lstSubmit)
        {
            var result = new List<AmazonOrderDto>();

            var lstInnerJoin = (from amazon in lstAmazon
                                join submit in lstSubmit on amazon.AmazonOrderId equals submit.Amz_Order_ID
                                select new AmazonOrderDto()
                                {
                                    AmazonOrderId = amazon.AmazonOrderId,
                                    SupplierOrderId = submit.OrderId,
                                    BuyerEmail = amazon.BuyerInfo.BuyerEmail,
                                    BuyerName = amazon.ShippingAddress.Name,
                                    Phone = amazon.ShippingAddress.Phone,
                                    AddressLine1 = amazon.ShippingAddress.AddressLine1,
                                    AddressLine2 = amazon.ShippingAddress.AddressLine2,
                                    AddressLine3 = amazon.ShippingAddress.AddressLine3,
                                    StateOrRegion = amazon.ShippingAddress.StateOrRegion,
                                    City = amazon.ShippingAddress.City,
                                    CountryCode = amazon.ShippingAddress.CountryCode,
                                    PostalCode = amazon.ShippingAddress.PostalCode,
                                    PurchaseDate = amazon.PurchaseDate,
                                    EarliestDeliveryDate = amazon.EarliestDeliveryDate,
                                    EarliestShipDate = amazon.EarliestShipDate,
                                    FulfillmentChannel = amazon.FulfillmentChannel,
                                    LatestDeliveryDate = amazon.LatestDeliveryDate,
                                    LatestShipDate = amazon.LatestShipDate,
                                    MarketplaceId = amazon.MarketplaceId,
                                    OrderStatus = amazon.OrderStatus,
                                    OrderType = amazon.OrderType,
                                    PaymentMethod = amazon.PaymentMethod,
                                    SalesChannel = amazon.SalesChannel,
                                    ShipServiceLevel = amazon.ShipServiceLevel,
                                    ShipmentServiceLevelCategory = amazon.ShipmentServiceLevelCategory,
                                    Total = amazon.OrderTotal.Amount + amazon.OrderTotal.CurrencyCode,
                                    IsSubmitted = true,
                                    OrderItemList = MapItemOrderAmazon(amazon.OrderItemList)
                                }).ToList();
            result.AddRange(lstInnerJoin);
            var lstAmazOrderInner = lstInnerJoin.Select(x => x.AmazonOrderId).ToList();

            var lstAmz = lstAmazon.Where(x => !lstAmazOrderInner.Contains(x.AmazonOrderId))
                .Select(amazon => new AmazonOrderDto()
                {
                    AmazonOrderId = amazon.AmazonOrderId,
                    SupplierOrderId = null,
                    BuyerEmail = amazon.BuyerInfo.BuyerEmail,
                    BuyerName = amazon.ShippingAddress.Name,
                    Phone = amazon.ShippingAddress.Phone,
                    AddressLine1 = amazon.ShippingAddress.AddressLine1,
                    AddressLine2 = amazon.ShippingAddress.AddressLine2,
                    AddressLine3 = amazon.ShippingAddress.AddressLine3,
                    StateOrRegion = amazon.ShippingAddress.StateOrRegion,
                    City = amazon.ShippingAddress.City,
                    CountryCode = amazon.ShippingAddress.CountryCode,
                    PostalCode = amazon.ShippingAddress.PostalCode,
                    PurchaseDate = amazon.PurchaseDate,
                    EarliestDeliveryDate = amazon.EarliestDeliveryDate,
                    EarliestShipDate = amazon.EarliestShipDate,
                    FulfillmentChannel = amazon.FulfillmentChannel,
                    LatestDeliveryDate = amazon.LatestDeliveryDate,
                    LatestShipDate = amazon.LatestShipDate,
                    MarketplaceId = amazon.MarketplaceId,
                    OrderStatus = amazon.OrderStatus,
                    OrderType = amazon.OrderType,
                    PaymentMethod = amazon.PaymentMethod,
                    SalesChannel = amazon.SalesChannel,
                    ShipServiceLevel = amazon.ShipServiceLevel,
                    ShipmentServiceLevelCategory = amazon.ShipmentServiceLevelCategory,
                    Total = amazon.OrderTotal?.Amount + amazon.OrderTotal?.CurrencyCode,
                    IsSubmitted = false,
                    OrderItemList = MapItemOrderAmazon(amazon.OrderItemList)
                }).ToList();
            result.AddRange(lstAmz);

            var lstSub = lstSubmit.Where(x => !lstAmazOrderInner.Contains(x.Amz_Order_ID))
                .Select(amazon => new AmazonOrderDto()
                {
                    AmazonOrderId = amazon.Amz_Order_ID,
                    SupplierOrderId = amazon.OrderId,
                    BuyerEmail = null,
                    BuyerName = amazon.ShipToName,
                    Phone = amazon.ShipToPhone,
                    AddressLine1 = amazon.ShipToAddress1,
                    AddressLine2 = amazon.ShipToAddress2,
                    AddressLine3 = amazon.ShipToAddress3,
                    StateOrRegion = amazon.ShipToState_Province,
                    City = amazon.ShipToCity,
                    CountryCode = amazon.ShipToCountryCode,
                    PostalCode = amazon.ShipToZip,
                    PurchaseDate = amazon.TimeStamp,
                    EarliestDeliveryDate = null,
                    EarliestShipDate = null,
                    FulfillmentChannel = null,
                    LatestDeliveryDate = null,
                    LatestShipDate = null,
                    MarketplaceId = null,
                    OrderStatus = "Shipped",
                    OrderType = $"ShipMethod:{amazon.Ship_Method_Code}",
                    PaymentMethod = "",
                    SalesChannel = $"{amazon.ShipToCountryCode}.com",
                    ShipServiceLevel = $"ShipOption:{amazon.Ship_Option_Code}",
                    ShipmentServiceLevelCategory = null,
                    Total = "",
                    IsSubmitted = true,
                    OrderItemList = MapItemOrderAmazon1(amazon.Item)
                }).ToList();
            result.AddRange(lstSub);

            return result;
        }
    }
}
