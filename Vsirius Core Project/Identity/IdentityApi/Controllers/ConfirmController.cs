using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VsiriusDataModel.DataModel;

namespace IdentityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfirmController : ControllerBase
    {
        private readonly UserManager<VsiriusUser> _userManager;
        private readonly SignInManager<VsiriusUser> _signInManager;

        public ConfirmController(UserManager<VsiriusUser> userManager, SignInManager<VsiriusUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpPost]
        public async Task<bool> Index([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return result.Succeeded;
        }
    }
}
