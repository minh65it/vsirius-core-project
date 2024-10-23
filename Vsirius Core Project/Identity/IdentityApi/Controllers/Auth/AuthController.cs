using AutoMapper;
using IdentityApi._Repositoties;
using IdentityApi._Services.Interfaces;
using IdentityApi.Dtos.Auth;
using IdentityApi.Helpers.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static IdentityApi.Dtos.Auth.AuthDto;

namespace IdentityApi.Controllers.Auth
{
    [Route("api/identity/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IRepositoryAccessor _repo;
        private readonly IConfiguration _configuration;
        private readonly IFunctionUtilities _functionUtilities;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IRepositoryAccessor repo, 
            IConfiguration configuration, IFunctionUtilities functionUtilities, IMapper mapper)
        {
            _authService = authService;
            _repo = repo;
            _configuration = configuration;
            _functionUtilities = functionUtilities;
            _mapper = mapper;
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            var result = await _authService.LogOut();
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthForLoginParam paramAuth)
        {
            var isLogin = await _authService.Login(paramAuth);
            if (isLogin == null)
            {
                return Unauthorized();
            }
            else
            {
                var roles = await _repo.userRolesEmployee.FindAll(x => x.Email == isLogin.Email).Join(
                    _repo.rolesEmployee.FindAll(),
                    x => x.RoleId, y => y.Id,
                    (x, y) => new {
                        y.Name,
                    }).Select(x => x.Name).ToListAsync();

                List<RoleClaimEmployeeDto> rolesClaim = new List<RoleClaimEmployeeDto>();
                if (roles.Any())
                {
                    rolesClaim = roles.Join(_repo.roleClaimsEmployee.FindAll().ToList(),
                        x => x, y => y.RoleId, (x, y) => new
                        {
                            y.RoleId,
                            y.ClaimType,
                        })
                        .GroupBy(x => x.RoleId).Select(x => new RoleClaimEmployeeDto
                        {
                            RoleId = x.Key,
                            ClaimType = x.Select(x => x.ClaimType).ToList(),
                        }).ToList();
                }

                var token = _functionUtilities.GenerateToken(isLogin.UserName, isLogin.Email, roles, 1);

                //export info when login
                var result = new
                {
                    isLogin = true,
                    user = isLogin,
                    roles = roles,
                    token = token,
                    rolesClaim = rolesClaim,
                };
                return Ok(result);
            }

        }
    }
}
