using ProductApi._Services.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProductApi._Services.Services.Tests
{
    [TestClass()]
    public class ElasticSearchProductServiceTests
    {
        private ElasticSearchProductService _service;
        private IConfiguration _configuration;
        [TestInitialize]
        public void Setup()
        {
            _configuration =
                new ConfigurationBuilder().AddInMemoryCollection(new List<KeyValuePair<string, string>>() {
                new("ElasticConnectString" , "http://192.168.1.92:9200")
            }).Build();
            _service = new ElasticSearchProductService(_configuration);
        }

        [TestMethod()]
        public async Task BulkInsertProductToElasticTest()
        {
            await _service.BulkInsertProductToElastic(new List<Dtos.ElasticProductSearch>()
            {
                new Dtos.ElasticProductSearch()
                {
                    item_name = "z",
                    item_sku = "z",
                    product_description = "z"
                }
            });
        }

        [TestMethod()]
        public async Task DeleteAllProductInElasticTest()
        {
            await _service.DeleteAllProductInElastic();
        }
    }
}