using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using SkuMappingAPI._Repositories.Interfaces;
using SkuMappingAPI.Common;
using SkuMappingAPI.Data;
using SkuMappingAPI.DTOs;
using SkuMappingAPI.Models;
using SkuMappingAPI.Services.Interfaces;

namespace SkuMappingAPI._Services.Services
{
    public class MappingSkuService : IMappingSkuService
    {
        private readonly MappingSkuDbContext _dbContext;
        private readonly IMappingSkuRepository _mappingSkuRepository;
        private readonly IMapper _mapper;
        private Result _result;
        public MappingSkuService(
            IMappingSkuRepository mappingSkuRepository, 
            IMapper mapper,
            Result result,
            MappingSkuDbContext dbContext
            )
        {
            _mappingSkuRepository = mappingSkuRepository;
            _dbContext = dbContext;
            _mapper = mapper;
            _result = result;
        }

        public async Task<Result> BulkSkuWalmartQgold(List<QgoldToWalmartDTO> dto)
        {
            var lstQgold = dto.Select(x => x.SKUQgold).ToList();
            List<SKUInfo> lstUpdate = await _mappingSkuRepository.FindAll(x=> lstQgold.Contains(x.SKUQgold)).ToListAsync();
            foreach (SKUInfo info in lstUpdate)
            {
                var obj = dto.FirstOrDefault(x => x.SKUQgold == info.SKUQgold);
                if(obj != null)
                {
                    info.SKUQgold = obj.SKUQgold;
                    info.SKUWalmart = obj.SKUQgold;
                }
            }
            var lstUpdateQgoldSKU = lstUpdate.Select(x => x.SKUQgold).ToList();

            if(lstUpdate.Any()) 
            {
                _dbContext.BulkUpdate(lstUpdate);
            }

            List<SKUInfo> lstInsert = dto.Where(x => !lstUpdateQgoldSKU.Contains(x.SKUQgold)).Select(x => new SKUInfo()
            {
                Id = Guid.NewGuid(),
                SKUQgold = x.SKUQgold,
                SKUWalmart = x.SKUWalmart
            }).ToList();

            _dbContext.BulkInsert(lstInsert);
           _result.IsSuccess = true;
            return _result;
        }

        public async Task<PaginationUtility<QgoldToWalmartDTO>> GetSkuWalmartFromQgold(PaginationParam pagination)
        {
            var query = _mappingSkuRepository.FindAll().Select(x => new QgoldToWalmartDTO()
            {
                SKUQgold = x.SKUQgold,
                SKUWalmart = x.SKUWalmart
            });
            return await PaginationUtility<QgoldToWalmartDTO>.CreateAsync(query, pageNumber: pagination.PageNumber, pageSize: pagination.PageSize, isPaging: pagination.IsPaging);

        }
    }
}
