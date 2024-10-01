using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrdersAPI.Dtos;

namespace OrdersAPI._Services.Services.Tests
{
    [TestClass()]
    public class VsiriusOrderServiceTests
    {
        VsiriusOrderService service = new VsiriusOrderService(); 
        [TestMethod()]
        public void ChargeCreditCardTest()
        {
            var invoice = new VsiriusOrderDto();
            invoice.CardNumber = "4524041833048690";
            invoice.CardCode = "348";
            invoice.ExpirationDate = "0727";
            service.ChargeCreditCard(invoice);
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void AuthorizeCreditCardTest()
        {
            service.AuthorizeCreditCard();
            Assert.IsTrue(true);
        }
    }
}