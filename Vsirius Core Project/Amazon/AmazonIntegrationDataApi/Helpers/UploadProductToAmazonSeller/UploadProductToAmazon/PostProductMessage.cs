using FikaAmazonAPI.ConstructFeed;
using FikaAmazonAPI.ConstructFeed.Messages;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon
{
    public class PostBaseMessage : BaseMessage
    {
        public PostProductMessage Product { get; set; }
    }
    public partial class PostProductMessage
    {
        public string SKU { get; set; }

        public StandardProductID StandardProductID { get; set; }
        public DescriptionDataJewelry DescriptionData { get; set; }

        public Condition Condition { get; set; }
    }
}
