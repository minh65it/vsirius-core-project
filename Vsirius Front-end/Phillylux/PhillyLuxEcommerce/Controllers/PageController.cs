using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Controllers
{
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public PageController(IPageService pageService, IUserService userService, IMapper mapper)
        {
            _pageService = pageService;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Contact()
        {
            var user = await _userService.GetUserByEmail();
            if (user == null)
            {
                ContactVM contactVM = new ContactVM();
                return View(contactVM);
            }
            else
            {
                ContactVM contactVM = _mapper.Map<ContactVM>(user);
                return View(contactVM);
            }
        }

        public async Task<IActionResult> AddContact(ContactVM contactVM)
        {
            var res = await _pageService.AddContact(contactVM);
            if (res.Success)
            {
                return RedirectToAction("ContactSuccess");
            }
            else
            {
                return View("Contact");
            }
        }
        [Route("page/{id}/{slug}")]
        public async Task<IActionResult> Page(string slug, int id)
        {
            var result = await _pageService.Page(id);
            PageVM? pageVM = null;
            if (result.Success)
            {
                pageVM = _mapper.Map<PageVM>(result.Data as PageInfoDto);
            }
            return View(pageVM);
        }

        public IActionResult ContactSuccess()
        {
            return View();
        }
    }
}
