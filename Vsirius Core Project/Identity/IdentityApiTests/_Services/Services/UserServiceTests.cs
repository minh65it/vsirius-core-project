using AutoMapper;
using IdentityApi._Repositoties;
using IdentityApi._Services.Interfaces;
using IdentityApi.Data;
using IdentityApi.Dtos.User;
using IdentityApi.Helpers.Utilities;
using IdentityApi.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using IEmailSender = IdentityApi.Helpers.Utilities.IEmailSender;

namespace IdentityApi._Services.Services.Tests
{
    [TestClass()]
    public class UserServiceTests
    {
        private UserService _yourClass;
        private Mock<UserManager<VsiriusUser>> _userManager;
        private SignInManager<VsiriusUser> _signInManager;
        private Mock<IMapper> _mapper;
        private Mock<IFunctionUtilities> _functionUtilities;
        private Mock<IEmailSender> _emailSender;
        private HttpRequest _request;
        private Mock<IRepositoryAccessor> _repositoryAccessor;
        private Mock<IConfiguration> _configuration;
        private Mock<ILogService> _logService;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private IdentityContext _context;
        private Mock<IUrlHelper> _urlHelper;
        private  Mock<IValidUser> _validUser;
        [TestInitialize]
        public void Initialize()
        {

            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            var _userClaimsPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<VsiriusUser>>();

            var options = new DbContextOptionsBuilder<IdentityContext>()
               .UseInMemoryDatabase(databaseName: "test_identity_db")
               .Options;
            _context = new IdentityContext(options);
            var userStore = new UserStore<VsiriusUser>(new IdentityContext(options));
            _userManager = new Mock<UserManager<VsiriusUser>>(userStore, null, null, null, null, null, null, null, null);

            //_signInManager = new Mock<SignInManager<VsiriusUser>>
            //    (_userManager.Object, _httpContextAccessor.Object, _userClaimsPrincipalFactoryMock.Object, null, null, null);

            _mapper = new Mock<IMapper>();
            _functionUtilities = new Mock<IFunctionUtilities>();
            _emailSender = new Mock<IEmailSender>();
            _repositoryAccessor = new Mock<IRepositoryAccessor>();
            _configuration = new Mock<IConfiguration>();
            _logService = new Mock<ILogService>();
            _urlHelper = new Mock<IUrlHelper>();
            _validUser = new Mock<IValidUser>();

            
            _yourClass = new UserService(
                _userManager.Object,
                _mapper.Object,
                _functionUtilities.Object,
                _signInManager,
                _emailSender.Object,
                _httpContextAccessor.Object,
                _repositoryAccessor.Object, 
                _logService.Object,
                _validUser.Object,
                _configuration.Object);
        }
        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod()]
        public async Task CreateUserTest()
        {
            //Arrange
            var httpRequest = new Mock<HttpRequest>();
            _httpContextAccessor.Setup(x => x.HttpContext.Request).Returns(httpRequest.Object);


            UserDto userDto = new UserDto()
            {
                Email = "UserTest@gmail.com",
                FullName = "User test",
                Password = "password",
                PhoneNumber = "+17897895555"
            };
            var urlHelper = _urlHelper.Object;
            _functionUtilities.Setup(x => x.IsValidEmail(userDto.Email)).Returns(true);
            _functionUtilities.Setup(x => x.IsValidPhone(userDto.PhoneNumber)).Returns(true);


            VsiriusUser userMap = new VsiriusUser()
            {
                UserName = userDto.Email,
                IsActive = true,
                TwoFactorEnabled = false,
                EmailConfirmed = false,
                Id = Guid.NewGuid().ToString(),
            };
            _mapper.Setup(x => x.Map<VsiriusUser>(userDto)).Returns(userMap);

            VsiriusUser userCheck = null;
            _userManager.Setup(x => x.FindByEmailAsync(userDto.Email)).ReturnsAsync(userCheck);

            var identityResult = IdentityResult.Success;
            _userManager.Setup(x => x.CreateAsync(userMap, userDto.Password)).ReturnsAsync(identityResult);

            _emailSender.Setup(x => 
                x.CheckEmailExists(userDto.Email, "Confirm your email", It.IsAny<string>())).ReturnsAsync(true);

            //Act
            var data = await _yourClass.CreateUser(userDto, urlHelper,"");
            //Assert
            Assert.IsFalse(data.Success);
            Assert.AreEqual(data.Message, "Password must contain uppercase letters, lowercase letters, and numbers");
        }
    }
}