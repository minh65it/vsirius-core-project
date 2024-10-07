using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using PimApi._Repositories.Interfaces;
using PimApi._Services.Interfaces;
using PimApi.Dtos;
using PimApi.Dtos.OrderDto;
using PimApi.Helpers.Utilities;
using PimApi.Models;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace PimApi._Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository,
            MapperConfiguration mapperConfiguration,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapperConfiguration = mapperConfiguration;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PaginationUtility<OrderDto>> GetOrder(PaginationParam pagination, SearchOrderParams searchParams)
        {
            var predicate = PredicateBuilder.New<Order>(true);
            if (!string.IsNullOrWhiteSpace(searchParams.ItemSku))
            {
                predicate.And(x => x.ItemSku.Contains(searchParams.ItemSku));
            }
            if (!string.IsNullOrWhiteSpace(searchParams.ShipToName))
            {
                predicate.And(x => x.ShipToName == searchParams.ShipToName);
            }
            if (!string.IsNullOrWhiteSpace(searchParams.Marketplace))
            {
                predicate.And(x => x.Marketplace == searchParams.Marketplace);
            }
            var query = _orderRepository.FindAll(predicate).ProjectTo<OrderDto>(_mapperConfiguration);

            if (searchParams.isGroupQuantity != null && searchParams.isGroupQuantity == true)
            {
                query = query.GroupBy(o => new { o.ItemSku, o.Marketplace })
                    .Select(g => new OrderDto()
                    {

                        ItemSku = g.Key.ItemSku,
                        OrderId = g.First().OrderId,
                        ShipToName = g.First().ShipToName,
                        ShipToPhone = g.First().ShipToPhone,
                        ShipToAddress = g.First().ShipToAddress,
                        ShipToZip = g.First().ShipToZip,
                        MethodCode = g.First().MethodCode,
                        TrackingId = g.First().TrackingId,
                        OrderQuantity = g.Sum(x => x.OrderQuantity),
                        ReturnQuantity = g.Sum(x => x.ReturnQuantity),
                        CreatedDate = g.First().CreatedDate,
                        Marketplace = g.Key.Marketplace,
                    });
            }
            if (searchParams.isSortQuantity != null && searchParams.isSortQuantity == true)
            {
                query = query.OrderByDescending(x => x.OrderQuantity);
            }
            bool isPagging = searchParams.isPagging ?? false;
            var result = await PaginationUtility<OrderDto>.CreateCustomAsync(query, pagination.PageNumber, pagination.PageSize, isPagging);

            return result;
        }

        public async Task<PaginationUtility<ProductDto>> GetProductTop(PaginationParam pagination, SearchOrderParams searchParams)
        {
            var predicate = PredicateBuilder.New<Order>(true);
            if (!string.IsNullOrWhiteSpace(searchParams.ItemSku))
            {
                predicate.And(x => x.ItemSku.Contains(searchParams.ItemSku));
            }
            if (!string.IsNullOrWhiteSpace(searchParams.ShipToName))
            {
                predicate.And(x => x.ShipToName == searchParams.ShipToName);
            }
            if (!string.IsNullOrWhiteSpace(searchParams.Marketplace))
            {
                predicate.And(x => x.Marketplace == searchParams.Marketplace);
            }
            var order = _orderRepository.FindAll(predicate).ProjectTo<OrderDto>(_mapperConfiguration);

            if (searchParams.isGroupQuantity != null && searchParams.isGroupQuantity == true)
            {
                order = order.GroupBy(o => new { o.ItemSku, o.Marketplace })
                    .Select(g => new OrderDto()
                    {

                        ItemSku = g.Key.ItemSku,
                        OrderId = g.First().OrderId,
                        ShipToName = g.First().ShipToName,
                        ShipToPhone = g.First().ShipToPhone,
                        ShipToAddress = g.First().ShipToAddress,
                        ShipToZip = g.First().ShipToZip,
                        MethodCode = g.First().MethodCode,
                        TrackingId = g.First().TrackingId,
                        OrderQuantity = g.Sum(x => x.OrderQuantity),
                        ReturnQuantity = g.Sum(x => x.ReturnQuantity),
                        CreatedDate = g.First().CreatedDate,
                        Marketplace = g.Key.Marketplace,
                    });
            }
            if (searchParams.isSortQuantity != null && searchParams.isSortQuantity == true)
            {
                order = order.OrderByDescending(x => x.OrderQuantity);
            }
            var product = _productRepository.FindAll(x => x.IsDelete != true);

            var data = order.Join(product, x => x.ItemSku,
                                 y => y.item_sku,
                                (x, y) => _mapper.Map<ProductDto>(y));

            bool isPagging = searchParams.isPagging ?? false;
            var result = await PaginationUtility<ProductDto>.CreateCustomAsync(data, pagination.PageNumber, pagination.PageSize, isPagging);
            return result;
        }
    }
}
