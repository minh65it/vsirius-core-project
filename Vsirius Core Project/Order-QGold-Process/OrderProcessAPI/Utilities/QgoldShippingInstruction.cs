using OrderProcessDomain.Qgold;

namespace OrderProcessApi.Utilities
{
    public interface IQgoldShippingInstruction
    {
        Task<bool> RandomOrder(List<AmazonOrderInfo> info);
        Task<bool> SendShippingInstruction(AmazonOrderInfo info);
    }
    public class QgoldShippingInstruction : IQgoldShippingInstruction
    {
        private readonly ISendMailService _iEmailHandler;
        private readonly IConfiguration _configuration;
        public QgoldShippingInstruction(ISendMailService iEmailHandler, IConfiguration configuration)
        {
            _iEmailHandler = iEmailHandler;
            _configuration = configuration;
        }
        public async Task<bool> RandomOrder(List<AmazonOrderInfo> info)
        {
            try
            {
                foreach (var item in info)
                {
                    if (item.TotalPrice >= 100.00)
                    {
                        await SendShippingInstruction(item);
                    }
                    else
                    {
                        if (item.TotalPrice > 30)
                        {
                            Random ran = new Random();

                            int num = ran.Next(100);
                            if (num > 50)
                            {
                                await SendShippingInstruction(item);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> SendShippingInstruction(AmazonOrderInfo info)
        {
            var _from = _configuration.GetSection("Email:From").Value!;
            var _to = _configuration.GetSection("Email:To").Value!;

            var qgold_id = info.Item;
            var subject = "Add signature requirement for order item " + qgold_id;
            var body = "Hi, <br /><br />"
                        + "Please add signature requirement for order item " + qgold_id + " that being shipped to: <br />"
                        + info.ShipToName + "<br />"
                        + info.ShipToAddress1 + "<br />"
                        + (info.ShipToAddress2 ?? info.ShipToAddress2 + "<br />")
                        + info.ShipToCity + ", " + info.ShipToState_Province + " " + info.ShipToZip + "<br />" + info.ShipToCountryCode + "<br /> <br /> Thank you";


            var result = await _iEmailHandler.SendToSingleEmail(_from, _to, subject, body);
            if(result.IsSuccess) 
            {
                await Common.WriteLogAsync($"Send mail Add signature item SUCCESSFUL: {qgold_id}");
                return true;
            }
            else
            {
                await Common.WriteLogAsync($"Send mail Add signature item ERROR: {qgold_id}");
                return false;
            }
                
        }
    }
}
