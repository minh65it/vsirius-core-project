using AutoMapper;
using IdentityApi._Repositoties;
using IdentityApi._Services.Interfaces;
using IdentityApi.Dtos.Auth;
using IdentityApi.Helpers.Utilities;
using static IdentityApi.Dtos.Auth.AuthDto;

namespace IdentityApi._Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepositoryAccessor _repo;
        private readonly IFunctionUtilities _functionUtilities;
        private readonly IMapper _mapper;
        private ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuthService(IRepositoryAccessor repo,
            IFunctionUtilities functionUtilities, IMapper mapper, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _functionUtilities = functionUtilities;
            _mapper = mapper;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EmployeeDto> Login(AuthForLoginParam paramAuth)
        {
            try
            {
                var checkMail = _functionUtilities.IsValidEmail(paramAuth.Email);
                if (!checkMail)
                {
                    return null;
                }
                var passwordHash = _functionUtilities.HashPasswordUser(paramAuth.Password);
                var result = await _repo.employee.FirstOrDefaultAsync(x => x.Email == paramAuth.Email && x.PasswordHash == passwordHash);
                if (result != null)
                {
                    var dataMap = _mapper.Map<EmployeeDto>(result);
                    dataMap.PasswordHash = null;
                    return dataMap;
                }
                return null;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new EmployeeDto();
            }
        }

        public Task<OperationResult> LogOut()
        {
            throw new NotImplementedException();
        }
    }
}
