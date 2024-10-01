using ExcludedProductAPI._Repositories.Interfaces;
using ExcludedProductAPI._Services.Interfaces;
using ExcludedProductAPI.DTOs;
using ExcludedProductAPI.Helpers;
using ExcludedProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ExcludedProductAPI._Services.Repositories
{
    public class ExcludedProductService : IExcludedProductService
    {
        private readonly IExcludedProductRepository _repository;

        public ExcludedProductService(IExcludedProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<ExcludedProductDTO?>> Create(ExcludedProductDTO dto)
        {
            if(await _repository.AnyAsync(x => x.MarketplaceSKU.Equals(dto.MarketplaceSKU)))
            {
                return new OperationResult<ExcludedProductDTO?>() {
                Success = false,
                Data = null,
                Message = "Product already existed"
                };
            }
            else
            {
                var entity = CustomMapper.ChangeExcludedDtoToEntity(dto);
                _repository.Add(entity);
                await _repository.SaveChangesAsync();
                return new OperationResult<ExcludedProductDTO?>()
                {
                    Success = true,
                    Data = dto
                };
            }
        }

        public async Task<OperationResult<object?>> Delete(string id)
        {
            var entity = await _repository.FirstOrDefaultAsync(x => x.MarketplaceSKU.Equals(id));
            if (entity == null)
            {
                return new OperationResult<object?>()
                {
                    Success = false,
                    Data = null,
                    Message = "Product not exist"
                };
            }
            else
            {
                _repository.Remove(entity);
                await _repository.SaveChangesAsync();
                return new OperationResult<object?>()
                {
                    Success = true,
                    Message = "Delete success",
                    Data = null
                };
            }
        }

        public async Task<PaginationUtil<ExcludedProductDTO>> GetAll(PaginationParam pagination, ExcludedProductParam search)
        {
            IQueryable<ExcludedProduct> query = _repository.FindAll();
            if(search != null)
            {
                if (!string.IsNullOrEmpty(search.MarketplaceSKU))
                {
                    query = query.Where(x => x.MarketplaceSKU.ToLower().Equals(search.MarketplaceSKU.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.Marketplace))
                {
                    query = query.Where(x => x.Marketplace.ToLower().Equals(search.Marketplace.ToLower()));
                }
                if (!string.IsNullOrEmpty(search.SupplierSKU))
                {
                    query = query.Where(x => x.SupplierSKU.ToLower().Equals(search.SupplierSKU.ToLower()));
                }
                if (search.ExclusionType != null)
                {
                    query = query.Where(x => x.ExclusionType == search.ExclusionType);
                }
            }
            var queryDto = query.AsNoTracking().Select(x => CustomMapper.ChangeExcludedProductToDTO(x));

            var rs = await PaginationUtil<ExcludedProductDTO>.CreateAsync(queryDto, pagination.PageNumber, pagination.PageSize);

            return rs;
        }

        public async Task<ExcludedProductDTO?> GetDetail(string id)
        {
            var entity = await _repository.FindSingle(x => x.MarketplaceSKU.Equals(id));
            var dto = CustomMapper.ChangeExcludedProductToDTO(entity);
            return dto;
        }

        public async Task<OperationResult<ExcludedProductDTO?>> Update(ExcludedProductDTO dto)
        {
            if (!await _repository.AnyAsync(x => x.MarketplaceSKU.Equals(dto.MarketplaceSKU)))
            {
                return new OperationResult<ExcludedProductDTO?>()
                {
                    Success = false,
                    Data = null,
                    Message = "Product not exist"
                };
            }
            else
            {
                var entity = CustomMapper.ChangeExcludedDtoToEntity(dto);
                _repository.Update(entity);
                await _repository.SaveChangesAsync();
                return new OperationResult<ExcludedProductDTO?>()
                {
                    Success = true,
                    Data = dto
                };
            }
        }
    }
}
