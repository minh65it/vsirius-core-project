using AutoMapper;
using OrdersAPI._Repositories.Interfaces;
using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;
using OrdersAPI.Helpers.Utilities;
using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataModel;
using Korzh.EasyQuery.Linq;
using OrdersAPI.Data;

namespace OrdersAPI._Services.Services
{
    public class VsiriusCartService : IVsiriusCartService
    {
        private readonly IVsiriusCartRepository _vsiriusCartRepository;
        private readonly IVsiriusCartDetailsRepository _vsiriusCartDetailsRepository;
        private readonly IVsiriusProductRepository _vsiriusProductRepository;
        private readonly IMapper _mapper;
        private readonly ILogService _logService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;
        public VsiriusCartService(IVsiriusCartRepository vsiriusCartRepository, IVsiriusCartDetailsRepository vsiriusCartDetailsRepository,
            IVsiriusProductRepository vsiriusProductRepository, IMapper mapper, ILogService logService, IHttpContextAccessor httpContextAccessor, DataContext context)
        {
            _vsiriusCartRepository = vsiriusCartRepository;
            _vsiriusCartDetailsRepository = vsiriusCartDetailsRepository;
            _vsiriusProductRepository = vsiriusProductRepository;
            _mapper = mapper;
            _logService = logService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<PaginationUtility<VsiriusListCartDto>> ListCartAll(PaginationParam pagination, string? fullTextSearch, bool isPaging = true)
        {
            try
            {
                var data = _vsiriusCartRepository.FindAll(x => x.VsiriusCartDetails)
                               .Select(x => new VsiriusListCartDto()
                               {
                                   CartId = x.CartId,
                                   TotalPrice = x.TotalPrice,
                                   UserId = x.UserId,
                                   UserName = x.UserName,
                                   Status = x.Status,
                                   Email = x.Email,
                                   Tax = x.Tax,
                                   QuantityItem = (int)x.VsiriusCartDetails.Where(z => z.IsDelete == false).Sum(d => d.Quantity)
                               })
                               .FullTextSearchQuery(fullTextSearch).Distinct().AsNoTracking();

                return await PaginationUtility<VsiriusListCartDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return null;
            }
        }

        public async Task<List<VsiriusListCartDetailsDto>> GetCartDetailByCartId(Guid cartId, bool isDelete)
        {
            try
            {
                var result = await _vsiriusCartDetailsRepository.FindAll(x => x.CartId == cartId && x.IsDelete == isDelete).ToListAsync();
                List<VsiriusListCartDetailsDto> listCartDetail = new List<VsiriusListCartDetailsDto>();

                foreach (var item in result)
                {
                    var product = _vsiriusProductRepository.FirstOrDefault(x => x.item_sku == item.ProductId);

                    var product1 = _vsiriusProductRepository.FirstOrDefault(x => x.item_sku == item.ProductId);
                    VsiriusListCartDetailsDto cartDetail = new VsiriusListCartDetailsDto
                    {
                        PriceSale = item.PriceSale,
                        CartDetailsId = item.CartDetailId,
                        CreateDate = item.CreateDate,
                        Price = item.Price,
                        InfoBrowser = item.InfoBrowser,
                        Quantity = item.Quantity,
                        ProductId = product.item_sku,
                        ProductName = product.item_name,
                        Image = product.main_image_url,
                        Size = product.size_name,
                        Color = product.color_name
                    };
                    listCartDetail.Add(cartDetail);
                }

                return listCartDetail;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new List<VsiriusListCartDetailsDto>();
            }
            
        }

        public async Task<VsiriusListCartDto> ListCartByUser(Guid userId, string? email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                await TransferCartIfLoginEmail(userId, email);
            }

            try
            {
                VsiriusCart dataCart = new VsiriusCart();
                if (string.IsNullOrWhiteSpace(email))
                {
                    dataCart = await _vsiriusCartRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.Status == false,
                                                                   y => y.VsiriusCartDetails.Where(x => x.IsDelete == false));
                }
                else
                {
                    dataCart = await _vsiriusCartRepository.FirstOrDefaultAsync(x => x.Email == email && x.Status == false,
                                                                    y => y.VsiriusCartDetails.Where(x => x.IsDelete == false));
                }

                if (dataCart == null) return null;

                var dataCartDetails = dataCart.VsiriusCartDetails
                                   .Select(x => new VsiriusListCartDto
                                   {
                                       CartId = x.CartId,
                                       TotalPrice = x.VsiriusCart.TotalPrice,
                                       QuantityItem = (int)x.VsiriusCart.VsiriusCartDetails.Sum(s => s.Quantity),
                                       UserId = x.VsiriusCart.UserId,
                                       UserName = x.VsiriusCart.UserName,
                                       Status = x.VsiriusCart.Status,
                                       Email = x.VsiriusCart.Email,
                                       Tax = x.VsiriusCart.Tax,
                                   }).FirstOrDefault();

                if (dataCartDetails == null) return null;

                List<VsiriusListCartDetailsDto> listCartDetail = new List<VsiriusListCartDetailsDto>();

                foreach (var item in dataCart.VsiriusCartDetails)
                {
                    var product = _vsiriusProductRepository.FirstOrDefault(x => x.item_sku == item.ProductId);

                    var product1 = _vsiriusProductRepository.FirstOrDefault(x => x.item_sku == item.ProductId);
                    VsiriusListCartDetailsDto cartDetail = new VsiriusListCartDetailsDto
                    {
                        PriceSale = item.PriceSale,
                        CartDetailsId = item.CartDetailId,
                        CreateDate = item.CreateDate,
                        Price = item.Price,
                        InfoBrowser = item.InfoBrowser,
                        Quantity = item.Quantity,
                        ProductId = product.item_sku,
                        ProductName = product.item_name,
                        Image = product.main_image_url,
                        Size = product.size_name,
                        Color = product.color_name
                    };
                    listCartDetail.Add(cartDetail);
                }

                dataCartDetails.ListCartDetailsDtos = listCartDetail;

                return dataCartDetails;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new VsiriusListCartDto();
            }
            
        }

        public async Task<OperationResult> AddCartOrDelete(VsiriusCartDetailsDto dtoCartDetail, HttpHeaderDto httpHeader)
        {
            try
            {
                VsiriusCart resultCart = new VsiriusCart();
                if (!string.IsNullOrWhiteSpace(dtoCartDetail.Email))
                {
                    resultCart = await _vsiriusCartRepository.FirstOrDefaultAsync(x => x.Email == dtoCartDetail.Email && x.Status == false);
                    if (resultCart == null) 
                    {
                        VsiriusCartDto cartDto = new VsiriusCartDto();
                        resultCart = _mapper.Map<VsiriusCart>(cartDto);

                        resultCart.CartId = Guid.NewGuid();
                        resultCart.TotalPrice = 0;
                        resultCart.UserName = dtoCartDetail.UserName;
                        resultCart.Email = dtoCartDetail.Email;
                        resultCart.Status = false;
                        resultCart.Tax = 0;
                        _vsiriusCartRepository.Add(resultCart);
                        await _vsiriusCartRepository.SaveChangesAsync();
                    }
                }
                else
                {
                     resultCart = await _vsiriusCartRepository.FirstOrDefaultAsync(x => x.UserId == dtoCartDetail.UserId && x.Status == false);

                    if (resultCart == null)
                    {
                        VsiriusCartDto cartDto = new VsiriusCartDto();
                        resultCart = _mapper.Map<VsiriusCart>(cartDto);

                        resultCart.CartId = Guid.NewGuid();
                        resultCart.TotalPrice = 0;
                        resultCart.UserId = dtoCartDetail.UserId;
                        resultCart.UserName = dtoCartDetail.UserName;
                        resultCart.Status = false;
                        resultCart.Tax = 0;
                        _vsiriusCartRepository.Add(resultCart);
                        await _vsiriusCartRepository.SaveChangesAsync();
                    }
                }


                // Add cart detail
                var resultProduct = await  _vsiriusProductRepository.FirstOrDefaultAsync(x => x.item_sku == dtoCartDetail.ProductId);
                if (resultProduct == null) return new OperationResult { Success = false, Message = "No data Product" };

                //if(dtoCartDetail.Quantity > int.Parse(resultProduct.quantity)) return new OperationResult { Success = false, Message = "Insufficient quantity in stock" };

                var resultCartDetail = _mapper.Map<VsiriusCartDetails>(dtoCartDetail);
                resultCartDetail.CartId = resultCart.CartId;
                resultCartDetail.Price = decimal.Parse(resultProduct.standard_price);
                resultCartDetail.PriceSale = resultProduct.sale_price == null ? null : decimal.Parse(resultProduct.sale_price);
                resultCartDetail.InfoBrowser = httpHeader.UserAgent;
                resultCartDetail.CreateDate = httpHeader.Date;
                resultCartDetail.IsDelete = false;

                var resultCheckProductOnCart = await _vsiriusCartDetailsRepository
                    .FirstOrDefaultAsync(x => x.CartId == resultCart.CartId 
                    && x.ProductId == resultProduct.item_sku && x.IsDelete == false);

                if (resultCheckProductOnCart == null)
                {
                    _vsiriusCartDetailsRepository.Add(resultCartDetail);
                }
                else
                {
                    resultCheckProductOnCart.Quantity = resultCheckProductOnCart.Quantity + resultCartDetail.Quantity;
                    if(resultCheckProductOnCart.Quantity <= 0)
                    {
                        resultCheckProductOnCart.IsDelete = true;
                        resultCheckProductOnCart.Quantity = 0;
                    }
                    _vsiriusCartDetailsRepository.Update(resultCheckProductOnCart);
                }
                await _vsiriusCartDetailsRepository.SaveChangesAsync();


                //update total price
                bool saveTotal = await UpdateTotalPriceCart(resultCartDetail);

                if (saveTotal)
                {
                    return new OperationResult { Success = true, Message = "Add Data Cart Successfully" };
                }

                return new OperationResult { Success = false, Message = "Add Data Cart Failed" };

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<bool> TransferCartIfLoginEmail(Guid userId, string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var resultCartUserId = await _vsiriusCartRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.Status == false,
                                                                              y => y.VsiriusCartDetails.Where(y => y.IsDelete == false));
                if (resultCartUserId == null) return false;

                var resultCartEmail = await _vsiriusCartRepository.FirstOrDefaultAsync(x => x.Email == email && x.Status == false,
                                                                              y => y.VsiriusCartDetails.Where(y => y.IsDelete == false));
                //if (resultCartEmail == null) return false;

                if (resultCartEmail == null)
                {
                    resultCartUserId.UserId = null;
                    resultCartUserId.Email = email;
                    _vsiriusCartRepository.Update(resultCartUserId);
                    await _vsiriusCartDetailsRepository.SaveChangesAsync();

                    foreach (var item in resultCartUserId.VsiriusCartDetails)
                    {
                        await UpdateTotalPriceCart(item);
                    }
                }
                else
                {
                    foreach (var item in resultCartUserId.VsiriusCartDetails)
                    {
                        var checkProductOnCart = resultCartEmail.VsiriusCartDetails.FirstOrDefault(x => x.ProductId == item.ProductId);
                        if (checkProductOnCart != null)
                        {
                            item.Quantity = item.Quantity + checkProductOnCart.Quantity;
                            _vsiriusCartDetailsRepository.Remove(checkProductOnCart);
                        }

                        item.CartId = resultCartEmail.CartId;
                        _vsiriusCartDetailsRepository.Update(item);
                    }

                    if(string.IsNullOrEmpty(resultCartUserId.Email))
                    _vsiriusCartRepository.Remove(resultCartUserId);

                    await _vsiriusCartDetailsRepository.SaveChangesAsync();

                    foreach (var item in resultCartEmail.VsiriusCartDetails)
                    {
                        await UpdateTotalPriceCart(item);
                    }

                }
                return true;

            }
            catch (Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }

        public async Task<bool> UpdateTotalPriceCart(VsiriusCartDetails cartDetail)
        {
            try
            {
                var resultProductTotalPrice = await _vsiriusCartRepository.FirstOrDefaultAsync(x => x.CartId == cartDetail.CartId);
                //var resultProductPrice = await _vsiriusCartDetailsRepository.FindAll(x => x.CartId == cartDetail.CartId && x.IsDelete == false).ToListAsync();
                var resultProductPrice = await _context.VsiriusCartDetails.Where(x => x.CartId == cartDetail.CartId && x.IsDelete == false).ToListAsync();

                if (resultProductTotalPrice == null) return false;

                decimal totalPrice = 0;
                foreach (var product in resultProductPrice)
                {
                    if (product.PriceSale == null)
                    {
                        var price = (decimal)product.Price * (decimal)product.Quantity;
                        totalPrice = totalPrice + price;
                    }
                    else
                    {
                        var price = (decimal)product.PriceSale * (decimal)product.Quantity;
                        totalPrice = totalPrice + price;
                    }

                }

                resultProductTotalPrice.TotalPrice = totalPrice;
                var result = _mapper.Map<VsiriusCart>(resultProductTotalPrice);
                _vsiriusCartRepository.Update(result);
                await _vsiriusCartRepository.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                await _logService.CreateLog(url: _httpContextAccessor.HttpContext.Request.Path, exception: ex.Message, level: LogLevel.Error);
                return false;
            }
        }
    }
}
