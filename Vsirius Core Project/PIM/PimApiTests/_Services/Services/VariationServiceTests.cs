using Microsoft.EntityFrameworkCore;
using PimApi.Context;
using VSiriusSystemLog.WriteLog;

namespace PimApi._Services.Services.Tests
{
    [TestClass()]
    public class VariationServiceTests
    {
        private DataContext _context;
        private VariationService _service;
        private readonly ILoggerCustom _logService;

        [TestInitialize()]
        public void SetUp()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=192.168.1.92,1433;Database=PIM;User Id=VsiriusServerCompany;Password=vsirius@123;TrustServerCertificate=True;");

            _context = new DataContext(optionsBuilder.Options);
            _service = new VariationService(_context, _logService);
        }

        [TestMethod()]
        public async Task UploadVariationFromPIMTest()
        {
            await _service.UploadVariationFromPIM();
        }
    }
}