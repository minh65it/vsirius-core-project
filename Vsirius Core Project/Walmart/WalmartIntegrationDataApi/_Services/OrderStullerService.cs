using MongoDB.Bson;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using WalmartIntegrationDataApi.Common.Helpers;
using StullerOrderProcessAPI.Common.Helpers;
using WalmartIntegrationDataApi._Repositories.Interfaces;
using StullerOrderProcessAPI.Common;
using WalmartIntegrationDataApi.DtosOrderStuller;
using WalmartIntegrationDataApi._Services.Interfaces;


namespace WalmartIntegrationDataApi._Services
{
    public class OrderStullerService : IOrderStullerService
    {
        private readonly IConfiguration _configuration;
        private readonly IStullerOrderRepository _repository;

        public OrderStullerService(IConfiguration configuration, IStullerOrderRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<PaginationUtility<StullerOrderDTO>> GetOrders( PaginationParam paginationParam, SearchOrderStullerParam searchParam)
        {
            FilterDefinition<StullerOrder>? filter = null;
            if (searchParam != null)
            {
                if (!string.IsNullOrEmpty(searchParam.Status))
                {
                    Utils2.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.Status == searchParam.Status)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.Marketplace))
                {
                    Utils2.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.Marketplace == searchParam.Marketplace)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.MarketplaceOrderId))
                {
                    Utils2.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.MarketplaceOrderId == searchParam.MarketplaceOrderId)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.OrderID))
                {
                    Utils2.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.OrderID == searchParam.OrderID)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.BuyerName))
                {
                    Utils2.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => x.BuyerName == searchParam.BuyerName)
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.SKU))
                {
                    Utils2.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Regex("Items.ProductSku",
                        new BsonRegularExpression(new Regex(Regex.Escape(searchParam.SKU), RegexOptions.IgnoreCase)))
                        );
                }

                if (!string.IsNullOrEmpty(searchParam.FromDate) || !string.IsNullOrEmpty(searchParam.ToDate))
                {
                    var fromDate = DateTime.Parse(string.IsNullOrEmpty(searchParam.FromDate) ? DateTime.Now.AddDays(-30).ToString() : searchParam.FromDate);
                    var toDate = DateTime.Parse(string.IsNullOrEmpty(searchParam.ToDate) ? DateTime.Now.ToString() : searchParam.ToDate);

                    List<string> lstDate = GetTimeSegment(fromDate, toDate);

                    Utils2.AddAndFilter(ref filter,
                        Builders<StullerOrder>.Filter.Where(x => lstDate.Contains(x.OrderDate))
                        );
                }

            }

            SortDefinition<StullerOrder>? sort = Builders<StullerOrder>.Sort.Descending(x => x.UpdatedDate);

            var entityPagination = await _repository.Find(
                NameCollection.OrderStuller,
                filter,
                paginationParam.PageNumber,
                paginationParam.PageSize,
                sort);

            var result = new PaginationUtility<StullerOrderDTO>()
            {
                Pagination = new PaginationUtility<StullerOrderDTO>.PaginationResult()
                {
                    PageSize = paginationParam.PageSize,
                    PageNumber = paginationParam.PageNumber,
                    Skip = entityPagination.Pagination.Skip,
                    TotalCount = entityPagination.Pagination.TotalCount,
                    TotalPage = entityPagination.Pagination.TotalPage
                },
                Result = entityPagination.Result.Select(CustomMapper.ConvertOrderEntityToDto).ToList()
            };

            return result;
        }

        private List<string> GetTimeSegment(DateTime fromDate, DateTime toDate)
        {
            var result = new List<string>();
            for (DateTime i = fromDate; i <= toDate; i = i.AddDays(1))
            {
                result.Add(i.ToString("M/d/yyyy"));
            }
            return result;
        }
    }
}
