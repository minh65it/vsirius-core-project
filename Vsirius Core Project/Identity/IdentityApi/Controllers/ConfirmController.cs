using IdentityApi.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers
{
    public class ConfirmController : Controller
    {
        private readonly UserManager<VsiriusUser> _userManager;
        private readonly SignInManager<VsiriusUser> _signInManager;

        public ConfirmController(UserManager<VsiriusUser> userManager, SignInManager<VsiriusUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Error));
            }
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
