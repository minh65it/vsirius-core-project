using AutoMapper;
using Moq;
using Newtonsoft.Json;
using PhillyLuxEcommerce.Dtos;
using PhillyLuxEcommerce.Helpers.Utilities;
using PhillyLuxEcommerce.Services.Interfaces;
using PhillyLuxEcommerce.ViewModels;
using System.Text;

namespace PhillyLuxEcommerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(IMapper mapper, IHttpClientFactory httpClientFactory, ILogService logService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> GetCartByEmail(string email)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("OrderCart");

                HttpResponseMessage response = await httpClient.GetAsync("ListCartByUser?email=" + email);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    var result = jsonObject.ToObject<VsiriusListCartDto>();

                    OrderCartVM oderCart = _mapper.Map<OrderCartVM>(result);

                    List<OrderProductVM> listProduct = new List<OrderProductVM>();
                    foreach (var item in result.ListCartDetailsDtos)
                    {
                        OrderProductVM oderCartDetail = _mapper.Map<OrderProductVM>(item);
                        listProduct.Add(oderCartDetail);
                    }

                    oderCart.ListProductByOrderId = listProduct;
                    return new OperationResult { Success = true, Data = oderCart };
                }
                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetInfoUserByEmail(string email)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("OrderUser");
                HttpResponseMessage response = await httpClient.GetAsync("GetUserDetail?mail=" + email);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    var result = jsonObject.ToObject<UserDto>();

                    OrderInformationVM orderInfo = _mapper.Map<OrderInformationVM>(result);

                    if (!string.IsNullOrEmpty(orderInfo.Phone))
                    {
                        if (orderInfo.Phone.Contains("+1"))
                        {
                            orderInfo.Phone = orderInfo.Phone.Replace("+1", "");
                        }
                    }

                    return new OperationResult { Success = true, Data = orderInfo };
                }
                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> AddOrder(OrderInformationVM orderInfo, OrderCartVM orderCart)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Order");

                var resultInformation = _mapper.Map<VsiriusOrderDto>(orderInfo);
                resultInformation.CartId = orderCart.CartId;
                resultInformation.TotalPrice = orderCart.TotalPrice;
                resultInformation.TotalQuantityItem = orderCart.QuantityTotalItem;
                resultInformation.StatusId = (int?)StatusOrder.Pending;
                resultInformation.CreateOrderDate = DateTime.Now;
                resultInformation.Shipping = false;
                resultInformation.ExpirationDate = orderInfo.ExpirationDateMonth + orderInfo.ExpirationDateYear;
                resultInformation.Phone = "+1" + orderInfo.Phone;
                if (resultInformation.PaymentMethod == null) resultInformation.PaymentMethod = 0;

                var json = JsonConvert.SerializeObject(resultInformation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("Create", content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);

                    if (jsonObject.success == false)
                    {
                        return new OperationResult { Success = false, Message = jsonObject.message };
                    }
                    else
                    {
                        var result = jsonObject.data.ToObject<VsiriusOrderDto>();
                        return new OperationResult { Success = true, Data = result.OrderId };
                    }
                }
                string responseBody1 = await response.Content.ReadAsStringAsync();
                return new OperationResult { Success = false };
            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetDetailOrder(string orderId)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Order");

                HttpResponseMessage response = await httpClient.GetAsync("GetDetail?orderId=" + orderId);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    var result = jsonObject.ToObject<VsiriusOrderDto>();

                    OrderInformationVM orderInfo = _mapper.Map<OrderInformationVM>(result);
                    if (!string.IsNullOrEmpty(orderInfo.Phone))
                    {
                        if (orderInfo.Phone.Contains("+1"))
                        {
                            orderInfo.Phone = orderInfo.Phone.Replace("+1", "");
                        }
                    }

                    OrderDetailVM orderDetaiVM = new OrderDetailVM();
                    orderDetaiVM.OrderInformation = orderInfo;

                    List<OrderProductVM> products = new List<OrderProductVM>();
                    foreach (var item in result.ListProducts)
                    {
                        OrderProductVM orderProduct = _mapper.Map<OrderProductVM>(item);
                        products.Add(orderProduct);
                    }

                    var resOrderStatus = await GetStatusOrder();
                    if (resOrderStatus.Success) orderDetaiVM.ListOrderStatus = (List<OrderStatusVM>)resOrderStatus.Data;

                    orderDetaiVM.ListProductByOrderId = products;

                    return new OperationResult { Success = true, Data = orderDetaiVM };
                }
                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetListOrderHistory(string email, int? pageNumber)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Order");
                int pageNumberParam = (int)(pageNumber != null ? pageNumber : 1);

                HttpResponseMessage response = await httpClient.GetAsync("GetListOrderHistory?email=" + email +
                            "&PageNumber=" + pageNumberParam + "&PageSize=" + 10);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    OrderHistoryPagingVM orderHistoryPagingVM = new OrderHistoryPagingVM();

                    if (jsonObject != null)
                    {
                        var result = jsonObject.result.ToObject<List<OrderHistoryDto>>();

                        List<OrderHistoryVM> orderHistoryVM = new List<OrderHistoryVM>();

                        foreach (var item in result)
                        {
                            var orderHistory = new OrderHistoryVM();
                            var orderInfo = _mapper.Map<OrderInformationVM>(item.Orders);
                            orderHistory.Order = orderInfo;

                            var orderProduct = _mapper.Map<OrderProductVM>(item.Product);
                            orderHistory.Product = orderProduct;

                            orderHistoryVM.Add(orderHistory);
                        }

                        orderHistoryPagingVM.Pagination = _mapper.Map<PaginationVM>(jsonObject.pagination);
                        orderHistoryPagingVM.ListOrderHistory = orderHistoryVM;
                    }
                    else
                    {
                        orderHistoryPagingVM.ListOrderHistory = new List<OrderHistoryVM>();
                        orderHistoryPagingVM.Pagination = new PaginationVM();
                    }

                    return new OperationResult { Success = true, Data = orderHistoryPagingVM };
                }
                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetStatusOrder()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("OrderStatus");

                HttpResponseMessage response = await httpClient.GetAsync("GetListStatus");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic jsonObject = JsonConvert.DeserializeObject(responseBody);
                    var result = jsonObject.ToObject<List<VsiriusOrderStatusDto>>();

                    var listOrderStatus = _mapper.Map<List<OrderStatusVM>>(result);

                    return new OperationResult { Success = true, Data = listOrderStatus };
                }
                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> CancelOrder(OrderInformationVM orderInfo)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Order");

                var resultInformation = _mapper.Map<VsiriusOrderDto>(orderInfo);
                resultInformation.StatusId = 5;

                var json = JsonConvert.SerializeObject(resultInformation);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync("Update", content);

                if (response.IsSuccessStatusCode)
                {
                    return new OperationResult { Success = true, Data = resultInformation.OrderId };
                }
                return new OperationResult { Success = false };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> GetProductByOrderIdMoq()
        {
            var mockDataService = new Mock<IOrderService>();
            var mockData = new OrderCartVM
            {
                CartId = Guid.Parse("0d02cd63-f708-49d0-a61b-94459dbd5854"),
                QuantityTotalItem = 3,
                TotalPrice = 100,
                ListProductByOrderId = new List<OrderProductVM>
                {
                    new OrderProductVM { SKU="S-688878:LG600:P", ProductName = "14K Yellow 1/8 CTW Natural Diamond Cluster Beaded Ring", Price = "123.90",  PriceSale = "12.90", Size = "10", Color = "Yellow", main_image_url = "https://meteor.stullercloud.com/das/35321848?$xlarge$", QuantityItem = 2  },
                    new OrderProductVM { SKU="S-688878:LG600:P",ProductName = "18K Rose 6 mm Cross Band Size 20", Price = "234.90",  PriceSale = "12.90",  Size = "10", Color = "Brown", main_image_url = "https://meteor.stullercloud.com/das/97763414?obj=metals&obj.recipe=rose&$xlarge$", QuantityItem = 1 },
                    new OrderProductVM { SKU="S-688878:LG600:P",ProductName = "14K White 1/2 CTW Lab-Grown Diamond Ring", Price = "345.90",  PriceSale = "13.90", Size = "10", Color = "Pink", main_image_url = "https://meteor.stullercloud.com/das/100518233?$xlarge$", QuantityItem = 3 },
                }
            };

            var operation = new OperationResult { Success = true, Data = mockData };
            var email = "email@gmail.com";
            mockDataService.Setup(x => x.GetCartByEmail(email)).Returns(Task.FromResult(operation));

            var result = await mockDataService.Object.GetCartByEmail(email);
            return result;
        }

        public async Task<OperationResult> GetInfomationdMoq()
        {
            var mockDataService = new Mock<IOrderService>();
            var mockData = new OrderInformationVM
            {
                UserId = Guid.NewGuid(),
                FirstName = "SADSAD",
                Address1 = "ASDSA",
                Country = "sdsd",
                City = "ss",
                Shipping = false,
                Email = "hien@gmail.com",
            };

            var operation = new OperationResult { Success = true, Data = mockData };
            var email = "email@gmail.com";
            mockDataService.Setup(x => x.GetInfoUserByEmail(email)).Returns(Task.FromResult(operation));

            var result = await mockDataService.Object.GetInfoUserByEmail(email);
            return result;
        }
    }
}
