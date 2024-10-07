using MongoDB.Bson;
using MongoDB.Driver;
using StullerDataApi.Data;
using StullerDataApi.IRepository;
using StullerDataApi.Utilities;

namespace StullerDataApi.Repository
{
    public class MongoDBRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly IMongoDatabase Database;
        private readonly IMongoCollection<TEntity> DbSet;

        public MongoDBRepository(IMongoContext context)
        {
            Database = context.Database;
            DbSet = Database.GetCollection<TEntity>(typeof(TEntity).Name.ToLower());
        }

        public  async Task<TEntity> Add(TEntity obj)
        {
            await DbSet.InsertOneAsync(obj);
            return obj;
        }

        public  async Task<TEntity> GetById(string id)
        {
            var data = await DbSet.Find(FilterId(id)).FirstOrDefaultAsync();
            return data;
        }

        public  async Task<IEnumerable<TEntity>> GetAll()
        {
            var all = await DbSet.FindAsync(Builders<TEntity>.Filter.Empty);
            return all.ToList();
        }

        public async  Task<TEntity> Update(string id, TEntity obj)
        {
            await DbSet.ReplaceOneAsync(FilterId(id), obj);
            return obj;
        }

        public async  Task<bool> Remove(string id)
        {
            var result = await DbSet.DeleteOneAsync(FilterId(id));
            return result.IsAcknowledged;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        private static FilterDefinition<TEntity> FilterId(string key)
        {
            var a = Builders<TEntity>.Filter.Eq("_id", key);
            return a;
        }

        public async Task<PaginationResult<TEntity>> Filter(List<DictionaryOption> queryParameters, PaginationParam page)
        {
            var skip = (page.PageNumber - 1) * page.PageSize;

            var filters = new List<FilterDefinition<TEntity>>();

            foreach (var parameter in queryParameters)
            {
                filters.Add(Builders<TEntity>.Filter.Regex(parameter.Name, new BsonRegularExpression(parameter.Value, "i")));
            }

            var data = new List<TEntity>();
            long totalCount;

            if (!filters.Any())
            {
                data = await DbSet.Find(_ => true).Skip(skip).Limit(page.PageSize).ToListAsync();
                totalCount = await DbSet.CountDocumentsAsync(_ => true);
            }
            else
            {
                data = await DbSet.Find(Builders<TEntity>.Filter.And(filters)).Skip(skip).Limit(page.PageSize).ToListAsync();
                totalCount = await DbSet.CountDocumentsAsync(Builders<TEntity>.Filter.And(filters));
            }



            var pageCount = (int)Math.Ceiling((double)totalCount / page.PageSize);

            return new PaginationResult<TEntity>
            {
                Data = data,
                PageNumber = page.PageNumber,
                PageSize = page.PageSize,
                TotalCount = totalCount,
                PageCount = pageCount
            };
        }
    }

}
