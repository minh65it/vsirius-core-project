using OrderProcessApi.DTOs;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;
using OrderProcessDomain.Stuller;
using OrderProcessDomain.Walmart;

namespace OrderProcessApi.Utilities.Mapping
{
    public static class CustomMapping
    {
        public static PaginationResult<QgoldOrderDTO> ConvertToQgoldOrderDTO(
            PaginationResult<OrderQgold> pagOrderUIQgold, 
            List<OrderMarketplace> lstMarketplace, 
            List<OrderMarketplace> lostPackage, 
            bool isMarket)
        {
            if (lstMarketplace != null && lstMarketplace.Any())
            {
                lstMarketplace = lstMarketplace
                .GroupBy(item => item.OrderId)
                .Select(group => group.First())
                .ToList();
            }
            var result = new PaginationResult<QgoldOrderDTO>()
            {
                PageCount = pagOrderUIQgold.PageCount,
                PageNumber = pagOrderUIQgold.PageNumber,
                PageSize = pagOrderUIQgold.PageSize,
                TotalCount = pagOrderUIQgold.TotalCount,
                Data = new List<QgoldOrderDTO>(),
            };
            result.Data = pagOrderUIQgold.Data.Select(x=> new QgoldOrderDTO()
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

            //var result = new PaginationResult<QgoldOrderDTO>()
            //{
            //    Data = new List<QgoldOrderDTO>(),
            //    PageCount = pagOrderUIQgold.PageCount,
            //    PageNumber = pagOrderUIQgold.PageNumber,
            //    PageSize = pagOrderUIQgold.PageSize,
            //    TotalCount = pagOrderUIQgold.TotalCount
            //};
            //if (!isMarket)
            //{
            //    result.Data = (from UIPathOrder in pagOrderUIQgold.Data
            //                   join market in lstMarketplace! on UIPathOrder.Order equals market.OrderId into joinTable
            //                   from lastResult in joinTable.DefaultIfEmpty()
            //                   select new QgoldOrderDTO()
            //                   {
            //                       OrderQgoldId = UIPathOrder.Order,
            //                       BuyerName = UIPathOrder.InvoiceDetails[0].ShipToText,
            //                       OrderStatus = UIPathOrder.Status,
            //                       OrderDate = UIPathOrder.Date,
            //                       ShipVia = UIPathOrder.InvoiceDetails[0].ShipVia,
            //                       Tracking = UIPathOrder.InvoiceDetails[0].Tracking,
            //                       Address1 = UIPathOrder.InvoiceDetails[0].ShipToAddress1,
            //                       ZipCode = UIPathOrder.InvoiceDetails[0].ZipCode,
            //                       Total = UIPathOrder.Total,
            //                       MarketOrderId = lastResult?.Seller_Order_ID,
            //                       Marketplace = lastResult?.Marketplace,
            //                       Items = UIPathOrder.InvoiceDetails[0].QgoldInvoiceItems
            //                   }).ToList();
            //}
            //else
            //{
            //    result.Data = (from market in lstMarketplace
            //                   join UIPathOrder in pagOrderUIQgold.Data on market.OrderId equals UIPathOrder.Order
            //                   //into joinTable
            //                   //where !string.IsNullOrEmpty(market.OrderId)
            //                   //from lastResult in joinTable.DefaultIfEmpty()
            //                   select new QgoldOrderDTO()
            //                   {
            //                       OrderQgoldId = market.OrderId,
            //                       BuyerName = market.ShipToName,
            //                       OrderStatus = UIPathOrder?.Status,
            //                       OrderDate = UIPathOrder?.Date,
            //                       ShipVia = UIPathOrder?.InvoiceDetails[0]?.ShipVia,
            //                       Tracking = UIPathOrder?.InvoiceDetails[0]?.Tracking,
            //                       Address1 = market.ShipToAddress1,
            //                       ZipCode = market.ShipToZip.Split('-')[0],
            //                       Total = UIPathOrder?.Total,
            //                       MarketOrderId = market.Seller_Order_ID,
            //                       Marketplace = market.Marketplace,
            //                       Items = UIPathOrder.InvoiceDetails[0].QgoldInvoiceItems
            //                   }).ToList();
            //    result = Common.CreatePagination(result.Data, result.PageNumber, result.PageSize);
            //}

            return result;
        }

        public static QgoldOrderDetailDTO ConvertToDetailOrder(OrderQgold orderUI, OrderMarketplace? orderMarketplace, OrderMarketplace? lostPackage)
        {
            var result = new QgoldOrderDetailDTO()
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

        private static List<ItemQgold> MapItem(List<OrderMarketplaceItem> items)
        {
            var result = new List<ItemQgold>();
            items.ForEach(item =>
            {
                result.Add(new ItemQgold()
                {
                    Style = item.Item,
                    Price = item.Price,
                    Qty = item.Qty,
                });
            });

            return result;
        }

        public static List<AmazonOrderDTO> ConvertToAmazonOrderDTO(
            List<OrderAmazonSeller> lstAmazon,
            List<OrderAmazonSubmitted> lstSubmit)
        {
            var result = new List<AmazonOrderDTO>();

            var lstInnerJoin = (from amazon in lstAmazon
                                join submit in lstSubmit on amazon.AmazonOrderId equals submit.Amz_Order_ID
                                select new AmazonOrderDTO()
                                {
                                    AmazonOrderId = amazon.AmazonOrderId,
                                    QgoldOrderId = submit.OrderId,
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
                .Select(amazon => new AmazonOrderDTO()
                {
                    AmazonOrderId = amazon.AmazonOrderId,
                    QgoldOrderId = null,
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
                    IsSubmitted = false,
                    OrderItemList = MapItemOrderAmazon(amazon.OrderItemList)
                }).ToList();
            result.AddRange(lstAmz);

            var lstSub = lstSubmit.Where(x => !lstAmazOrderInner.Contains(x.Amz_Order_ID))
                .Select(amazon => new AmazonOrderDTO()
                {
                    AmazonOrderId = amazon.Amz_Order_ID,
                    QgoldOrderId = amazon.OrderId,
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

        public static List<WalmartOrderDetailDTO> ConvertToWalmartOrderDTO(List<WalmartOrder> data, List<OrderMarketplace> lstQgold)
        {
            var result = new List<WalmartOrderDetailDTO>();

            foreach (var item in data)
            {
                result.Add(new WalmartOrderDetailDTO()
                {
                    customerOrderId = item.customerOrderId,
                    purchaseOrderId = item.purchaseOrderId,
                    BuyerName = item.shippingInfo.postalAddress.name,
                    Address1 = item.shippingInfo.postalAddress.address1,
                    Address2 = item.shippingInfo.postalAddress.address2,
                    City = item.shippingInfo.postalAddress.city,
                    Country = item.shippingInfo.postalAddress.country,
                    customerEmailId = item.customerEmailId,
                    Phone = item.shippingInfo.phone,
                    estimatedDeliveryDate = Common.ConvertEpochToDateTime(item.shippingInfo.estimatedDeliveryDate),
                    estimatedShipDate = Common.ConvertEpochToDateTime(item.shippingInfo.estimatedShipDate),
                    IsSubmitted = null,
                    methodShipping = item.shippingInfo.methodCode,
                    orderDate = Common.ConvertEpochToDateTime(item.orderDate),
                    PostalCode = item.shippingInfo.postalAddress.postalCode,
                    QgoldOrderId = lstQgold.FirstOrDefault(x => x.Seller_Order_ID == item.purchaseOrderId)?.OrderId,
                    State = item.shippingInfo.postalAddress.state,
                    CarrierName = item.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.carrierName?.carrier,
                    TrackingNumber = item.orderLines?.orderLine[0]?.orderLineStatuses?.orderLineStatus[0]?.trackingInfo?.trackingNumber,
                    Items = MapItemWalmart(item.orderLines!.orderLine)
                });
            }

            return result;
        }

        public static List<WalmartItemDTO> MapItemWalmart(List<OrderLine> orderLine)
        {
            var result = new List<WalmartItemDTO>();

            foreach (var item in orderLine)
            {
                result.Add(new WalmartItemDTO()
                {
                    OrderStatus = item.orderLineStatuses.orderLineStatus[0].status,
                    Price = item.charges?.charge[0]?.chargeAmount?.amount,
                    ProductName = item.item.productName,
                    quantity = item.orderLineQuantity.amount,
                    Sku = item.item.sku,
                    Tax = item.charges?.charge[0]?.tax?.taxAmount?.amount
                });
            }

            return result;
        }

        public static QgoldOrderDTO QgoldToQgoldOrderDTO(OrderQgold data)
        {
            return new QgoldOrderDTO{
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

        public static QgoldOrderDTO MapReOrderToQgoldOrderDTO(OrderMarketplace data)
        {
            return new QgoldOrderDTO
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
    }
}
