using FikaAmazonAPI.ConstructFeed;
using static FikaAmazonAPI.Utils.Constants;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon
{
    public static class ConstructFeedProduct
    {
        public static AmazonEnvelope FeedProduct(string? merchantIdentifier = "A1XQU6VM5PM9U0")
        {
            AmazonEnvelope envelope = new AmazonEnvelope();
            FeedHeader header = new FeedHeader()
            {
                DocumentVersion = "1.02",
                MerchantIdentifier = merchantIdentifier
            };
            List<BaseMessage> Message = new List<BaseMessage>();

            envelope.Header = header;
            envelope.MessageType = FeedMessageType.Product;
            envelope.PurgeAndReplace = false;
            envelope.Message = Message;
            return envelope;
        }
    }
}
