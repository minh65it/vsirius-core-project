using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using PhillyLuxEcommerce.Helpers.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos.Params;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;

namespace PhillyLuxEcommerce.Controllers
{
    [RequestInfo("Page Home")]
    public class HomeController : Controller
    {
        private readonly string _MessageErrorRegister = $"<div id='message' class=\"invalid-feedback\" style=\"display:block\">Register failed</div>";
        private readonly string _MessageErrorLogin = $"<div id='message' class=\"invalid-feedback\" style=\"display:block\">Login failed</div>";
        private readonly ILogger<HomeController> _logger;
        private readonly IHomeService _homeService;
        private readonly string _baseUrlImage;
        private const string AccessKeyFB = "AccessKeyFB";

        public HomeController(ILogger<HomeController> logger, IHomeService homeService, IConfiguration configuration)
        {
            _logger = logger;
            _homeService = homeService;
            _baseUrlImage = configuration["BaseURLImage"]!;
        }
        [HttpGet]

        public async Task<IActionResult> Index()
        {
            if (Common.categoryMenus.Count == 0)
            {
                var resFilter = await _homeService.GetFilterByProductType();
                Common.categoryMenus = (List<HomeVMFilter>)resFilter.Data;
            }

            var lstPopular = (List<HomeVMProduct>)(await _homeService.GetAllHomeProduct("Popular", 20)).Data;
            if (lstPopular != null)
            {
                if (lstPopular.Count % 2 != 0)
                {
                    lstPopular.RemoveAt(lstPopular.Count - 1);
                }
            }

            var lstLatest = (List<HomeVMProduct>)(await _homeService.GetAllHomeProduct("Latest", 20)).Data;
            HomeVM homeViewModel = new ()
            {
                BaseURLImage = _baseUrlImage,
                VsiriusFilterDtos = Common.categoryMenus,
                VsiriusProductLatest = lstLatest,
                VsiriusProductPopular = lstPopular
            };

            return View(homeViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await SignoutAll();

            return RedirectToAction("Index", "Home");
        }

        private async Task SignoutAll()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            }
            catch (Exception)
            {

            }
            try
            {
                await HttpContext.SignOutAsync("Google");
            }
            catch (Exception)
            {

            }
            try
            {
                await HttpContext.SignOutAsync("Facebook");
            }
            catch (Exception)
            {

            }
            try
            {
                await HttpContext.SignOutAsync();
            }
            catch (Exception)
            {
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [SecretUserInfo("Login POST")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginParam login)
        {
            if (Common.IsValidEmail(login.Email))
            {
                OperationResult result = await _homeService.LoginResult(login);
                login.IsAuthen = true;
                if (result.Success == true)
                {
                    var claims =
                        new List<Claim>()
                        {
                        new(ClaimTypes.Authentication, result.Data.ToString()),
                        new(ClaimTypes.Email, login.Email)
                        };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        //IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity),
                        authProperties);
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Message = _MessageErrorLogin;
                    return View();
                }
            }
            else
            {
                ViewBag.Message = _MessageErrorLogin;
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register(RegisterParam? registerParam)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View(registerParam);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterParam registerParam)
        {
            OperationResult result;
            if (Common.IsValidEmail(registerParam.Email) || Common.IsValidPhoneNumber(registerParam.PhoneNumber))
            {
                registerParam.PhoneNumber = $"+1{registerParam.PhoneNumber.Replace(" ", "")}";
                //result from register
                result = await _homeService.RegisterResult(registerParam);
                if (result.Success == true)
                {
                    LoginParam loginParam = new()
                    {
                        Email = registerParam.Email,
                        Password = registerParam.Password,
                        IsAuthen = false
                    };
                    //result from login
                    result = await _homeService.LoginResult(loginParam);
                    if (result.Success == true)
                    {
                        var claims = new List<Claim>
                        {
                        new (ClaimTypes.Authentication, result.Data.ToString()),
                        new (ClaimTypes.Email, registerParam.Email)
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            //AllowRefresh = <bool>,
                            // Refreshing the authentication session should be allowed.

                            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                            // The time at which the authentication ticket expires. A 
                            // value set here overrides the ExpireTimeSpan option of 
                            // CookieAuthenticationOptions set with AddCookie.

                            //IsPersistent = true,
                            // Whether the authentication session is persisted across 
                            // multiple requests. When used with cookies, controls
                            // whether the cookie's lifetime is absolute (matching the
                            // lifetime of the authentication ticket) or session-based.

                            //IssuedUtc = <DateTimeOffset>,
                            // The time at which the authentication ticket was issued.

                            //RedirectUri = <string>
                            // The full path or absolute URI to be used as an http 
                            // redirect response value.
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity),
                            authProperties);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Message = _MessageErrorLogin;
                        //Login failed
                        return RedirectToAction("Login");
                    }
                }
                else
                {
                    ViewBag.MessageEmail = _MessageErrorRegister.Replace("Register failed", $"{result.Data}");
                    registerParam.PhoneNumber = registerParam.PhoneNumber.Replace("+1", "");
                    return View(registerParam);
                }
            }
            else
            {
                ViewBag.MessageEmail = _MessageErrorRegister;
                registerParam.PhoneNumber = registerParam.PhoneNumber.Replace("+1", "");
                return View(registerParam);
            }
        }

        [HttpGet]
        public IActionResult InputEmail()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> InputEmail(string email)
        {
            var accessToken = TempData[AccessKeyFB]?.ToString();
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return RedirectToAction("Login");
            }
            // Here, you can use the email address to look up the user in your database
            LoginParam login = new()
            {
                Email = email,
                IsAuthen = true,
                Password = accessToken,
                Provider = FacebookDefaults.AuthenticationScheme
            };
            if (TempData.ContainsKey(AccessKeyFB))
            {
                TempData.Remove(AccessKeyFB);
            }
            OperationResult result = await _homeService.LoginResult(login);
            // or create a new user account with the email retrieved from Google
            if (string.IsNullOrWhiteSpace($"{result.Data}"))
            {
                await SignoutAll();
                ViewBag.Message = _MessageErrorLogin;
                return RedirectToAction("Login");
            }
            var claims = new List<Claim>
            {
                new (ClaimTypes.Authentication, $"{result.Data}"),
                new (ClaimTypes.Email, email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                authProperties);

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? provider, string? returnUrl, string? remoteError)
        {

            if (remoteError != null)
            {
                await SignoutAll();
                // Handle errors during external authentication
                ViewBag.Message = _MessageErrorLogin;
                return RedirectToAction("Login");
            }

            var authResult = await HttpContext.AuthenticateAsync(provider);
            if (!authResult.Succeeded)
            {
                await SignoutAll();
                // Handle the case where external authentication failed
                ViewBag.Message = _MessageErrorLogin;
                return RedirectToAction("Login");
            }

            var email = authResult.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                await SignoutAll();
                // Handle the case where the email claim is not present
                ViewBag.Message = _MessageErrorLogin;
                return RedirectToAction("Login");
            }
            var accessToken = await HttpContext.GetTokenAsync(provider, "access_token");
            if (provider == "Facebook")
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://graph.facebook.com/v18.0/me?fields=id,name,email,picture&access_token={accessToken}");

                if (response.IsSuccessStatusCode)
                {
                    // Đọc và xử lý phản hồi từ Google API
                    var content = await response.Content.ReadAsStringAsync();
                    var fbUser = JsonConvert.DeserializeObject<FacebookUser>(content);
                    if (string.IsNullOrEmpty(fbUser.Email))
                    {
                        TempData[AccessKeyFB] = accessToken;
                        await SignoutAll();
                        return RedirectToAction("InputEmail");
                    }
                }
            }
            // Here, you can use the email address to look up the user in your database
            LoginParam login = new()
            {
                Email = email,
                IsAuthen = true,
                Password = accessToken,
                Provider = provider
            };

            OperationResult result = await _homeService.LoginResult(login);
            // or create a new user account with the email retrieved from Google
            if (string.IsNullOrWhiteSpace($"{result.Data}"))
            {
                await SignoutAll();
                ViewBag.Message = _MessageErrorLogin;
                return RedirectToAction("Login");
            }
            var claims = new List<Claim>
            {
                new (ClaimTypes.Authentication, result.Data.ToString()),
                new (ClaimTypes.Email, email)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across 
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                authProperties);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult ExternalLogin(string returnUrl, string provider = "google")
        {
            var auth = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback", new { provider, returnUrl })
            };

            switch (provider.ToLower())
            {
                case "google":
                    return new ChallengeResult(GoogleDefaults.AuthenticationScheme, auth);
                case "facebook":
                    return new ChallengeResult(FacebookDefaults.AuthenticationScheme, auth);
                default:
                    break;
            }
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, auth);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}