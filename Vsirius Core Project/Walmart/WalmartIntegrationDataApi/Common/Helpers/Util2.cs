using MongoDB.Driver;
using WalmartIntegrationDataApi.DtosOrderStuller;

namespace StullerOrderProcessAPI.Common.Helpers
{
    public static class Utils2
    {
        public static string _logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Logs");

        public static void WriteLog(string Message)
        {
            CreateFolder(_logPath);
            string filePath = Path.Combine(_logPath, $"Log_{DateTime.Now:yyyyMMdd}.txt");

            try
            {
                StreamWriter writer = File.AppendText(filePath);
                if (!string.IsNullOrWhiteSpace(Message))
                {
                    writer.WriteLine($"[{DateTime.Now}] {Message}");
                    Console.WriteLine($"[{DateTime.Now}] {Message}");
                }
                writer.Flush();
                writer.Close();
            }
            catch (IOException)
            {
                Task.Delay(10).Wait();
                WriteLog(Message);
            }
        }

        public static void CreateFolder(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static void AddOrFilter<T>(
           ref FilterDefinition<T>? filter,
           FilterDefinition<T> filterDefinition)
        {
            if (filter == null)
            {
                filter = filterDefinition;
            }
            else
            {
                filter |= filterDefinition;
            }
        }

        public static void AddAndFilter<T>(
            ref FilterDefinition<T>? filter,
            FilterDefinition<T>? filterDefinition)
        {
            if (filter == null)
            {
                filter = filterDefinition;
            }
            else if (filterDefinition != null)
            {
                filter &= filterDefinition;
            }
        }

        internal static StullerOrder ConvertResponseStullerOrderToEntity(
            OrderStatusStuller itemApi,
            UnshipOrderDto? objOrderMarketplace)
        {
            return new()
            {
                OrderID = itemApi.OrderNumber,
                PO = itemApi.PurchaseOrderNumber,
                Account = null,
                Billing = null,
                Address1 = null,
                BuyerName = null,
                ZipCode = null,
                Confirmation = null,
                EstTotalPrice = itemApi.InvoiceTotal.ToString(),
                OrderDate = itemApi.OrderDate.ToShortDateString(),
                Shipping = itemApi.ShipMethod,
                ShippingMethod = itemApi.ShipMethod,
                TrackOrder = new()
                {
                    new()
                    {
                        Tracking = itemApi.TrackingNumber,
                        Carrier = itemApi.ShipMethod,
                        ShipDate = itemApi.ActualShipDate.ToShortDateString(),
                        DeliveryDate = itemApi.InvoiceDate.ToShortDateString(),
                        Status = itemApi.Status,
                        Invoice = itemApi.OrderNumber
                    }
                },
                Marketplace = null,
                MarketplaceOrderId = null,
                Status = itemApi.Status,
                UrlDetail = null,
                Items = (itemApi.OrderStatusDetails != null && itemApi.OrderStatusDetails.Any()
                ? itemApi.OrderStatusDetails.Select(ConvertItemReponseToEntityItem).ToList()
                : objOrderMarketplace?.ItemUnShip.Select(ConvertItemReponseToEntityItem).ToList()
                ),
                UpdatedDate = DateTime.Now.ToString("yyyyMMddhhMMssfff")
            };
        }

        private static ItemStuller ConvertItemReponseToEntityItem(OrderStatusDetail x)
        {
            return new ItemStuller()
            {
                ProductSku = GetOriginStullerSku(x.ItemNumber ?? ""),
                EstPrice = x.UnitPrice.ToString(),
                Ordered = x.OrderQuantity.ToString(),
            };
        }

        public static string GetOriginStullerSku(string input)
        {
            const string prefix = "S-";

            if (input.StartsWith(prefix))
            {
                return input.Substring(prefix.Length);
            }
            else
            {
                return input;
            }
        }

        private static ItemStuller ConvertItemReponseToEntityItem(ItemUnShip x)
        {
            return new ItemStuller()
            {
                ProductSku = GetOriginStullerSku(x.Sku ?? ""),
                EstPrice = null,
                Ordered = x.Quantity,
            };
        }
    }
}
