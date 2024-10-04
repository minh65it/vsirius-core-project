using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersAPI.Helpers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrdersAPI.Helpers.Utilities.Tests
{
    [TestClass()]
    public class FunctionUtilitiesTests
    {
        private IConfiguration _configuration;
        private FunctionUtilities fun;

        public FunctionUtilitiesTests()
        {
            // Initialize configuration and FunctionUtilities here
            var inMemorySettings = new Dictionary<string, string> {
                {"AmazonSMS:AccessKey", "REMOVED_AWS_KEY"},
                {"AmazonSMS:SecretKey", "GOwZP9ufneE0yF3f72GK3Gg7no8igfxt6A2oJoed"}
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            fun = new FunctionUtilities(_configuration);
        }

        [TestMethod()]
        public async Task SendSMSWithAWSTest()
        {
            Assert.AreEqual(true, await fun.SendSMSWithAWS("+12155002667", "testsms"));
        }
    }
}
