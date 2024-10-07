//using OrderProcessApi.Utilities;
//using MongoDB.Bson;
//using MongoDB.Driver;
//using Microsoft.Extensions.Options;
//using OrderProcessApi.Data;
//using OrderProcessDomain.AmazonSeller;
//using OrderProcessApi.DTOs;
//using OrderProcessApi._Services.IServices.Amazon;

//namespace OrderProcessApi._Services.Services.Amazon
//{
//    public class AmazonSellerOrderService : IAmazonSellerOrderService
//    {
//        private readonly IMongoCollection<OrderAmazonSeller> _orderCollection;
//        public AmazonSellerOrderService(IOptions<MongoDbSettings> databaseSettings)
//        {
//            var mongoClient = new MongoClient(
//            databaseSettings.Value.ConnectionString);

//            var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.AmazonSellerDatabaseName);
//            _orderCollection = mongoDatabase.GetCollection<OrderAmazonSeller>(databaseSettings.Value.AmazonSellerCollectionName);
//        }

//        public async Task<bool> DeleteOrder(string id)
//        {
//            try
//            {
//                await _orderCollection.DeleteOneAsync(x => x.AmazonOrderId == id);
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }

//        public async Task<OrderAmazonSeller> DetailOrder(string id)
//        {
//            return await _orderCollection.Find(x => x.AmazonOrderId == id).FirstOrDefaultAsync();
//        }

//        public async Task<PaginationResult<OrderAmazonSeller>> GetOrder(OrderParams param, PaginationParam page)
//        {
//            var queryParameters = new List<DictionaryOption>();
//            var properties = typeof(OrderParams).GetProperties();

//            foreach (var property in properties)
//            {
//                var value = property.GetValue(param)?.ToString();
//                if (value != null)
//                {
//                    queryParameters.Add(new DictionaryOption()
//                    {
//                        Name = property.Name,
//                        Value = value
//                    });
//                }
//            }

//            var skip = (page.PageNumber - 1) * page.PageSize;

//            var filters = new List<FilterDefinition<OrderAmazonSeller>>();

//            if (queryParameters != null)
//                foreach (var parameter in queryParameters)
//                {
//                    filters.Add(Builders<OrderAmazonSeller>.Filter.Regex(parameter.Name, new BsonRegularExpression(parameter.Value, "i")));
//                }

//            var data = new List<OrderAmazonSeller>();
//            long totalCount;

//            if (!filters.Any())
//            {
//                data = await _orderCollection.Find(_ => true).Skip(skip).Limit(page.PageSize).ToListAsync();
//                totalCount = await _orderCollection.CountDocumentsAsync(_ => true);
//            }
//            else
//            {
//                data = await _orderCollection.Find(Builders<OrderAmazonSeller>.Filter.And(filters)).Skip(skip).Limit(page.PageSize).ToListAsync();
//                totalCount = await _orderCollection.CountDocumentsAsync(Builders<OrderAmazonSeller>.Filter.And(filters));
//            }

//            var pageCount = (int)Math.Ceiling((double)totalCount / page.PageSize);
//            return new PaginationResult<OrderAmazonSeller>
//            {
//                Data = data,
//                PageNumber = page.PageNumber,
//                PageSize = page.PageSize,
//                TotalCount = totalCount,
//                PageCount = pageCount
//            };
//        }

//        public async Task<bool> UpdateOrder(string id, OrderAmazonSeller model)
//        {
//            var checkOrder = await _orderCollection.Find(x => x.AmazonOrderId == id).FirstOrDefaultAsync();

//            if (checkOrder is null)
//            {
//                return false;
//            }

//            try
//            {
//                await _orderCollection.ReplaceOneAsync(x => x.AmazonOrderId == id, model);
//                return true;
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//    }
//}
