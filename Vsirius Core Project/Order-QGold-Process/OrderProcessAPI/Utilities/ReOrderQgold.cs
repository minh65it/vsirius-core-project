using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi.Utilities
{
    public interface IReOrderQgold
    {
        Task<List<AmazonOrderInfo>> CheckOrderDuplicate(List<AmazonOrderInfo> listInput);
        Task<bool> SendMailReOrder(AmazonOrderInfo info);
    }

    public class ReOrderQgold : IReOrderQgold
    {
        private readonly IQgoldOrderMarketplaceService _qgoldOrderMarketplaceService;
        private readonly ISendMailService _iEmailHandler;
        private readonly IConfiguration _configuration;

        public ReOrderQgold(IQgoldOrderMarketplaceService qgoldOrderMarketplaceService, 
            ISendMailService iEmailHandler, IConfiguration configuration)
        {
            _qgoldOrderMarketplaceService = qgoldOrderMarketplaceService;
            _iEmailHandler = iEmailHandler;
            _configuration = configuration;
        }

        public async Task<List<AmazonOrderInfo>> CheckOrderDuplicate(List<AmazonOrderInfo> listInput)
        {
            try
            {
                List<AmazonOrderInfo> result = new List<AmazonOrderInfo>();
                if (listInput.Count == 0)
                    return result;
                var orderDB = await _qgoldOrderMarketplaceService.GetRecentOrder(-2);
                for (int i = 0; i < listInput.Count(); i++)
                {
                    // TH1 check trong tsv file gửi
                    if(listInput.Where(x => x.ShipToName == listInput[i].ShipToName
                        && x.ShipToAddress1 == listInput[i].ShipToAddress1
                        && x.ShipToAddress2 == listInput[i].ShipToAddress2
                        && x.Item == listInput[i].Item
                        && x.Qty == listInput[i].Qty
                        && x.ShipToZip == listInput[i].ShipToZip
                        && x.Amz_Order_ID != listInput[i].Amz_Order_ID).ToList().Count() > 0)
                    {
                        result.Add(listInput[i]);
                    }

                    // TH2 check trong DB đã submit cách đây 2 ngày
                    var checkItem = orderDB.Where(x => x.ShipToName == listInput[i].ShipToName
                        && x.ShipToAddress1 == listInput[i].ShipToAddress1
                        && x.ShipToAddress2 == listInput[i].ShipToAddress2
                        && x.ShipToZip == listInput[i].ShipToZip
                        && x.Seller_Order_ID != listInput[i].Amz_Order_ID).FirstOrDefault();


                    if (checkItem != null)
                    {
                        if (checkItem.Item.Count > 0)
                        {
                            foreach (var j in checkItem.Item)
                            {
                                if (j.Qty == listInput[i].Qty && j.Item == listInput[i].Item)
                                {
                                    result.Add(listInput[i]);
                                }
                            }
                        }
                    }
                }
                List<AmazonOrderInfo> resultSendMail = result.GroupBy(x => new { x.ShipToName, x.ShipToAddress1, x.ShipToAddress2, x.ShipToZip, x.Item, x.Qty })
                                                            .Select(g => g.First()).ToList();
                foreach (var item in resultSendMail)
                {
                    await SendMailReOrder(item);
                }
                return resultSendMail;
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Check Order Duplicate error: {ex.ToString()}", $"CheckOrderDuplicate_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new List<AmazonOrderInfo>();
            }
        }

        public async Task<bool> SendMailReOrder(AmazonOrderInfo info)
        {
            try
            {
                var _from = _configuration.GetSection("Email:From").Value!;
                var _to = _configuration.GetSection("Email:To").Value!;

                var qgold_id = info.Item;
                var subject = "Please help me add one more order for the product item " + qgold_id;
                var body = "Hi, <br /><br />"
                            + "Please help me add one more order for the product item " + qgold_id + " that being shipped to: <br />"
                            + info.ShipToName + "<br />"
                            + info.ShipToAddress1 + "<br />"
                            + (info.ShipToAddress2 ?? info.ShipToAddress2 + "<br />")
                            + info.ShipToCity + ", " + info.ShipToState_Province + " " + info.ShipToZip + "<br />" + info.ShipToCountryCode  +"<br />" 
                            + "Because your system is reported as duplicate, I have to send this email. If your system don't report please skip this email" + "<br /> <br /> Thank you";


                var result = await _iEmailHandler.SendToSingleEmail(_from, _to, subject, body);
                if (result.IsSuccess)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Send Mail ReOrder error: {ex.ToString()}", $"SendMail_ReOrder_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return false;
            }
           
        }
    }
}
