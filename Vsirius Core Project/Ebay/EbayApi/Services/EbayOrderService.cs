using EbayApi.Services.Interfaces;
using System.Threading.Tasks;
using EbayApi.DataContext;
using EbayApi.Utils;
using EbayApi.Dtos;
using System;
using System.Linq;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;

namespace EbayApi.Services
{
    public class EbayOrderService : IEbayOrderService
    {
        private readonly EbayContext _context;

        public EbayOrderService(EbayContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> GetOrder(string orderId)
        {
            return AutoMapperHelper.Mapper.Map<OrderDto>(await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId));
        }

        public async Task<PaginationUtil<OrderDto>> SearchOrders(PaginationParam pagination, SearchParam searchParam)
        {
            var query = _context.Orders.AsNoTracking().AsQueryable().Include(o => o.OrderDetails);
            if (searchParam != null)
            {
                if (!string.IsNullOrWhiteSpace(searchParam.Ids))
                {
                    query = query.Where(x => searchParam.Ids.Contains(x.OrderId));
                }
                if (!string.IsNullOrWhiteSpace(searchParam.KeySearch))
                {
                    query = query.Where(x => x.OrderId.Contains(searchParam.KeySearch)
                    || x.Address1.Contains(searchParam.KeySearch)
                    || x.Address2.Contains(searchParam.KeySearch)
                    || x.CustomerName.Contains(searchParam.KeySearch)
                    || x.City.Contains(searchParam.KeySearch)
                    || x.ZipCode.Contains(searchParam.KeySearch)
                    || x.State.Contains(searchParam.KeySearch)
                    || x.TrackingNo.Contains(searchParam.KeySearch)
                    || x.OrderDetails.Any(a => a.Sku.Contains(searchParam.KeySearch))
                    );
                }
            }

            return await PaginationUtil<OrderDto>.CreateAsync(
                query.OrderByDescending(x => x.OrderDate)
                .ProjectTo<OrderDto>(AutoMapperHelper.Mapper.ConfigurationProvider),
                pagination.PageNumber,
                pagination.PageSize,
                string.IsNullOrWhiteSpace(searchParam?.KeySearch));
        }
    }
}