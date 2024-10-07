using AutoMapper;
using MongoDB.Driver;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.Data;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using OrderProcessDomain.Stuller;
using System.Text.RegularExpressions;

namespace OrderProcessApi._Services.Services.Stuller
{
    public class StullerProcessService : IStullerProcessService
    {
        IStullerRepository _stullerDB;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public StullerProcessService(IStullerRepository StullerDB, IMapper mapper, IConfiguration config)
        {
            _stullerDB = StullerDB;
            _mapper = mapper;
            _config = config;
        }

        public Task<Result> DeleteOrders(List<string> ds)
        {
            throw new NotImplementedException();
        }

        public async Task<StullerOrderDTO?> GetDetailOrder(string orderNumber)
        {
            StullerOrderDTO result = new StullerOrderDTO();
            var filter = Builders<OrderStuller>.Filter.Where(x =>
                                    Regex.IsMatch(x.OrderNumber, orderNumber, RegexOptions.IgnoreCase));
            var res = await _stullerDB.FisrtOrDefault(_config.GetSection("Database:StullerInvoice").Value!, filter);
            result = _mapper.Map<StullerOrderDTO>(res);
            return result;
        }

        public async Task<PaginationResult<StullerOrderDTO>> GetOrders(PaginationParam paginationParam, SearchStullerProcessParam seachParam)
        {
            var result = new PaginationResult<StullerOrderDTO>();

            FilterDefinition<OrderStuller>? filterStuller = null;
            if (!string.IsNullOrWhiteSpace(seachParam.OrderStullerId))
            {
                var filter = Builders<OrderStuller>.Filter.Where(x =>
                                    Regex.IsMatch(x.OrderNumber, seachParam.OrderStullerId, RegexOptions.IgnoreCase));
                Common.AddAndFilter(ref filterStuller, filter);

            }
            if (!string.IsNullOrWhiteSpace(seachParam.TrackingNumber))
            {
                var filter = Builders<OrderStuller>.Filter.Where(x =>
                                    Regex.IsMatch(x.TrackingNumber, seachParam.TrackingNumber, RegexOptions.IgnoreCase));
                Common.AddAndFilter(ref filterStuller, filter);
            }
            if (!string.IsNullOrWhiteSpace(seachParam.FromInvoiceDate) && !string.IsNullOrWhiteSpace(seachParam.ToInvoiceDate))
            {
                var fromDate = DateTime.Parse(seachParam.FromInvoiceDate);
                var toDate = DateTime.Parse(seachParam.ToInvoiceDate).AddDays(1);
                var filter = Builders<OrderStuller>.Filter.And(
                    Builders<OrderStuller>.Filter.Gte("InvoiceDate", fromDate),
                    Builders<OrderStuller>.Filter.Lte("InvoiceDate", toDate)
                );
                Common.AddAndFilter(ref filterStuller, filter);
            }
            var res = await _stullerDB.Find(_config.GetSection("Database:StullerInvoice").Value!, filterStuller, paginationParam.PageNumber, paginationParam.PageSize);
            var data = _mapper.Map<List<StullerOrderDTO>>(res.Data);
            result.Data = data;
            result.PageNumber = res.PageNumber;
            result.PageSize = res.PageSize;
            result.TotalCount = res.TotalCount;
            result.PageCount = res.PageCount;
            return result;
        }
    }
}
