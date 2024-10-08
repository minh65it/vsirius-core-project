using AutoMapper;
using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos.AuthenticationEtsyDto;
using EtsyIntegrationDataAPI.Helpers.AutoMapper;
using EtsyIntegrationDataAPI.Helpers.OrderEtsy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services.Tests
{
    [TestClass()]
    public class OrderServiceTests
    {
        private Mock<IAuthenticationEtsyService> _mockAuthenticationEtsyService;
        private Mock<ILoggerCustom> _mockLogService;
        private OrderService _orderService;
        private MapperConfiguration _mapperConfiguration;
        private Mock<IOrderRepository> _orderRepository;
        private Mock<IMapper> _mapper;
        private DataContext _mockDataContext;
        private Mock<IInsertOrder> _insertOrder;
        private Mock<IQgoldApiClient> _qgoldApiClient;
        private Mock<IStullerApiClient> _stullerApiClient;

        [TestInitialize]
        public void Setup()
        {

            var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase("DataContext")
            .Options;

            _mockDataContext = new DataContext(options);
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.Development.json")
                   .Build();

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new EfToDtoMappingProfile());
                cfg.AddProfile(new DtoToEfMappingProfile());
            });
            _orderRepository = new Mock<IOrderRepository>();
            _mapper = new Mock<IMapper>();
            _mockLogService = new Mock<ILoggerCustom>();
            _mockAuthenticationEtsyService = new Mock<IAuthenticationEtsyService>();
            _insertOrder = new Mock<IInsertOrder>();
            _qgoldApiClient = new Mock<IQgoldApiClient>();
            _stullerApiClient = new Mock<IStullerApiClient>();
            _orderService = new OrderService(_mapperConfiguration, _orderRepository.Object, _mapper.Object,
                _mockLogService.Object, configuration,
                _mockAuthenticationEtsyService.Object,
                _mockDataContext, _insertOrder.Object, _qgoldApiClient.Object, _stullerApiClient.Object);
        }



        [TestMethod()]
        public async Task GetShopReceiptsTest()
        {
            // Arrange
            OAuthTokenDto oAuthTokenDto = new OAuthTokenDto();
            oAuthTokenDto.Access_token = "valid_refresh_token";
            _mockAuthenticationEtsyService.Setup(x => x.RefreshToken(null)).ReturnsAsync(oAuthTokenDto);

            // Act
            var result = await _orderService.GetShopReceipts(null, null);

            // Assert
            Assert.AreEqual(result.Count, 0);
        }
    }
}