using AmazonIntegrationDataApi.Dtos.OrderAmazonProcessor;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Models.OrderAmazonProcessor.Providers;
using System.Linq;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.SubmitOrderToQgold
{
    public static class MappingAndReverse
    {
        public static List<OrderQgold> QgoldDtoToQgold(List<QgoldOrderDto> input)
        {
            if(input.Any())
            {
                var result = input.Select(x => new OrderQgold()
                {
                    Order = x.OrderQgoldId,
                    InvoiceDetails = new List<InvoiceDetail>() { DetailQgoldDtoToQgold (x)},
                    Status = x.OrderStatus,
                    Date = x.OrderDate,
                    Total = x.Total,

                }).ToList();
                return result;
            }
            return null;
        }
        public static InvoiceDetail DetailQgoldDtoToQgold(QgoldOrderDto input)
        {
            if(input is not null)
            {
                return new InvoiceDetail()
                {
                    ShipToText = input.BuyerName,
                    ShipVia = input.ShipVia,
                    Tracking = input.Tracking,
                    ShipToAddress1 = input.Address1,
                    ZipCode = input.ZipCode,
                    QgoldInvoiceItems = input.Items
                };
            }
            return null;
        }
    }
}
