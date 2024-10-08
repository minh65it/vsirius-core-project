using AutoMapper;
using AutoMapper.QueryableExtensions;
using EbayCoreApi._Services.Interfaces;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.DataContext;
using EbayCoreApi.Dtos;
using EbayCoreApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EbayCoreApi._Services.Services
{
    public class EbayProductService : IEbayProductService
    {
        private readonly EbayContext _context;
        private readonly IMapper _mapper;

        public EbayProductService(
            EbayContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultUtil<ProductDto>> AddProduct(ProductDto dto)
        {
            ResultUtil<ProductDto> validateProduct = ValidateProduct(dto);
            if (validateProduct.IsSuccess == false)
            {
                return validateProduct;
            }
            if (await _context.Products.Where(x => x.IsDeleted == false).AnyAsync(x => x.Sku.Equals(dto.Sku)))
            {
                return new ResultUtil<ProductDto>(false, "Product already exists");
            }
            else
            {
                var entity = _mapper.Map<Product>(dto);
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedBy = dto.CreatedBy;
                _context.Products.Add(entity);
                return new ResultUtil<ProductDto>(
                     await _context.SaveChangesAsync() > 0
                     );
            }
        }

        private ResultUtil<ProductDto> ValidateProduct(ProductDto dto)
        {
            var rs = new ResultUtil<ProductDto>();
            var lstValidation = new List<KeyValuePair<string, object>>();
            var msg = string.Empty;
            if (!CategoryDictionary.Categories.Select(x => x.Key).Contains(dto.Category))
            {
                lstValidation.Add(new KeyValuePair<string, object>("Category", "Not exist"));
            }
            if (string.IsNullOrWhiteSpace(dto.Sku))
            {
                msg = "Required";
                if (dto.Sku.Length > 50)
                {
                    msg += ", Max length: 50";
                }
                lstValidation.Add(new KeyValuePair<string, object>("SKU", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.Tittle))
            {
                msg = "Required";
                if (dto.Tittle.Length > 80)
                {
                    msg += ", Max length: 80";
                }
                lstValidation.Add(new KeyValuePair<string, object>("Tittle", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                msg = "Required";
                if (dto.Description.Length > 800)
                {
                    msg += ", Max length: 800";
                }
                lstValidation.Add(new KeyValuePair<string, object>("Description", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.Brand))
            {
                msg = "Required";
                if (dto.Brand.Length > 100)
                {
                    msg += ", Max length: 100";
                }
                lstValidation.Add(new KeyValuePair<string, object>("Brand", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.ProductType))
            {
                msg = "Required";
                if (dto.ProductType.Length > 100)
                {
                    msg += ", Max length: 100";
                }
                lstValidation.Add(new KeyValuePair<string, object>("ProductType", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.ItemStyle))
            {
                msg = "Required";
                if (dto.ItemStyle.Length > 100)
                {
                    msg += ", Max length: 100";
                }
                lstValidation.Add(new KeyValuePair<string, object>("ItemStyle", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.Material))
            {
                msg = "Required";
                if (dto.Material.Length > 100)
                {
                    msg += ", Max length: 100";
                }
                lstValidation.Add(new KeyValuePair<string, object>("Material", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.MainImageUrl))
            {
                msg = "Required";
                if (dto.MainImageUrl.Length > 4000)
                {
                    msg += ", Max length: 4000";
                }
                lstValidation.Add(new KeyValuePair<string, object>("MainImageUrl", msg));
            }
            if (string.IsNullOrWhiteSpace(dto.CreatedBy))
            {
                lstValidation.Add(new KeyValuePair<string, object>("CreatedBy", "Required"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image1Url))
            {
                if (dto.Image1Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image1Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image2Url))
            {
                if (dto.Image2Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image2Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image3Url))
            {
                if (dto.Image3Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image3Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image4Url))
            {
                if (dto.Image4Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image4Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image5Url))
            {
                if (dto.Image5Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image5Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image6Url))
            {
                if (dto.Image6Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image6Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image7Url))
            {
                if (dto.Image7Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image7Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image8Url))
            {
                if (dto.Image8Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image8Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Image9Url))
            {
                if (dto.Image9Url.Length > 4000)
                    lstValidation.Add(new KeyValuePair<string, object>("Image9Url", "Max Length: 4000"));
            }
            if (!string.IsNullOrWhiteSpace(dto.ItemLength))
            {
                if (dto.ItemLength.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("ItemLength", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.Origin))
            {
                if (dto.Origin.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("Origin", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.ItemSize))
            {
                if (dto.ItemSize.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("ItemSize", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.ItemColor))
            {
                if (dto.ItemColor.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("ItemColor", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.PrimaryStoneType))
            {
                if (dto.PrimaryStoneType.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("PrimaryStoneType", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.PrimaryStoneSize))
            {
                if (dto.PrimaryStoneSize.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("PrimaryStoneSize", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.PrimaryStoneShape))
            {
                if (dto.PrimaryStoneShape.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("PrimaryStoneShape", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.ItemWeight))
            {
                if (dto.ItemWeight.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("ItemWeight", "Max Length: 100"));
            }
            if (!string.IsNullOrWhiteSpace(dto.ChainType))
            {
                if (dto.ChainType.Length > 100)
                    lstValidation.Add(new KeyValuePair<string, object>("ChainType", "Max Length: 100"));
            }

            if (lstValidation.Count > 0)
            {
                rs.IsSuccess = false;
                rs.Validation = lstValidation;
            }
            else
            {
                rs.IsSuccess = true;
            }
            return rs;
        }

        public async Task<ResultUtil<string>> DeleteProduct(string skus)
        {
            var lstId = skus.Trim().TrimEnd('|').Split('|');

            var lstEntity = await _context.Products.Where(x => x.IsDeleted == false).Where(x => skus.Contains(x.Sku)).ToListAsync();
            lstEntity.ForEach(x =>
            {
                x.IsDeleted = true;
                x.UpdatedDate = DateTime.Now;
            });

            return new ResultUtil<string>(await _context.SaveChangesAsync() > 0);
        }

        public async Task<ProductDto> GetProduct(string id)
        {
            return _mapper.Map<ProductDto>(await _context.Products
                    .Where(x => x.IsDeleted == false)
                    .FirstOrDefaultAsync(x => x.Sku == id)
                    );
        }

        public async Task<PaginationUtil<ProductDto>> SearchProducts(PaginationParam pagination, SearchParam searchParam)
        {
            var query = _context.Products.Where(x => x.IsDeleted == false);
            if (searchParam != null)
            {
                if (!string.IsNullOrWhiteSpace(searchParam.Ids))
                {
                    var lstId = searchParam.Ids.Trim().TrimEnd('|').Split('|');
                    query = query.Where(x => lstId.Contains(x.Sku));
                }
                if (!string.IsNullOrWhiteSpace(searchParam.KeySearch))
                {
                    query = query.Where(x => x.Tittle.Contains(searchParam.KeySearch));
                }
            }

            return await PaginationUtil<ProductDto>.CreateAsync(
                query.ProjectTo<ProductDto>(_mapper.ConfigurationProvider),
                pagination.PageNumber,
                pagination.PageSize,
                string.IsNullOrWhiteSpace(searchParam?.KeySearch));
        }

        public async Task<ResultUtil<ProductDto>> UpdateProduct(ProductDto dto)
        {
            ResultUtil<ProductDto> validateProduct = ValidateProduct(dto);
            if (validateProduct.IsSuccess == false)
            {
                return validateProduct;
            }
            var existingProduct = await _context.Products.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Sku == dto.Sku);
            if (existingProduct != null)
            {
                var entity = _mapper.Map<Product>(dto);
                entity.CreatedBy = existingProduct.CreatedBy;
                entity.CreatedDate = existingProduct.CreatedDate;
                entity.UpdatedBy = dto.CreatedBy;
                entity.UpdatedDate = DateTime.Now;
                _context.Entry(existingProduct).CurrentValues.SetValues(entity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return new ResultUtil<ProductDto>(true, "Product updated successfully", dto);
                }
                else
                {
                    return new ResultUtil<ProductDto>(false, "System error", dto);
                }
            }
            else
            {
                return new ResultUtil<ProductDto>(false, "Cannot find product");
            }
        }
    }
}