using AutoMapper;
using AutoMapper.QueryableExtensions;
using EbayCoreApi._Services.Interfaces;
using EbayCoreApi.Common.Helpers;
using EbayCoreApi.DataContext;
using EbayCoreApi.Dtos;
using EbayCoreApi.Models;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace EbayCoreApi._Services.Services
{
    public class EbayOrderService : IEbayOrderService
    {
        private readonly EbayContext _context;
        private readonly IMapper _mapper;

        public EbayOrderService(EbayContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderDto> GetOrder(string orderId)
        {
            return _mapper.Map<OrderDto>(await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId));
        }

        public async Task<PaginationUtil<OrderDto>> SearchOrders(PaginationParam pagination, SearchOrderParam searchParam)
        {
            IQueryable<Order> query = _context.Orders.AsNoTracking().Include(o => o.OrderDetails);
            var fastCheck = typeof(SearchOrderParam).GetProperties().All(p => p.GetValue(searchParam) == null);
            if (!fastCheck)
            {
                if (!string.IsNullOrWhiteSpace(searchParam.ids))
                {
                    query = query.Where(x => x.OrderId.Contains(searchParam.ids));
                }


                if (searchParam.supplier != null || searchParam.sKU != null)
                {
                    var predicate = PredicateBuilder.New<Order>();
                    if (searchParam.supplier != null)
                    {
                        var sup = searchParam.supplier[0].ToString();
                        predicate = predicate.And(o => o.OrderDetails.Any(od => od.Sku.StartsWith(sup)));
                    }
                    if (searchParam.sKU != null)
                    {
                        predicate = predicate.And(o => o.OrderDetails.Any(od => od.Sku == searchParam.sKU));
                    }

                    query = query.Where(predicate);
                }
                if (!string.IsNullOrEmpty(searchParam.fromDate) && !string.IsNullOrEmpty(searchParam.toDate))
                {
                    var fromDate = DateTime.Parse(searchParam.fromDate);
                    var toDate = DateTime.Parse(searchParam.toDate).AddDays(1).AddMilliseconds(-1);
                    var predicate = PredicateBuilder.New<Order>();
                    predicate = predicate.And(x => x.OrderDate > fromDate && x.OrderDate < toDate);
                    query = query.Where(predicate);
                }
                if (!string.IsNullOrEmpty(searchParam.address1) || !string.IsNullOrEmpty(searchParam.address2))
                {
                    if (!string.IsNullOrEmpty(searchParam.address1))
                    {
                        query = query.Where(x => x.Address1.Contains(searchParam.address1));
                    }
                    else if (!string.IsNullOrEmpty(searchParam.address2))
                    {
                        query = query.Where(x => x.Address2.Contains(searchParam.address2));
                    }
                    else
                    {
                        query = query.Where(x => x.Address1.Contains(searchParam.address1) && x.Address2.Contains(searchParam.address2));
                    }
                }

                if (searchParam.submited != null)
                {
                    query = query.Where(x => x.IsSubmit == searchParam.submited);
                }
                if (searchParam.buyerName != null)
                {
                    query = query.Where(x => x.CustomerName.Contains(searchParam.buyerName));
                }
            }

            return await PaginationUtil<OrderDto>.CreateAsync(
                query.OrderByDescending(x => x.OrderDate)
                .ProjectTo<OrderDto>(_mapper.ConfigurationProvider),
                pagination.PageNumber,
                pagination.PageSize,
                searchParam != null);
        }
    }
}