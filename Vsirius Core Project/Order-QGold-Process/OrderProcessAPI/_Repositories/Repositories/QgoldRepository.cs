using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using OrderProcessApi._Repositories.IRepositories;
using OrderProcessApi.DTOs;
using OrderProcessApi.Utilities;
using System.Linq.Expressions;

namespace OrderProcessApi._Repositories.Repositories
{
    public class QgoldRepository : IQgoldRepository
    {
        private readonly string _conn;
        private MongoClient? _dbClient { get; set; }
        readonly int _bufferSize;
        private readonly IConfiguration _configuration;

        public QgoldRepository(string? conn = null, string dbName = "Qgold", int bufferSize = 20000, IConfiguration configuration = null)
        {
            _configuration = configuration;
            _conn = conn ?? _configuration.GetSection("ConnectionStrings:MongoDB").Value!;
            _dbClient = new(_conn);
            _bufferSize = bufferSize;
            database = _dbClient.GetDatabase(dbName);
        }
        public IMongoDatabase database { get; set; }

        public async Task<bool> Any<T>(string collectionName, FilterDefinition<T>? filter = null)
        {
            await CreateCollection(collectionName);

            filter ??= Builders<T>.Filter.Where(p => true);

            var collection = database.GetCollection<T>(collectionName);
            return await collection.Find(filter).AnyAsync();
        }

        public async Task<bool> CollectionExist(string collectionName)
        {
            var filter = new BsonDocument("name", collectionName);
            var options = new ListCollectionNamesOptions { Filter = filter };

            return await database.ListCollectionNames(options).AnyAsync();
        }

        public async Task<long> Count<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = database.GetCollection<T>(collectionName);
            return await collection.CountDocumentsAsync(filter);
        }

        public async Task CreateCollection(string collectionName)
        {
            if (!await CollectionExist(collectionName))
            {
                await database.CreateCollectionAsync(collectionName);
            }
        }

        public async Task DeleteMany<T>(string collectionName, FilterDefinition<T> filter)
        {
            var collection = database.GetCollection<T>(collectionName);
            await collection.DeleteManyAsync(filter);
        }

        public async Task DropCollection(string CollectionName)
        {
            if (await CollectionExist(CollectionName))
                await database.CreateCollectionAsync(CollectionName);
        }

        public async Task EmptyCollection(string CollectionName)
        {
            await database.DropCollectionAsync(CollectionName);
            await database.CreateCollectionAsync(CollectionName);
        }

        public async Task<PaginationResult<T>> Find<T>(string collectionName,
            FilterDefinition<T>? filter = null,
            int page = 1,
            int PageSize = int.MaxValue,
            SortDefinition<T>? sort = null)
        {
            filter ??= Builders<T>.Filter.Where(p => true);
            await CreateCollection(collectionName);
            var collection = database.GetCollection<T>(collectionName);

            var paginationResult = new PaginationResult<T>
            {
                PageNumber = page,
                PageSize = PageSize,
                TotalCount = await Count(collectionName, filter),
                Data = await collection
                .Find(filter)
                .Sort(sort)
                .Skip((page - 1) * PageSize)
                .Limit(PageSize)
                .ToListAsync()
            };
            paginationResult.PageCount = (int)Math.Ceiling(paginationResult.TotalCount / (double)paginationResult.PageSize);

            return paginationResult;
        }

        public async Task InsertOne<T>(T o, string collectionName)
        {
            await CreateCollection(collectionName);

            var collection = database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(o);
        }

        public async Task InsertMany<T>(IEnumerable<T> list, string collectionName)
        {
            if (list == null || list.Count() == 0) return;

            await CreateCollection(collectionName);

            var collection = database.GetCollection<T>(collectionName);
            ConventionRegistry.Register("IgnoreIfDefault", new ConventionPack
            {
                new IgnoreIfDefaultConvention(true)
            }, t => true);
            if (list.Count() <= _bufferSize)
            {
                await collection.InsertManyAsync(list);
                return;
            }

            var lists = Split(list, 2);

            foreach (var l in lists)
            {
                await InsertMany(l, collectionName);
            }

        }

        public async Task RenameCollection(string OldName, string NewName)
        {
            if (await CollectionExist(OldName))
                await database.RenameCollectionAsync(OldName, NewName);
            else await CreateCollection(NewName);
        }

        public async Task ReplaceOne<T>(string collectionName, T source, FilterDefinition<T>? filter = null)
        {
            await CreateCollection(collectionName);
            filter ??= Builders<T>.Filter.Where(p => true);
            var collection = database.GetCollection<T>(collectionName);

            await DeleteMany(collectionName, filter);
            await InsertOne(source, collectionName);
        }

        public async Task UpdateMany<T>(string collectionName, Expression<Func<T, bool>> filter, UpdateDefinition<T> definition, UpdateOptions? options = null)
        {
            await CreateCollection(collectionName);
            var collection = database.GetCollection<T>(collectionName);

            await collection.UpdateManyAsync<T>(filter, definition, options);
        }

        private IEnumerable<IEnumerable<T>> Split<T>(IEnumerable<T> list, int parts)
        {
            int i = 0;
            var splits = from item in list
                         group item by i++ % parts into part
                         select part.AsEnumerable();
            return splits;
        }

        public async Task<T?> FisrtOrDefault<T>(string collectionName, FilterDefinition<T>? filter = null, int page = 1, int PageSize = int.MaxValue, SortDefinition<T>? sort = null)
        {
            return (await Find(collectionName, filter, page, PageSize, sort)).Data.FirstOrDefault();
        }
    }
}
