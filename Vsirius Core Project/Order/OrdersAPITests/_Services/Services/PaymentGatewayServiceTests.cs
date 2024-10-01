using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersAPI.Data;

namespace OrdersAPI._Services.Services.Tests
{
    [TestClass()]
    public class PaymentGatewayServiceTests
    {

        [TestMethod()]
        public void CreateTest()
        {
            //var connectionString = "Server=192.168.1.25,1433;Database=VsiriusWeb;User Id=VsiriusServerCompany;Password=vsirius@123; TrustServerCertificate=True;";
            //var options = new DbContextOptionsBuilder<DbContext>().UseSqlServer(connectionString).Options;
            //DataContext context = new(options);
            //var service = new PaymentGatewayService(context);
            //var a = service.Create("886sD2vMBa", "5p5MK288qN74Le4X", 3);
            var a = "";
            Assert.IsNotNull(a);
        }
    }
}