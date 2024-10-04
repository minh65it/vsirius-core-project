using FikaAmazonAPI.ConstructFeed;
using static FikaAmazonAPI.Utils.Constants;
using System.Xml.Serialization;

namespace AmazonIntegrationDataApi.Helpers.UploadProductToAmazonSeller.UploadProductToAmazon
{
    public class AmazonEnvelope
    {

        [XmlElement(Order = 1)]
        public FeedHeader Header { get; set; }

        [XmlElement(Order = 2)]
        public FeedMessageType? MessageType { get; set; }

        [XmlElement(Order = 3)]
        public bool? PurgeAndReplace { get; set; }
        [XmlIgnore]
        public bool PurgeAndReplaceSpecified { get { return PurgeAndReplace.HasValue; } }

        [XmlElement(ElementName = "Message", Order = 4)]
        public List<BaseMessage> Message;

    }
}
