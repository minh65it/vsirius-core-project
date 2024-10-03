using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using System.Security.Claims;

namespace PhillyLuxEcommerce.Controllers
{
    //[Authorize]
    [RequestInfo("Page Account")]
    public class UserController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IWishlistService _wishlistService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private const string _PHONE = "phoneUser";
        private const string _UPDATED_ERR = "msgUpdate";
        private const string _MSG_VERIFY_PHONE = "msgPhone";
        public UserController(
            IMapper mapper,
            IUserService userService,
            IConfiguration configuration,
            IWishlistService wishlistService,
            INotificationService notificationService)
        {
            _configuration = configuration;
            _userService = userService;
            _wishlistService = wishlistService;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        [Route("Info")]
        public async Task<IActionResult> Index(UserVM userUpdate)
        {

            if (userUpdate!.Id != null)
            {
                ViewBag.ResultUpdateUser = TempData.Peek(_UPDATED_ERR) as string;
                if (!string.IsNullOrWhiteSpace(userUpdate.PhoneNumber))
                {
                    TempData[_PHONE] = userUpdate.PhoneNumber;
                }
                return View(userUpdate);
            }
            UserDto? user = await _userService.GetUserByEmail();

            if (user != null)
            {
                UserVM userVM = _mapper.Map<UserVM>(user);
                if (!string.IsNullOrEmpty(user.Avatar))
                {
                    if (!user.Avatar.Contains("http"))
                        userVM.Avatar = _configuration.GetSection("BaseURLImageUser").Value + user.Avatar;
                }
                TempData[_PHONE] = userVM.PhoneNumber;
                return View(userVM);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserVM user)
        {
            var avatar = user.Avatar;
            var (result, msg) = (true, "");
            if (user.UploadImg == null || user.UploadImg.Length == 0)
            {
                user.Avatar = null;
            }
            else
            {
                string[] acceptFiles = { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png" };
                if (acceptFiles.Contains(user.UploadImg.ContentType.ToLower()))
                {
                    using MemoryStream memoryStream = new();
                    user.UploadImg.CopyTo(memoryStream);
                    byte[] fileBytes = memoryStream.ToArray();
                    user.Avatar = "data:image/png;base64," + Convert.ToBase64String(fileBytes);
                }
                user.UploadImg = null;
            }
            if (user.State == "Canada")
            {
                user.City = Common.lstStateCanada.FirstOrDefault(x => x.Key == user.State)?.Title;
            }
            else
            {
                user.City = Common.lstStateUSA.FirstOrDefault(x => x.Key == user.State)?.Title;
            }
            var phone = user.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(phone))
            {
                user.PhoneNumber = $"+1{phone}";
            }

            (result, msg) = await _userService.UpdateUser(user);

            if (result != true)
            {
                TempData[_UPDATED_ERR] = $"<div class=\"invalid-feedback\" style=\"display:block\">{msg}</div>";
                user.PhoneNumber = phone;
                user.Avatar = avatar;
                user.UploadImg = null;
                return RedirectToAction(nameof(Index), user);
            }
            else
            {
                if (TempData.ContainsKey(_UPDATED_ERR))
                {
                    TempData.Remove(_UPDATED_ERR);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendEmailVerify(string email)
        {
            bool result = await _userService.VerifyEmail(email);
            return Json(result);
        }

        public async Task<IActionResult> OTP()
        {
            ViewBag.PhoneNumber = TempData.Peek(_PHONE) as string;
            ViewBag.ResultVerifyPhone = TempData.Peek(_MSG_VERIFY_PHONE) as string;

            bool result = await _userService.SendOTP();

            if (result == true)
            {
                if (TempData.ContainsKey(_UPDATED_ERR))
                {
                    TempData.Remove(_UPDATED_ERR);
                }

                return View();
            }
            else
            {
                TempData[_UPDATED_ERR] = $"<div class=\"invalid-feedback\" style=\"display:block\">Verify number failed. Please update your phone number</div>";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPhoneByOTP(string OTP)
        {
            TempData[_MSG_VERIFY_PHONE] = null;
            bool result = await _userService.VerifyPhone(OTP);
            if (result == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData[_MSG_VERIFY_PHONE] = $"<div class=\"invalid-feedback\" style=\"display:block\">Verify failed.</div>";
            }

            return RedirectToAction("OTP");
        }

        public IActionResult Order()
        {
            return View();
        }

        public async Task<IActionResult> Wishlist(int? pageNumber)
        {
            var res = await _wishlistService.GetWishlistByUserId(pageNumber);
            return View(res.Data);
        }

        public async Task<IActionResult> Notification(int pageNumber)
        {
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            string email = $"{HttpContext.User.FindFirst(ClaimTypes.Email)?.Value}";
            var res = await _notificationService.GetAllNotification(email, pageNumber);
            for (int i = 0; i < res.Result.Count; i++)
            {
                await _notificationService.UpdateNotificationByUser(email, res.Result[i].Id);
            }

            return View(res);
        }

        [HttpGet]
        public ActionResult CheckLogin()
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return Json("False");
            }

            return Json("True");
        }

    }
}
