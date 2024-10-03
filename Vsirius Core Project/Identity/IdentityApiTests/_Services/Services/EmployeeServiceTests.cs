//using AutoMapper;
//using IdentityApi._Repositoties;
//using IdentityApi._Services.Interfaces;
//using IdentityApi.Data;
//using IdentityApi.Helpers.Utilities;
//using IdentityApi.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;

//namespace IdentityApi._Services.Services.Tests
//{
//    [TestClass()]
//    public class EmployeeServiceTests
//    {
//        private EmployeeService _yourClass;
//        private Mock<IRepositoryAccessor> _repo;
//        private Mock<IMapper> _mapper;
//        private Mock<IFunctionUtilities> _functionUtilities;
//        private Mock<ILogService> _logService;
//        private Mock<IHttpContextAccessor> _httpContextAccessor;
//        private IdentityContext _context;
//        [TestInitialize]
//        public void Initialize()
//        {
//            _repo = new Mock<IRepositoryAccessor>();
//            _functionUtilities = new Mock<IFunctionUtilities>();
//            _mapper = new Mock<IMapper>();
//            _logService = new Mock<ILogService>();
//            _httpContextAccessor = new Mock<IHttpContextAccessor>();
//            var options = new DbContextOptionsBuilder<IdentityContext>()
//               .UseInMemoryDatabase(databaseName: "test_identity_db")
//               .Options;
//            _context = new IdentityContext(options);
//            _yourClass = new EmployeeService(_repo.Object, _mapper.Object, _functionUtilities.Object,
//                _logService.Object, _httpContextAccessor.Object, _context);
//        }
//        [TestCleanup]
//        public void Cleanup()
//        {
//            _context.Database.EnsureDeleted();
//            _context.Dispose();
//        }
//        [TestMethod()]
//        public async Task DeleteEmployeeTest()
//        {
//            //Arrange
//            string i = Guid.NewGuid().ToString();
//            var employ = new Employee
//            {
//                Id = Guid.NewGuid(),
//                Email = "test2@test.com",
//                UserName = "admin",
//                PasswordHash = "PasswordHash",
//            };
//            _repo.Setup(x => x.employee.FindSingle(x => x.Id.ToString() == i).Result)
//                .Returns(new Employee()
//                {
//                    Email = "test2@test.com",
//                });

//            var userRoles = new List<UserRolesEmployee>
//                {
//                    new UserRolesEmployee { Email = "test1@test.com", Id = Guid.NewGuid().ToString() },
//                    new UserRolesEmployee { Email = "test2@test.com", Id = Guid.NewGuid().ToString() },
//                    new UserRolesEmployee { Email = "test3@test.com", Id = Guid.NewGuid().ToString() }
//                };

//            _context.UserRolesEmployee.AddRange(userRoles);
//            _context.SaveChanges();
//            //Act
//            var test = await _yourClass.DeleteEmployee(i);
//            //Assert
//            Assert.IsTrue(test.Success);
//            Assert.AreEqual(test.Message, "Successful delete");
//        }
//    }
//}