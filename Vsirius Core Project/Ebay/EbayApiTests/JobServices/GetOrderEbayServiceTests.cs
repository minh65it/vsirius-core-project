using Microsoft.VisualStudio.TestTools.UnitTesting;
using EbayApi.DataContext;
using System.Threading.Tasks;

namespace EbayApi.JobServices.Tests
{
    [TestClass()]
    public class GetOrderEbayServiceTests
    {
        private GetOrderEbayService _service;
        [TestInitialize]
        public void Setup()
        {
            var context = new EbayContext();
            _service = new GetOrderEbayService(context);
        }
        [TestMethod()]
        public async Task GetOrderFromEbayTest()
        {
            await _service.GetOrderFromEbay();
        }
    }
}