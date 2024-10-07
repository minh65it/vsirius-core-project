using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi._Services.Services.Qgold
{
    public class PackageClaimService : IPackageClaimService
    {
        IQgoldRepository _qgoldDB;
        private readonly IConfiguration _configuration;
        private readonly ISendMailService _iEmailHandler;
        private readonly string _lostPackageColleciton = "";

        public PackageClaimService(
            IQgoldRepository qgoldDB,
            IConfiguration configuration,
            ISendMailService iEmailHandler)
        {
            _qgoldDB = qgoldDB;
            _configuration = configuration;
            _iEmailHandler = iEmailHandler;
            _lostPackageColleciton = _configuration.GetSection("Database:QgoldLostPackage").Value!;
        }

        public async Task<OperationResult> SendMailLostPackage(QgoldOrderDetailDTO order)
        {
            try
            {
                if(order.OrderQgoldId == null || order.ShipToPhone == null)
                {
                    return new OperationResult() { Success = false, Message = "Order Qgold Id or Phone is blank" };
                }
                OrderMarketplace orderAdd = new OrderMarketplace() { 
                    OrderId = order.OrderQgoldId,
                    ShipToPhone = order.ShipToPhone,
                    ShipToName = order.ShipToText,
                    Marketplace = order.Marketplace,
                    ShipToAddress1 = order.ShipToAddress1,
                    Seller_Order_ID = order.MarketOrderId,
                    PO = order.POQgold,
                    ShipToZip = order.ZipCode,
                    TimeStamp = DateTime.Now,
                };
                var _from = _configuration.GetSection("Email:From").Value!;
                var _to = _configuration.GetSection("Email:To").Value!;

                var subject = "Lost Package Claim";
                var body = "Hi, <br /><br />"
                            + "Customer of the order " + orderAdd.OrderId + " claimed that she did not receive the package yet. <br />"
                            + "Please advise and support to file the claim with the phone number: " + orderAdd.ShipToPhone + ". <br /> "
                            + "<br /> <br /> Thank you";


                var result = await _iEmailHandler.SendToSingleEmail(_from, _to, subject, body);
                if (result.IsSuccess)
                {
                    await _qgoldDB.InsertOne(orderAdd, _lostPackageColleciton);
                    return new OperationResult() { Success = true, Message = "Send mail succeeded" };
                }
                else
                {
                    return new OperationResult() { Success = false, Message = "Send mail error" };
                }
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Send Mail Lost Package error: {ex.ToString()}", $"SendMailLostPackage_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new OperationResult() { Success = false, Message = "System send mail error" };
            }
        }
    }
}
