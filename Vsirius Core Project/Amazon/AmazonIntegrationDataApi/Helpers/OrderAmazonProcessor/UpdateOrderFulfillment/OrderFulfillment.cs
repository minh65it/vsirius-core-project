using FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds;
using FikaAmazonAPI.ConstructFeed.Messages;
using FikaAmazonAPI.ConstructFeed;
using FikaAmazonAPI;
using static FikaAmazonAPI.Utils.Constants;
using AmazonIntegrationDataApi.Helpers.Ultilities;

namespace AmazonIntegrationDataApi.Helpers.OrderAmazonProcessor.UpdateOrderFulfillment
{
    public interface IOrderFulfillment
    {
        public void FeedPostOrderFulfillment(AmazonConnection amazonConnection, List<OrderFulfillmentMessage> list);
    }
    public class OrderFulfillment : IOrderFulfillment
    {
        public void FeedPostOrderFulfillment(AmazonConnection amazonConnection, List<OrderFulfillmentMessage> list)
        {
            ConstructFeedService createDocument = new ConstructFeedService(amazonConnection.GetCurrentSellerID, "1.02");

            //var list1 = new List<OrderFulfillmentMessage>();

            //list1.Add(new OrderFulfillmentMessage()
            //{
            //    AmazonOrderID = "112-3219232-0097039",
            //    FulfillmentDate = DateTime.Now.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK"),
            //    FulfillmentData = new FulfillmentData()
            //    {

            //        CarrierName = "USPS", //người cung cấp dich vụ
            //        ShippingMethod = "USPS Frst Class",
            //        ShipperTrackingNumber = "9400111206209360541138",
            //    }
            //});
            createDocument.AddOrderFulfillmentMessage(list);

            var xml = createDocument.GetXML();

            var feedID = amazonConnection.Feed.SubmitFeed(xml, FeedType.POST_ORDER_FULFILLMENT_DATA);
            GetFeedDetails(feedID, amazonConnection);
        }

        private void GetFeedDetails(string feedID, AmazonConnection amazonConnection)
        {
            string ResultFeedDocumentId = string.Empty;
            while (string.IsNullOrEmpty(ResultFeedDocumentId))
            {
                var feedOutput = amazonConnection.Feed.GetFeed(feedID);
                if (feedOutput.ProcessingStatus == Feed.ProcessingStatusEnum.DONE)
                {
                    var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

                    var reportOutput = outPut.Url;
                    Utilities2.WriteLog($"{DateTime.Now} - GetFeedDetails() " + reportOutput);
                    var processingReport = amazonConnection.Feed.GetFeedDocumentProcessingReport(outPut);

                    DisplayProcessingReportMessage(processingReport);

                    break;
                }

                if (!(feedOutput.ProcessingStatus == Feed.ProcessingStatusEnum.INPROGRESS ||
                    feedOutput.ProcessingStatus == Feed.ProcessingStatusEnum.INQUEUE))
                    break;
                else Thread.Sleep(10000);
            }
        }
        private void DisplayProcessingReportMessage(ProcessingReportMessage processingReport)
        {
            Console.WriteLine("MessagesProcessed=" + processingReport.ProcessingSummary.MessagesProcessed);
            Console.WriteLine("MessagesSuccessful= " + processingReport.ProcessingSummary.MessagesSuccessful);
            Console.WriteLine("MessagesWithError=" + processingReport.ProcessingSummary.MessagesWithError);
            Console.WriteLine("MessagesWithWarning=" + processingReport.ProcessingSummary.MessagesWithWarning);

            if (processingReport.Result != null && processingReport.Result.Count > 0)
            {
                foreach (var itm in processingReport.Result)
                {
                    Console.WriteLine("ResultDescription=" + (itm.AdditionalInfo?.SKU ?? string.Empty) + " > " + itm.ResultDescription);
                }
            }
        }
    }
}
