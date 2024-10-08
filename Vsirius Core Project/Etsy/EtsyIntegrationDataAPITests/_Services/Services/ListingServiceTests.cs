using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos.AuthenticationEtsyDto;
using Microsoft.Extensions.Configuration;
using Moq;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services.Tests
{
    [TestClass()]
    public class ListingServiceTests
    {
        private Mock<IConfiguration> _mockIConfiguration;
        private Mock<ILoggerCustom> _mockLogService;
        private Mock<IAuthenticationEtsyService> _mockAuthenticationEtsyService;
        private Mock<IProductEtsyService> _mockProductEtsyService;
        private Mock<IListingImageService> _mockListingImageService;
        private Mock<ITopOrderService> _mockTopOrderService;
        private DataContext _mockDataContext;
        private ListingService _listingService;
        private Mock<HttpMessageHandler> _handlerMock;
        private Mock<IProductEtsyRepository> _mockProductEtsyRepository;

        [TestInitialize]
        public void Setup()
        {

            //var options = new DbContextOptionsBuilder<DataContext>()
            //.UseInMemoryDatabase("DataContext")
            //.Options;

            //_mockDataContext = new DataContext(options);
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.Development.json")
                   .Build();

            _handlerMock = new Mock<HttpMessageHandler>();
            _mockIConfiguration = new Mock<IConfiguration>();
            _mockProductEtsyRepository = new Mock<IProductEtsyRepository>();
            _mockLogService = new Mock<ILoggerCustom>();
            _mockAuthenticationEtsyService = new Mock<IAuthenticationEtsyService>();
            _mockProductEtsyService = new Mock<IProductEtsyService>();
            _mockListingImageService = new Mock<IListingImageService>();
            _mockTopOrderService = new Mock<ITopOrderService>();
            _listingService = new ListingService(_mockLogService.Object, configuration,
                _mockAuthenticationEtsyService.Object,
                _mockProductEtsyService.Object, _mockListingImageService.Object, _mockTopOrderService.Object,
                _mockProductEtsyRepository.Object);
        }


        [TestMethod()]
        public async Task GetAllListingsByShopTest()
        {
            // Arrange
            OAuthTokenDto oAuthTokenDto = new OAuthTokenDto()
            {
                Refresh_token = "Refresh_token",
            };
            oAuthTokenDto.Access_token = "valid_refresh_token";
            _mockAuthenticationEtsyService.Setup(x => x.RefreshToken(null)).ReturnsAsync(oAuthTokenDto);

            int limit = 100;
            var data = await _listingService.GetPaginationListingsByShop(oAuthTokenDto.Refresh_token, limit, 0, "active");
            // Act

            var result = await _listingService.GetAllListingsByShop(oAuthTokenDto.Refresh_token);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 0);
            Assert.IsNull(data);
        }
    }
}