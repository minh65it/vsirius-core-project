using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.Utilities;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;
using System.Net;
using System.Text;

namespace OrderProcessApi._Services.Services.Qgold
{
    public class SubmitOrderQgoldService : ISubmitOrderQgoldService
    {
        private readonly IConfiguration _configuration;
        private readonly IQgoldShippingInstruction _iQgoldShippingInstruction;
        private readonly IQgoldOrderMarketplaceService _qgoldOrderMarketplaceService;
        private readonly IReOrderQgold _reOrderQgold;
        private readonly IReSubmitOrderService _reSubmitOrderService;
        private readonly string? _ftpUrl = "";
        private readonly string? _qgoldAccountId = "";
        private readonly string? _userName = "";
        private readonly string? _password = "";

        public SubmitOrderQgoldService(IConfiguration configuration, IQgoldShippingInstruction iQgoldShippingInstruction,
            IQgoldOrderMarketplaceService qgoldOrderMarketplaceService, IReOrderQgold reOrderQgold, IReSubmitOrderService reSubmitOrderService)
        {
            _configuration = configuration;
            _ftpUrl = _configuration.GetSection("Qgold:FtpUrl").Value;
            _qgoldAccountId = _configuration.GetSection("Qgold:AccountId").Value;
            _userName = _configuration.GetSection("Qgold:UserName").Value;
            _password = _configuration.GetSection("Qgold:Password").Value;

            _iQgoldShippingInstruction = iQgoldShippingInstruction;
            _qgoldOrderMarketplaceService = qgoldOrderMarketplaceService;
            _reOrderQgold = reOrderQgold;
            _reSubmitOrderService = reSubmitOrderService;
        }

        public async Task<bool> CreateFile(string textContent, string fileName)
        {
            try
            {
                string filePath = _ftpUrl + fileName;

                // Get the object used to communicate with the server.
                FtpWebRequest request = WebRequest.Create(filePath) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential(_userName, _password);

                // convert contents to byte.
                byte[] fileContents = Encoding.ASCII.GetBytes(textContent);

                request.ContentLength = fileContents.Length;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
                }

                FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync();

                if (response.StatusCode == FtpStatusCode.CommandOK || response.StatusCode == FtpStatusCode.FileActionOK || response.StatusCode == FtpStatusCode.ClosingData)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {
                var exx = ex.Message;
                return false;
            }
        }
        public async Task<bool> SubmitQgold(List<QgoldFtpOrderObject> orders, bool? isResubmit)
        {
            if (orders.Count() == 0)
            {
                return false;
            }
            string text = "";
            DateTime timeStamp = DateTime.Now;
            List<AmazonOrderInfo> listSignature = new List<AmazonOrderInfo>();
            List<AmazonOrderInfo> listCheckReOrder = new List<AmazonOrderInfo>();
            foreach (var order in orders)
            {
                if (order.Item == null)
                    continue;
                order.Format = "5";
                order.Customer = "58772";
                order.TimeStamp = timeStamp;
                foreach (var item in order.Item)
                {
                    string lineTmp = order.Format + "\t"
                                + order.Customer + "\t"
                                + order.PO + "\t"
                                + order.ShipToName + "\t"
                                + order.ShipToAddress1 + "\t"
                                + order.ShipToAddress2 + "\t"
                                + order.ShipToAddress3 + "\t"
                                + order.ShipToCity + "\t"
                                + order.ShipToState_Province + "\t"
                                + order.ShipToZip + "\t"
                                + order.ShipToCountryCode + "\t"
                                + order.ShipToPhone + "\t"
                                + item.Item + "\t"
                                + item.Size + "\t"
                                + order.VSiriusItem + "\t"
                                + item.Qty + "\t"
                                + null + "\t"
                                + order.Description + "\t"
                                + order.Ship_Method_Code + "\t"
                                + order.Ship_Option_Code + "\t"
                                + order.Gift_Message + "\t"
                                + order.Cust_Order_Number + "\t"
                                + order.Amz_Order_ID + "\t"
                                + order.Amz_Item_ID + "\r\n";
                    text += lineTmp;

                    try
                    {
                        // Add listCheckReOrder
                        listCheckReOrder.Add(new AmazonOrderInfo()
                        {
                            OrderId = null,
                            Item = item.Item,
                            ShipToName = order.ShipToName,
                            ShipToAddress1 = order.ShipToAddress1,
                            ShipToAddress2 = order.ShipToAddress2,
                            ShipToCity = order.ShipToCity,
                            ShipToState_Province = order.ShipToState_Province,
                            ShipToZip = order.ShipToZip,
                            ShipToCountryCode = order.ShipToCountryCode,
                            Qty = item.Qty,
                            Price = item.Price,
                            Amz_Order_ID = order.Amz_Order_ID,
                        });

                        //Add listSignature
                        var total = double.Parse(item.Price) * double.Parse(item.Qty);
                        if (total > 30)
                        {
                            listSignature.Add(new AmazonOrderInfo()
                            {
                                OrderId = null,
                                Item = item.Item,
                                ShipToName = order.ShipToName,
                                ShipToAddress1 = order.ShipToAddress1,
                                ShipToAddress2 = order.ShipToAddress2,
                                ShipToCity = order.ShipToCity,
                                ShipToState_Province = order.ShipToState_Province,
                                ShipToZip = order.ShipToZip,
                                ShipToCountryCode = order.ShipToCountryCode,
                                TotalPrice = total,
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        await Common.WriteLogAsync($"Add listSignature error: {ex.ToString()}", $"Add_listSignature_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                    }

                }
            }

            var now = DateTime.Now;

            //string filename = _qgoldAccountId + $"_{orders[0].Marketplace}_" + now.ToString("MMddyyHHmm") + ".txt";
            //string filename2 = _qgoldAccountId + "_" + now.ToString("MMddyyHHmm") + ".txt";
            //await Common.WriteLogAsync(text, filename);
            //var result = await CreateFile(text, filename2);


            string filename = "";
            if (orders[0].Marketplace.Contains("Amazon"))
            {
                filename = $"{_qgoldAccountId}_1{(isResubmit == true ? "R" : "")}_{now:MMddyyHHmm}.txt";
            }
            else if (orders[0].Marketplace.Contains("Walmart"))
            {
                filename = $"{_qgoldAccountId}_2{(isResubmit == true ? "R" : "")}_{now:MMddyyHHmm}.txt";
            }
            else if (orders[0].Marketplace.Contains("Vsirius"))
            {
                filename = $"{_qgoldAccountId}_3{(isResubmit == true ? "R" : "")}_{now:MMddyyHHmm}.txt";
            }
            else if (orders[0].Marketplace.Contains("Ebay"))
            {
                filename = $"{_qgoldAccountId}_4{(isResubmit == true ? "R" : "")}_{now:MMddyyHHmm}.txt";
            }
            else if (orders[0].Marketplace.Contains("Etsy"))
            {
                filename = $"{_qgoldAccountId}_5{(isResubmit == true ? "R" : "")}_{now:MMddyyHHmm}.txt";
            }
            else
            {
                filename = $"{_qgoldAccountId}_{(isResubmit == true ? "R_" : "_")}{now:MMddyyHHmm}.txt";
            }
            await Common.WriteLogAsync(text, filename);
            var result = await CreateFile(text, filename);
            //var result = true;
            if (result)
            {
                bool checkResult = false;
                if (listSignature.Any())
                {
                    checkResult = await _iQgoldShippingInstruction.RandomOrder(listSignature);
                }
                await _reOrderQgold.CheckOrderDuplicate(listCheckReOrder);
                checkResult = await _qgoldOrderMarketplaceService.AddOrder(orders);
                if (isResubmit == true)
                {
                    await _reSubmitOrderService.AddReSubmitOrder(orders);
                }
                return checkResult;
            }
            else
            {
                return false;
            }
        }
    }
}
