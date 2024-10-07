using AutoMapper;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.AmazonSeller;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi._Services.Services.Qgold
{
    public class ReSubmitOrderService : IReSubmitOrderService
    {
        IQgoldRepository _qgoldDB;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly string _reOrderColleciton = "";

        public ReSubmitOrderService(IQgoldRepository qgoldDB, 
            IConfiguration configuration, 
            IMapper mapper)
        {
            _qgoldDB = qgoldDB;
            _configuration = configuration;
            _mapper = mapper;
            _reOrderColleciton = _configuration.GetSection("Database:QgoldReSubmitOrder").Value!;
        }

        //public async Task<OperationResult> AddReSubmitOrder(OrderMarketplaceDto returnOrder)
        //{
        //    try
        //    {
        //        if (returnOrder == null)
        //        {
        //            return new OperationResult { Success = false, Message = "Data is null" };
        //        }
        //        returnOrder._id = null;
        //        var dataAdd = _mapper.Map<OrderMarketplace>(returnOrder);
        //        await _qgoldDB.InsertOne(dataAdd, _reOrderColleciton);
        //        return new OperationResult { Success = true, Message = "AddReSubmitOrder done" };
        //    }
        //    catch (Exception ex)
        //    {
        //        await Common.WriteLogAsync($"AddReSubmitOrder error: {ex.Message}", $"AddReSubmitOrder_Error{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
        //        return new OperationResult { Success = false, Message = $"Save AddReSubmitOrder to database is faild with: {ex.ToString()}" };
        //    }
            
        //}

        public async Task<OperationResult> AddReSubmitOrder(List<QgoldFtpOrderObject> models)
        {
            try
            {
                var dataAdd = _mapper.Map<List<OrderMarketplace>>(models);
                await _qgoldDB.InsertMany(dataAdd, _reOrderColleciton);
                return new OperationResult { Success = true, Message = "AddReSubmitOrder done" };
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"AddReSubmitOrder error: {ex.Message}", $"AddReSubmitOrder_Error{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new OperationResult { Success = false, Message = $"Save AddReSubmitOrder to database is faild with: {ex.ToString()}" };
            }
        }
    }
}
