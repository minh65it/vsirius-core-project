using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi._Services.IServices.Qgold;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using OrderProcessApi.Utilities.Mapping;
using OrderProcessDomain.Qgold;

namespace OrderProcessApi._Services.Services.Qgold
{
    public class QAService : IQAService
    {
        IQgoldRepository _qgoldDB;
        private readonly IConfiguration _configuration;
        private readonly ISendMailService _iEmailHandler;
        private readonly IMapper _mapper;
        private readonly string _qAColleciton = "";

        public QAService(
            IQgoldRepository qgoldDB,
            IConfiguration configuration,
            ISendMailService iEmailHandler,
            IMapper mapper)
        {
            _qgoldDB = qgoldDB;
            _configuration = configuration;
            _iEmailHandler = iEmailHandler;
            _qAColleciton = _configuration.GetSection("Database:QgoldQA").Value!;
            _mapper = mapper;
        }

        public async Task<OperationResult> SendMailQuestion(QgoldQADTO question)
        {
            try
            {
                if (question.Description == null)
                {
                    return new OperationResult() { Success = false, Message = "Description is blank" };
                }
                var dataAdd = _mapper.Map<QgoldQA>(question);
                dataAdd.SKU = Util.RemoveFirstChar(question.SKU);
                dataAdd._id = Guid.NewGuid().ToString();
                dataAdd.Date = DateTime.Now;
                var _from = _configuration.GetSection("Email:From").Value!;
                var _to = _configuration.GetSection("Email:To").Value!;

                var subject = $"Customer question regarding item {dataAdd.SKU}";
                var body = "Hi, <br /><br />"
                            + $"Customer asked: \"{dataAdd.Description}\" <br /> <br />"
                            + "Please advise."
                            + "<br /> <br /> Thank you";


                var result = await _iEmailHandler.SendToSingleEmail(_from, _to, subject, body);
                if (result.IsSuccess)
                {
                    await _qgoldDB.InsertOne(dataAdd, _qAColleciton);
                    return new OperationResult() { Success = true, Message = "Send mail question succeeded" };
                }
                else
                {
                    return new OperationResult() { Success = false, Message = "Send mail question error" };
                }
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Send Mail Question error: {ex.ToString()}", $"SendMailQuestion_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new OperationResult() { Success = false, Message = "System send mail error" };
            }
        }

        public async Task<PaginationResult<QgoldQADTO>> GetQuestion(PaginationParam paginationParam, SearchQgoldQAParam seachParam)
        {
            var result = new PaginationResult<QgoldQADTO>();

            FilterDefinition<QgoldQA>? filters = null;

            if (seachParam != null)
            {
                
                if (!string.IsNullOrWhiteSpace(seachParam.Title))
                {
                    var filter = Builders<QgoldQA>.Filter.
                        Regex("Title", new BsonRegularExpression(seachParam.Title));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.Description))
                {
                    var filter = Builders<QgoldQA>.Filter.
                        Regex("Description", new BsonRegularExpression(seachParam.Description));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.SKU))
                {
                    var filter = Builders<QgoldQA>.Filter.
                        Regex("SKU", new BsonRegularExpression(seachParam.SKU));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.CustomerName))
                {
                    var filter = Builders<QgoldQA>.Filter.
                        Regex("CustomerName", new BsonRegularExpression(seachParam.CustomerName));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.CustomerEmail))
                {
                    var filter = Builders<QgoldQA>.Filter.
                        Regex("CustomerEmail", new BsonRegularExpression(seachParam.CustomerEmail));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.Marketplace))
                {
                    var filter = Builders<QgoldQA>.Filter.
                        Regex("Marketplace", new BsonRegularExpression(seachParam.Marketplace));
                    Common.AddAndFilter(ref filters, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.MarketOrderId))
                {
                    var filter = Builders<QgoldQA>.Filter.
                        Regex("MarketOrderId", new BsonRegularExpression(seachParam.MarketOrderId));
                    Common.AddAndFilter(ref filters, filter);
                }

            }
            var data = await _qgoldDB.Find(_qAColleciton, filters, paginationParam.PageNumber, paginationParam.PageSize);
            result.PageNumber = data.PageNumber;
            result.PageSize = data.PageSize;
            result.TotalCount = data.TotalCount;
            result.PageCount = data.PageCount;
            result.Data = _mapper.Map<List<QgoldQADTO>>(data.Data);

            return result;
        }

        public async Task<QgoldQADTO> DetailQuestion(string id)
        {
            QgoldQADTO result = new QgoldQADTO();
            FilterDefinition<QgoldQA> filter = Builders<QgoldQA>.Filter.Eq(x => x._id, id);

            var data = await _qgoldDB.FisrtOrDefault(_qAColleciton, filter);
            if (data != null)
            {
                result = _mapper.Map<QgoldQADTO>(data);
            }
            return result;
        }
    }
}
