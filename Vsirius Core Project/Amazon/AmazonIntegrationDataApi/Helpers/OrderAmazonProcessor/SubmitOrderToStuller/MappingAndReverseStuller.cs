using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.OrderStuller;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToStuller
{
    public static class MappingAndReverseStuller
    {
        public static List<OrderQgold> StullerDtoToQgold(List<StullerOrderDTO> input)
        {
            if (input.Any())
            {
                var result = input.Select(x => new OrderQgold()
                {
                    Order = x.OrderID,
                    InvoiceDetails = new List<InvoiceDetail>() { DetailStullerDtoToQgold(x) },
                    Status = x.Status,
                    Date = x.OrderDate,

                }).ToList();
                return result;
            }
            return null;
        }
        public static InvoiceDetail DetailStullerDtoToQgold(StullerOrderDTO input)
        {
            if (input is not null)
            {
                return new InvoiceDetail()
                {
                    ShipToText = input.BuyerName,
                    Tracking = input.Tracking,
                    ShipToAddress1 = input.Address1,
                    ZipCode = input.ZipCode,
                    QgoldInvoiceItems = DetailStullerItemDtoToQgoldInvoiceItems(input.Items)
                };
            }
            return null;
        }
        public static List<QgoldInvoiceItem> DetailStullerItemDtoToQgoldInvoiceItems(List<ItemStuller> input)
        {
            if (input.Any())
            {
                var result = input.Select(x => new QgoldInvoiceItem()
                {
                    Style = x.ProductSku.Split('\n')[0],
                    Qty = x.Ordered,
                    Price = x.EstPrice,

                }).Where(x => x.Style != "SHIPPING ESTIMATES").ToList();
                return result;
            }
            return null;
        }
    }
}
