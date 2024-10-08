using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services.Tests
{
    [TestClass()]
    public class ListingInventoryServiceTests
    {
        private Mock<IConfiguration> _mockIConfiguration;
        private Mock<ILoggerCustom> _mockLogService;
        private Mock<IAuthenticationEtsyService> _mockAuthenticationEtsyService;
        private Mock<IListingService> _mockListingService;
        private Mock<IProductEtsyService> _mockProductEtsyService;
        private Mock<IListingImageService> _mockListingImageService;
        private Mock<IProductEtsyRepository> _productEtsyRepository;
        private ListingInventoryService _listingInventoryService;
        private Mock<HttpMessageHandler> _handlerMock;


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
            _mockLogService = new Mock<ILoggerCustom>();
            _mockListingService = new Mock<IListingService>();
            _mockAuthenticationEtsyService = new Mock<IAuthenticationEtsyService>();
            _mockProductEtsyService = new Mock<IProductEtsyService>();
            _mockListingImageService = new Mock<IListingImageService>();
            _productEtsyRepository = new Mock<IProductEtsyRepository>();
            _listingInventoryService = new ListingInventoryService(_mockLogService.Object, configuration,
                _mockAuthenticationEtsyService.Object, _mockProductEtsyService.Object, _mockListingService.Object,
                _productEtsyRepository.Object);
        }

        [TestMethod()]
        public async Task UpdateListingInventoryTest()
        {
            var result = await _listingInventoryService.UpdateAllListingInventory();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}