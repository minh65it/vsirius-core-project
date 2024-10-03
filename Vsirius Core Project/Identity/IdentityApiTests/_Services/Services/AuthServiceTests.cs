//using AutoMapper;
//using IdentityApi._Repositoties;
//using IdentityApi._Services.Interfaces;
//using IdentityApi.Dtos.Auth;
//using IdentityApi.Helpers.Utilities;
//using IdentityApi.Models;
//using Microsoft.AspNetCore.Http;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using static IdentityApi.Dtos.Auth.AuthDto;

//namespace IdentityApi._Services.Services.Tests
//{
//    [TestClass()]
//    public class AuthServiceTests
//    {
//        private AuthService _yourClass;
//        private Mock<IRepositoryAccessor> _repo;
//        private Mock<IFunctionUtilities> _functionUtilities;
//        private Mock<IMapper> _mapper;
//        private Mock<ILogService> _logService;
//        private Mock<IHttpContextAccessor> _httpContextAccessor;
//        [TestInitialize]
//        public void Initialize()
//        {
//            _repo = new Mock<IRepositoryAccessor>();
//            _functionUtilities = new Mock<IFunctionUtilities>();
//            _mapper = new Mock<IMapper>();
//            _logService = new Mock<ILogService>();
//            _httpContextAccessor = new Mock<IHttpContextAccessor>();
//            _yourClass = new AuthService(_repo.Object, _functionUtilities.Object, _mapper.Object,
//                _logService.Object, _httpContextAccessor.Object);
//        }

//        [TestMethod()]
//        public async Task LoginTest()
//        {
//            //Arrange
//            AuthForLoginParam p = new AuthForLoginParam()
//            {
//                Email = "admin@vsirius.com",
//                Password = "password"
//            };
//            var employ = new Employee
//            {
//                Id = Guid.NewGuid(),
//                Email = "admin@vsirius.com",
//                UserName = "admin",
//                PasswordHash = "asdfasdfasdf",
//            };
//            var employDto = new EmployeeDto
//            {
//                Id = Guid.NewGuid(),
//                Email = "admin@vsirius.com",
//                UserName = "admin",
//                PasswordHash = "asdfasdfasdf",
//                Roles = new List<string>() { "Admin" },
//            };
//            _functionUtilities.Setup(foo => foo.IsValidEmail(p.Email)).Returns(true);
//            _functionUtilities.Setup(foo => foo.HashPasswordUser(p.Password)).Returns("asdfasdfasdf");
//            _repo.Setup(x => x.employee.FirstOrDefaultAsync(
//                x => x.Email == p.Email && x.PasswordHash == "asdfasdfasdf"
//                ).Result).Returns(employ);
//            _mapper.Setup(x => x.Map<EmployeeDto>(employ)).Returns(employDto);
//            //Act
//            var test = await  _yourClass.Login(p);
//            //Assert
//            Assert.IsNotNull(test);
//            Assert.AreEqual(test.Roles[0], "Admin");
//        }
//    }
//}