using Microsoft.AspNetCore.Mvc;

namespace PhillyLuxEcommerce.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
