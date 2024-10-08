using EtsyIntegrationDataAPI._Repositories.Interfaces;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services.Tests
{
    [TestClass()]
    public class PropertiesProductServiceTests
    {
        private Mock<IPropertiesByTaxonomyRepository> _mockIPropertiesByTaxonomyRepository;
        private Mock<IPropertiesProductRepository> _mockIPropertiesProductRepository;
        private Mock<IAuthenticationEtsyService> _mockIAuthenticationEtsyService;
        private Mock<IProductEtsyRepository> _mockIProductEtsyRepository;
        private Mock<IListingService> _mockIListingService;
        private Mock<ILoggerCustom> _mockLogService;
        private Mock<IConfiguration> _mockIConfiguration;
        private DataContext _mockDataContext;
        private PropertiesProductService _propertiesProductService;

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

            _mockIPropertiesByTaxonomyRepository = new Mock<IPropertiesByTaxonomyRepository>();
            _mockIPropertiesProductRepository = new Mock<IPropertiesProductRepository>();
            _mockIAuthenticationEtsyService = new Mock<IAuthenticationEtsyService>();
            _mockIProductEtsyRepository = new Mock<IProductEtsyRepository>();
            _mockIListingService = new Mock<IListingService>();
            _mockLogService = new Mock<ILoggerCustom>();
            _propertiesProductService = new PropertiesProductService(_mockIPropertiesByTaxonomyRepository.Object,
                _mockLogService.Object, configuration, _mockIPropertiesProductRepository.Object, _mockIAuthenticationEtsyService.Object,
                _mockIListingService.Object, _mockIProductEtsyRepository.Object);
        }



        [TestMethod()]
        public async Task SeedDataPropertiesByTaxonomyTest()
        {
            var data = await _propertiesProductService.SeedDataPropertiesByTaxonomy(1231);
            Assert.IsTrue(data.Success);
        }
    }
}