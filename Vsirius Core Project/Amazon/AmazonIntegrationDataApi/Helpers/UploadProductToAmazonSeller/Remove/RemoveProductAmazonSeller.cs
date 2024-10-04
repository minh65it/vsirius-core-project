using FikaAmazonAPI.ConstructFeed.Messages;
using FikaAmazonAPI.ConstructFeed;
using FikaAmazonAPI;
using static FikaAmazonAPI.Utils.Constants;
using FikaAmazonAPI.AmazonSpApiSDK.Models.Feeds;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.Remove
{
    public class RemoveProductAmazonSeller
    {
        AmazonConnection amazonConnection;
        public RemoveProductAmazonSeller(AmazonConnection amazonConnection)
        {
            this.amazonConnection = amazonConnection;
        }
        public void SubmitFeedDeleteAddProduct(List<string> skus)
        {
            ConstructFeedService createDocument = new ConstructFeedService(amazonConnection.GetCurrentSellerID, "1.02");

            var list = new List<ProductMessage>();
            foreach (var sku in skus)
            {
                list.Add(new ProductMessage()
                {
                    SKU = sku
                });
            }
            createDocument.AddProductMessage(list, OperationType.Delete);
            var xml = createDocument.GetXML();

            var feedID = amazonConnection.Feed.SubmitFeedContent(xml, FeedType.POST_PRODUCT_DATA);

            GetFeedDetails(feedID);
        }
        public void GetFeedDetails(string feedID)
        {
            string ResultFeedDocumentId = string.Empty;
            while (string.IsNullOrEmpty(ResultFeedDocumentId))
            {
                var feedOutput = amazonConnection.Feed.GetFeed(feedID);
                if (feedOutput.ProcessingStatus == Feed.ProcessingStatusEnum.DONE)
                {
                    var outPut = amazonConnection.Feed.GetFeedDocument(feedOutput.ResultFeedDocumentId);

                    var reportOutput = outPut.Url;

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
