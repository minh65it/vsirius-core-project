using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using PhillyLuxEcommerce.ViewModels;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.Services.Interfaces;

namespace PhillyLuxEcommerce.Controllers
{
    [RequestInfo("Page Payment-Order")]
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        public PaymentController(IOrderService orderService, IUserService userService)
        {
            _orderService = orderService;
            _userService = userService;
        }

        [Route("Payment")]
        public async Task<IActionResult> Index(OrderInformationVM? orderVM)
        {
            if (orderVM.UserId == null)
            {
                var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return RedirectToAction("Login", "Home");
                }

                var resCart = await _orderService.GetCartByEmail(email);
                var resUser = await _orderService.GetInfoUserByEmail(email);

                var checkEmailAndPhoneVerify = (OrderInformationVM)resUser.Data;
                if (checkEmailAndPhoneVerify.PhoneNumberConfimed == false || checkEmailAndPhoneVerify.EmailConfimed == false
                    )
                {
                    return RedirectToAction("OrderVerify", "Payment");
                }

                OrderVM orderViewModel = new ()
                {
                    OrderCart = (OrderCartVM)resCart.Data,
                    OrderInformation = (OrderInformationVM)resUser.Data,
                };

                if (orderViewModel.OrderCart != null)
                {
                    return View(orderViewModel);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                OrderVM orderViewModel = new () { OrderInformation = orderVM };

                if (TempData["MessageError"] != null)
                {
                    var message = TempData["MessageError"].ToString();
                    if (message.Contains("card number")) orderViewModel.MessageCardNumber = message;
                    if (message.Contains("expired")) orderViewModel.MessageExpiredDate = message;

                    if (!message.Contains("card number") && !message.Contains("expired")) orderViewModel.MessageGeneral = message;
                }
                var resCart = await _orderService.GetCartByEmail(orderVM.Email);
                orderViewModel.OrderCart = (OrderCartVM)resCart.Data;

                return View(orderViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderVM orderVM)
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            var quantityTotalItem = Request.Form["QuantityTotalItem"];
            var totalPrice = Request.Form["TotalPrice"];
            var cartId = Request.Form["CartId"];
            var userId = Request.Form["UserId"];

            OrderCartVM orderCart = new ()
            {
                CartId = Guid.Parse(cartId),
                QuantityTotalItem = int.Parse(quantityTotalItem),
                TotalPrice = decimal.Parse(totalPrice)
            };

            orderVM.OrderInformation.UserId = Guid.Parse(userId);
            if (orderVM.OrderInformation.Country == "Canada")
            {
                orderVM.OrderInformation.City = Common.lstStateCanada.FirstOrDefault(x => x.Key.Equals(orderVM.OrderInformation.State))?.Title;
            }
            else
            {
                orderVM.OrderInformation.City = Common.lstStateUSA.FirstOrDefault(x => x.Key.Equals(orderVM.OrderInformation.State))?.Title;
            }
            var res = await _orderService.AddOrder(orderVM.OrderInformation, orderCart);

            if (res.Success)
            {
                return RedirectToAction(nameof(OrderSuccess), new { orderId = res.Data, resSuccess = res.Success });
            }
            else
            {
                if (!string.IsNullOrEmpty(res.Message))
                {
                    TempData["MessageError"] = res.Message;
                    return RedirectToAction("Index", "Payment", orderVM.OrderInformation);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult OrderSuccess(string orderId, bool resSuccess)
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            if (!string.IsNullOrEmpty(orderId) || resSuccess == false)
            {
                OrderSuccessVM orderSuccess = new ()
                {
                    OrderId = orderId,
                    ResSuccess = resSuccess
                };
                return View(orderSuccess);
            }
            return RedirectToAction(nameof(Index));
        }

        [Route("OrderDetail")]
        public async Task<IActionResult> OrderDetail(string orderId)
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            var resOrder = await _orderService.GetDetailOrder(orderId);

            if (resOrder.Success == true)
            {
                return View(resOrder.Data);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [Route("OrderHistory")]
        public async Task<IActionResult> OrderHistory(int? pageNumber)
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            var resOrderHistory = await _orderService.GetListOrderHistory(email, pageNumber);

            if (resOrderHistory.Success == true)
            {
                return View(resOrderHistory.Data);
            }
            else
            {
                return View(resOrderHistory.Data);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(OrderDetailVM orderDetailVM)
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            orderDetailVM.OrderInformation.TotalQuantityItem = int.Parse(Request.Form["QuantityTotalItem"]);
            orderDetailVM.OrderInformation.TotalPrice = decimal.Parse(Request.Form["TotalPrice"]);
            orderDetailVM.OrderInformation.CartId = Guid.Parse(Request.Form["CartId"]);
            orderDetailVM.OrderInformation.UserId = Guid.Parse(Request.Form["UserId"]);
            orderDetailVM.OrderInformation.PaymentMethod = int.Parse(Request.Form["PaymentMethod"]);
            orderDetailVM.OrderInformation.StatusId = int.Parse(Request.Form["StatusId"]);
            orderDetailVM.OrderInformation.OrderId = Request.Form["OrderId"].ToString();
            orderDetailVM.OrderInformation.CreateOrderDate = DateTime.Parse(Request.Form["CreateOrderDate"]);
            //orderDetailVM.OrderInformation.Tax = decimal.Parse(Request.Form["Tax"]);
            //orderDetailVM.OrderInformation.Shipping = bool.Parse();

            var res = await _orderService.CancelOrder(orderDetailVM.OrderInformation);
            return RedirectToAction(nameof(OrderDetail), new { orderId = res.Data });
        }

        public async Task<IActionResult> OrderVerify()
        {
            var email = HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Home");
            }

            var checkUser = await _userService.GetUserByEmail();
            if (checkUser.EmailConfirmed == true && checkUser.TwoFactorEnabled == true)
            {
                return RedirectToAction(nameof(Index));
            }

            if (checkUser.PhoneNumber != null)
            {
                checkUser.PhoneNumber = checkUser.PhoneNumber.Replace("+1", "(+1) ");
            }

            UserVM userVM = new()
            {
                Email = email,
                PhoneNumber = checkUser.PhoneNumber,
                EmailConfirmed = checkUser.EmailConfirmed,
                TwoFactorEnabled = checkUser.TwoFactorEnabled,
            };

            return View(userVM);
        }
    }
}
