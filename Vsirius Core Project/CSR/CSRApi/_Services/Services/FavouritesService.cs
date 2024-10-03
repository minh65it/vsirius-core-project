using AutoMapper;
using CSRApi._Repositories.Interfaces;
using CSRApi._Services.Interfaces;
using CSRApi.Dtos;
using CSRApi.Helpers.Utilities;
using Microsoft.EntityFrameworkCore;
using VsiriusDataModel.DataModel;
using VSiriusSystemLog.WriteLog;

namespace CSRApi._Services.Services
{
    public class FavouritesService : IFavouritesService
    {
        private readonly IFavouritesRepository _favouritesRepository;
        private readonly IVsiriusProductRepository _vsiriusProductRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ILoggerCustom _logService;
        public FavouritesService(IFavouritesRepository favouritesRepository, IVsiriusProductRepository vsiriusProductRepository, IUserRepository userRepository,
            IMapper mapper, IHttpContextAccessor httpContextAccessor, ILoggerCustom logService)
        {
            _favouritesRepository = favouritesRepository;
            _vsiriusProductRepository = vsiriusProductRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logService = logService;
        }

        public async Task<OperationResult> Create(FavouritesDto dto)
        {
            var checkExist = await _favouritesRepository.FindSingle(x => x.ProductId == dto.ProductId && x.UserId == dto.UserId);
            if (checkExist != null)
            {
                return new OperationResult { Success = false, Message = "The product has been liked" };
            }
            var data = _mapper.Map<Favourites>(dto);
            try
            {
                _favouritesRepository.Add(data);
                await _favouritesRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Liked the product", Data = data };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<OperationResult> Delete(string id)
        {
            var data = await _favouritesRepository.FindSingle(x => x.Id.ToString() == id);
            try
            {
                _favouritesRepository.Remove(data);
                await _favouritesRepository.SaveChangesAsync();
                return new OperationResult { Success = true, Message = "Dislike the product" };
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        public async Task<List<FavouritesDto>> GetAll(string userId)
        {
            try
            {
                var data = await _favouritesRepository.FindAll().Join(
              _vsiriusProductRepository.FindAll(),
              x => x.ProductId,
              y => y.item_sku,
              (x, y) => new { Favourite = x, Product = y })
              .Join(_userRepository.FindAll(x => x.Id == userId),
              x => x.Favourite.UserId,
              y => y.Id,
              (x, y) => new { x.Product, x.Favourite, User = y })
              .Select(x => new FavouritesDto()
              {
                  item_sku = x.Product.item_sku,
                  item_name = x.Product.item_name,
                  Id = x.Favourite.Id,
                  bullet_point = x.Product.bullet_point1,
                  CreatedDate = x.Favourite.CreatedDate,
                  main_image_url = x.Product.main_image_url,
                  ProductId = x.Product.item_sku,
                  quantity = x.Product.quantity,
                  standard_price = x.Product.standard_price.ToString(),
                  price_seo = x.Product.sale_price,
                  UserId = x.User.Id
              }).ToListAsync();
                return data;
            }
            catch(Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return new List<FavouritesDto>();
            }
           
        }

        public async Task<PaginationUtility<FavouritesDto>> GetData(PaginationParam pagination, string userId)
        {
            try
            {
                var data = _favouritesRepository.FindAll().Join(
              _vsiriusProductRepository.FindAll(),
              x => x.ProductId,
              y => y.item_sku,
              (x, y) => new { Favourite = x, Product = y })
              .Join(_userRepository.FindAll(x => x.Id == userId),
              x => x.Favourite.UserId,
              y => y.Id,
              (x, y) => new { x.Product, x.Favourite, User = y })
              .Select(x => new FavouritesDto()
              {
                  item_sku = x.Product.item_sku,
                  item_name = x.Product.item_name,
                  Id = x.Favourite.Id,
                  bullet_point = x.Product.bullet_point1,
                  CreatedDate = x.Favourite.CreatedDate,
                  main_image_url = x.Product.main_image_url,
                  ProductId = x.Product.item_sku,
                  quantity = x.Product.quantity,
                  standard_price = x.Product.standard_price.ToString(),
                  price_seo = x.Product.sale_price,
                  UserId = x.User.Id
              });
                return await PaginationUtility<FavouritesDto>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
            }
            catch(Exception ex)
            {
                await _logService.CreateLogError(exception: ex.Message);
                return null;
            }
          
        }
    }
}
