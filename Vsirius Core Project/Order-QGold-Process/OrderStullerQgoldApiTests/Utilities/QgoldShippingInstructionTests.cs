using OrderStullerQgoldApi.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderStullerQgoldApi.Data;
using OrderStullerQgoldDomain.Qgold;

namespace OrderStullerQgoldApi.Utilities.Tests
{
    [TestClass()]
    public class QgoldShippingInstructionTests
    {
        private Mock<ISendMailService> _iEmailHandler;
        private Mock<IConfiguration> _configuration;
        private Mock<IQgoldShippingInstruction> _iQgoldShippingInstruction;
        private QgoldShippingInstruction _yourClass;
        private SendMailService _mailClass;

        [TestInitialize]
        public void Initialize()
        {
            _iEmailHandler = new Mock<ISendMailService>();
            _configuration = new Mock<IConfiguration>();
            _iQgoldShippingInstruction = new Mock<IQgoldShippingInstruction>();
            _configuration.Setup(c => c.GetSection("AWS:AccessKey").Value).Returns("AccessKey");
            _configuration.Setup(c => c.GetSection("AWS:SecretKey").Value).Returns("SecretKey");
            _configuration.Setup(c => c.GetSection("Email:CC").Value).Returns("CC");
            _mailClass = new SendMailService(_configuration.Object);
            _yourClass = new QgoldShippingInstruction(_iEmailHandler.Object, _configuration.Object);
        }

        [TestMethod()]
        public async Task RandomOrderTest()
        {
            // Arrange
            List<AmazonOrderInfo> amazonOrderInfos = new List<AmazonOrderInfo>()
            {
                new AmazonOrderInfo
                {
                    OrderId = "22350084-0",
                    Item = "899N",
                    ShipToName = "Andreanna Prude1",
                    ShipToAddress1 = "1708 Susanne Circle",
                    ShipToAddress2 = null,
                    ShipToCity = "Davisburg",
                    ShipToState_Province = "MI",
                    ShipToZip = "48350",
                    ShipToCountryCode = "US",
                    TotalPrice = 35.00,
                },
                new AmazonOrderInfo
                {
                    OrderId = "22351086-0",
                    Item = "K6353",
                    ShipToName = "Primalee Misa",
                    ShipToAddress1 = "500 King Dr",
                    ShipToAddress2 = null,
                    ShipToCity = "Davisburg",
                    ShipToState_Province = "MI",
                    ShipToZip = "48350",
                    ShipToCountryCode = "US",
                    TotalPrice = 48.48,
                },
                new AmazonOrderInfo
                {
                    OrderId = "22351999-0",
                    Item = "K6353",
                    ShipToName = "Provin gurung",
                    ShipToAddress1 = "956 BEATRICE PKWY",
                    ShipToAddress2 = null,
                    ShipToCity = "Davisburg",
                    ShipToState_Province = "MI",
                    ShipToZip = "48350",
                    ShipToCountryCode = "US",
                    TotalPrice = 230.12,
                },

            };

            var f = _configuration.Setup(c => c.GetSection("Email:From").Value).Returns("from@example.com");
            var t = _configuration.Setup(c => c.GetSection("Email:To").Value).Returns("to@example.com");
            Result checkSendMail = new Result() { IsSuccess = true };
            _iEmailHandler.Setup(c => c.SendToSingleEmail(f.ToString(), t.ToString(), "", "").Result).Returns(checkSendMail);
            _iEmailHandler.Setup(c => c.SendToMultipleEmail(f.ToString(), new List<string>() { "" }, "", "").Result).Returns(checkSendMail);

            // Act
            var test = await _yourClass.RandomOrder(amazonOrderInfos);
            // Assert
            Assert.AreEqual(test, true);
        }
    }
}