using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Controllers;
using AutoMapper;
using AuthorizeNet.Api.Contracts.V1;
using Korzh.EasyQuery.Linq;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;
using OrdersAPI.Helpers.Params;
using OrdersAPI.Helpers.Utilities;
using VsiriusDataModel.DataModel;

namespace OrdersAPI._Services.Services
{
    public class VsiriusOrderService : IVsiriusOrderService
    {
        private readonly IVsiriusOrderRepository _orderRepository;
        private readonly IVsiriusOrderDetailRepository _orderDetailRepository;
        private readonly IVsiriusCartRepository _cartRepository;
        private readonly IVsiriusCartDetailsRepository _cartDetailsRepository;
        private readonly IVsiriusProductRepository _productRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogService _logService;
        private IConfigurationRoot _configuration;

        private readonly string ApiLoginID;
        private readonly string ApiTransactionKey;

        public VsiriusOrderService()
        {
            _configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
            ApiLoginID = _configuration.GetSection("ApiLoginID").Value;
            ApiTransactionKey = _configuration.GetSection("ApiTransactionKey").Value;
        }

        public VsiriusOrderService(IVsiriusOrderRepository orderRepository
            , IMapper mapper, IVsiriusCartRepository cartRepository
            , IVsiriusCartDetailsRepository cartDetailsRepository
            , IVsiriusOrderDetailRepository orderDetailRepository
            , IVsiriusProductRepository productRepository
            , IOrderStatusRepository orderStatusRepository
            , IUserRepository userRepository
            , IHttpContextAccessor httpContextAccessor
            , ILogService logService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _mapper = mapper;
            _cartRepository = cartRepository;
            _cartDetailsRepository = cartDetailsRepository;
            _productRepository = productRepository;
            _orderStatusRepository = orderStatusRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
            _configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
            ApiLoginID = _configuration.GetSection("ApiLoginID").Value;
            ApiTransactionKey = _configuration.GetSection("ApiTransactionKey").Value;
        }

        public async Task<OperationResult> Create(VsiriusOrderDto dto)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, 8).Select(s => s[random.Next(s.Length)]).ToArray());

            dto.OrderId = randomString;
            var resultChargeCard = ChargeCreditCard(dto);
            if (!resultChargeCard.Success)
            {
                return resultChargeCard;
            }

            var data = _mapper.Map<VsiriusOrders>(dto);

            // Xử dụng RollBack để trả dữ liệu về trạng thái trước khi lưu vào Database trong trường hợp lỗi
            var transaction = await _orderRepository.BeginTransactionAsync();

            try
            {
                _orderRepository.Add(data);
                await _orderRepository.SaveChangesAsync();

                var cartStatus = await _cartRepository.FirstOrDefaultAsync(x => x.CartId == data.CartId);

                if (cartStatus != null)
                {
                    cartStatus.Status = true;
                    _cartRepository.Update(cartStatus);
                    await _cartRepository.SaveChangesAsync();
                }

                var listCartByOrder = _cartRepository.FindAll(x => x.CartId == data.CartId).Join(_cartDetailsRepository.FindAll(x => x.IsDelete == false),
                                  x => x.CartId,
                                  y => y.CartId,
                                  (x, y) => new { Cart = x, CartDetail = y })
                                  .Select(x => new
                                  {
                                      CartId = x.Cart.CartId,
                                      ProductId = x.CartDetail.ProductId,
                                      Quantity = x.CartDetail.Quantity,
                                      Price = x.CartDetail.Price
                                  }).ToList();

                foreach (var item in listCartByOrder)
                {
                    VsiriusOrderDetails orderDetails = new()
                    {
                        OrderId = data.OrderId,
                        ProductId = item.ProductId,
                        Price = item.Price,
                        QuantityItem = item.Quantity,
                    };
                    _orderDetailRepository.Add(orderDetails);
                }
                await _orderDetailRepository.SaveChangesAsync();

                // Commit nếu thêm dữ liệu thành công
                await transaction.CommitAsync();
                return new OperationResult { Success = true, Data = data };
            }
            catch (System.Exception ex)
            {
                // Trả lại trạng thái trước khi lưu
                await transaction.RollbackAsync();
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        #region method to process Payment
        public OperationResult ChargeCreditCard(VsiriusOrderDto dto)
        {
            try
            {
                if(_configuration.GetSection("AuthorizeNet").Value == "SANDBOX")
                    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
                else
                    ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.PRODUCTION;
                // define the merchant information (authentication / transaction id)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                {                   
                    name = ApiLoginID,
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = ApiTransactionKey,                  
                };

                var creditCard = new creditCardType
                {                  
                    cardNumber = dto.CardNumber,
                    expirationDate = dto.ExpirationDate,
                    cardCode = dto.CardCode
                };

                var billingAddress = new customerAddressType
                {
                   
                    firstName = dto.FirstName,
                    lastName = dto.LastName,
                    address = dto.Address1,
                    city = dto.City,
                    zip = dto.PostCode.ToString()
                };

                //standard api call to retrieve response
                var paymentType = new paymentType { Item = creditCard };

                // Add line Items
                //var lineItems = new lineItemType[2];
                //lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt", quantity = 2, unitPrice = new Decimal(15.00) };
                //lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

                var order = new orderType
                {
                    invoiceNumber = dto.OrderId,
                    purchaseOrderDateUTC = DateTime.UtcNow,
                };

                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card

                    amount = (decimal)dto.TotalPrice,
                    payment = paymentType,
                    billTo = billingAddress,
                    shipTo = billingAddress,
                    order = order,
                    //lineItems = lineItems
                };

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the controller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();

                // validate response
                if (response != null)
                {
                    if (response.messages.resultCode == messageTypeEnum.Ok)
                    {
                        if (response.transactionResponse.messages != null)
                        {
                            //Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                            //Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                            //Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                            //Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                            //Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                            return new OperationResult(true);
                        }
                        else
                        {
                            //Console.WriteLine("Failed Transaction.");
                            if (response.transactionResponse.errors != null)
                            {
                                //Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                                //Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                                return new OperationResult(false, response.transactionResponse.errors[0].errorText);
                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse != null && response.transactionResponse.errors != null)
                        {
                            //Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            //Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                            return new OperationResult(false, response.transactionResponse.errors[0].errorText);
                        }
                        else
                        {
                            //Console.WriteLine("Error Code: " + response.messages.message[0].code);
                            //Console.WriteLine("Error message: " + response.messages.message[0].text);
                            return new OperationResult(false, response.messages.message[0].text);

                        }
                    }
                }
                else
                {
                    return new OperationResult(false, "Wrong information");
                    //Console.WriteLine("Null Response.");
                }
            }
            catch
            {
                return new OperationResult(false, "The system is maintenance, please contact us for this inconvenient");
            }

            return new OperationResult(false);
        }

        public OperationResult AuthorizeCreditCard()
        {
            var amount = 2;

            var result = new OperationResult();
            try
            {
                Console.WriteLine("Authorize Credit Card Sample");

                ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

                // define the merchant information (authentication / transaction id)
                ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
                {
                    name = ApiLoginID,
                    ItemElementName = ItemChoiceType.transactionKey,
                    Item = ApiTransactionKey,
                };

                var creditCard = new creditCardType
                {
                    cardNumber = "4111111111111112",
                    expirationDate = "1028"
                };

                //standard api call to retrieve response
                var paymentType = new paymentType { Item = creditCard };

                var transactionRequest = new transactionRequestType
                {
                    transactionType = transactionTypeEnum.authOnlyTransaction.ToString(),    // authorize only
                    amount = amount,
                    payment = paymentType
                };

                var request = new createTransactionRequest { transactionRequest = transactionRequest };

                // instantiate the controller that will call the service
                var controller = new createTransactionController(request);
                controller.Execute();

                // get the response from the service (errors contained if any)
                var response = controller.GetApiResponse();

                // validate response
                if (response != null)
                {
                    if (response.messages.resultCode == messageTypeEnum.Ok)
                    {
                        if (response.transactionResponse.messages != null)
                        {
                            Console.WriteLine("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                            Console.WriteLine("Response Code: " + response.transactionResponse.responseCode);
                            Console.WriteLine("Message Code: " + response.transactionResponse.messages[0].code);
                            Console.WriteLine("Description: " + response.transactionResponse.messages[0].description);
                            Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
                        }
                        else
                        {
                            Console.WriteLine("Failed Transaction.");
                            if (response.transactionResponse.errors != null)
                            {
                                Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                                Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed Transaction.");
                        if (response.transactionResponse != null && response.transactionResponse.errors != null)
                        {
                            Console.WriteLine("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            Console.WriteLine("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                        else
                        {
                            Console.WriteLine("Error Code: " + response.messages.message[0].code);
                            Console.WriteLine("Error message: " + response.messages.message[0].text);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Null Response.");
                }
                
            }
            catch
            {

            }
            return result;
        }


        #endregion

        public async Task<PaginationUtility<VsiriusOrderDto>> GetData(PaginationParam pagination, OrderParams param)
        {
            var predicate = PredicateBuilder.New<VsiriusOrders>(true);

            if (!string.IsNullOrWhiteSpace(param.StartDate) && !string.IsNullOrWhiteSpace(param.EndDate))
                predicate.And(
                     x => x.CreateOrderDate.HasValue &&
                     x.CreateOrderDate.Value >= Convert.ToDateTime($"{param.StartDate} 00:00:00.000") &&
                     x.CreateOrderDate.Value <= Convert.ToDateTime($"{param.EndDate} 23:59:59.997")
                 );

            try
            {
                var data = await _orderRepository.FindAll(predicate)
                              .Join(_orderStatusRepository.FindAll(),
                              x => x.StatusId,
                              y => y.Id,
                              (x, y) => new { Order = x, OrderStatus = y })
                              .FullTextSearchQuery(param.Keyword)
                             .Select(x => new VsiriusOrderDto()
                             {

                                 OrderId = x.Order.OrderId,
                                 CartId = x.Order.CartId,
                                 Address1 = x.Order.Address1,
                                 Address2 = x.Order.Address2,
                                 BirthDay = x.Order.BirthDay,
                                 City = x.Order.City,
                                 Country = x.Order.Country,
                                 CreateOrderDate = x.Order.CreateOrderDate,
                                 Email = x.Order.Email,
                                 FirstName = x.Order.FirstName,
                                 Gender = x.Order.Gender,
                                 LastName = x.Order.LastName,
                                 Note = x.Order.Note,
                                 PostCode = x.Order.PostCode,
                                 Shipping = x.Order.Shipping,
                                 PaymentMethod = x.Order.PaymentMethod,
                                 State = x.Order.State,
                                 StatusId = x.OrderStatus.Id,
                                 StatusName = x.OrderStatus.Name,
                                 TotalPrice = (decimal)x.Order.TotalPrice,
                                 TotalQuantityItem = x.Order.TotalQuantityItem
                             }).ToListAsync();

                return PaginationUtility<VsiriusOrderDto>.Create(data, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
            
        }

        public async Task<VsiriusOrderDto> GetDetail(string orderId)
        {
            List<VsiriusProduct> listProduct = new();
            VsiriusOrderDto vsiriusOrder = new();

            try
            {
                var data = await _orderDetailRepository.FindAll(x => x.OrderId == orderId)
                       .Select(x => new VsiriusOrderDto()
                       {
                           OrderId = x.OrderId,
                           ProductId = x.ProductId,
                           Quantity = x.QuantityItem,
                           Price = x.Price
                       }).ToListAsync();

                foreach (var item in data)
                {
                    var product = await _productRepository.FirstOrDefaultAsync(x => x.item_sku == item.ProductId);
                    var order = await _orderRepository.FirstOrDefaultAsync(x => x.OrderId == item.OrderId);
                    if (product != null)
                    {
                        product.quantity = item.Quantity.ToString();
                        product.standard_price = item.Price.ToString();
                        listProduct.Add(product);
                    }
                    if (order != null)
                    {
                        vsiriusOrder = _mapper.Map<VsiriusOrderDto>(order);
                    }
                }
                vsiriusOrder.ListProducts = listProduct;
                return vsiriusOrder;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new VsiriusOrderDto();
            }
        }

        public async Task<PaginationUtility<OrderHistory>> GetListOrderHistory(PaginationParam pagination,string email)
        {
            List<OrderHistory> orderHistories = new();

            try
            {
                var listOrders = await _orderRepository.FindAll(x => x.Email == email)
                                    .Join(_orderStatusRepository.FindAll(),
                                    x => x.StatusId,
                                    y => y.Id,
                                    (x, y) => new { Order = x, OrderStatus = y })
                                    .Select(x => new VsiriusOrderDto()
                                    {
                                        OrderId = x.Order.OrderId,
                                        Address1 = x.Order.Address1,
                                        StatusId = x.Order.StatusId,
                                        Address2 = x.Order.Address2,
                                        BirthDay = x.Order.BirthDay,
                                        CartId = x.Order.CartId,
                                        City = x.Order.City,
                                        Country = x.Order.Country,
                                        CreateOrderDate = x.Order.CreateOrderDate,
                                        Email = x.Order.Email,
                                        FirstName = x.Order.FirstName,
                                        LastName = x.Order.LastName,
                                        Gender = x.Order.Gender,
                                        Note = x.Order.Note,
                                        PaymentMethod = x.Order.PaymentMethod,
                                        Phone = x.Order.Phone,
                                        PostCode = x.Order.PostCode,
                                        Shipping = x.Order.Shipping,
                                        State = x.Order.State,
                                        TotalPrice = (decimal)x.Order.TotalPrice,
                                        TotalQuantityItem = x.Order.TotalQuantityItem,
                                        StatusName = x.OrderStatus.Name
                                    }).ToListAsync();

                if (listOrders.Count == 0)
                {
                    return null;
                }

                foreach (var order in listOrders)
                {
                    OrderHistory orderHistory = new();
                    orderHistory.Orders = order;
                    var orderDetails = await _orderDetailRepository.FindAll(x => x.OrderId == order.OrderId).ToListAsync();
                    foreach (var item in orderDetails)
                    {
                        var product = await _productRepository.FirstOrDefaultAsync(x => x.item_sku == item.ProductId);
                        product.quantity = item.QuantityItem.ToString();
                        product.standard_price = item.Price.ToString();
                        if (product != null)
                        {
                            orderHistory.Product = product;
                        }
                    }
                    orderHistories.Add(orderHistory);
                }

                return PaginationUtility<OrderHistory>.Create(orderHistories, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
        }

        public async Task<List<VsiriusOrderDto>> TopOrders()
        {
            try
            {
                return await _orderRepository.FindAll()
                          .Join(_userRepository.FindAll(),
                          x => x.Email,
                          y => y.Email,
                          (x, y) => new { Order = x, User = y })
                          .Join(_orderStatusRepository.FindAll(),
                          x => x.Order.StatusId,
                          y => y.Id,
                          (x, y) => new { x.Order, x.User, OrderStatus = y })
                          .Where(x => x.Order.StatusId == 1)
                          .Select(x => new VsiriusOrderDto()
                          {
                              FullName = x.User.FullName,
                              CreateOrderDate = x.Order.CreateOrderDate,
                              OrderId = x.Order.OrderId,
                              Email = x.User.Email,
                              StatusId = x.Order.StatusId,
                              StatusName = x.OrderStatus.Name,
                              isRead = x.Order.isRead
                          })
                         .OrderByDescending(x => x.CreateOrderDate).Take(20).ToListAsync();
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<VsiriusOrderDto>();
            }
            
        }

        public async Task<OperationResult> Update(VsiriusOrderDto dto)
        {
            var data = _mapper.Map<VsiriusOrders>(dto);
            _orderRepository.Update(data);
            try
            {
                await _orderRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Updated Order Successfully" };
            }
            catch (System.Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}
