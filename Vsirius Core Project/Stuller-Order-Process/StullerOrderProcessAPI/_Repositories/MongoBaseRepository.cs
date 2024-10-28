using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson;
using MongoDB.Driver;
using StullerOrderProcessAPI.Common.Helpers;
using System.Linq.Expressions;

namespace StullerOrderProcessAPI._Repositories
{
    public class MongoBaseRepository : IMongoBaseRepository
    {
        private MongoClient? _dbClient { get; set; }
        private readonly IConfiguration _configuration;
        readonly int _bufferSize = 20000;
        public MongoBaseRepository(string dbName, IConfiguration configuration)
        {
            _configuration = configuration;
            if (string.IsNullOrEmpty(dbName))
                throw new ArgumentException(null, nameof(dbName));
            _dbClient = new MongoClient(_configuration["Connectionstrings:MongoDB"]);
            database = _dbClient.GetDatabase(dbName);
            _configuration = configuration;
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

        public async Task<PaginationUtility<T>> Find<T>(string collectionName,
            FilterDefinition<T>? filter = null,
            int page = 1,
            int PageSize = int.MaxValue,
            SortDefinition<T>? sort = null) where T : class
        {
            filter ??= Builders<T>.Filter.Where(p => true);
            await CreateCollection(collectionName);
            var collection = database.GetCollection<T>(collectionName);

            var data = collection
                .Find(filter)
                .Sort(sort);

            return await PaginationUtility<T>.CreateAsync(data, page, PageSize);
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

        public async Task<T?> FisrtOrDefault<T>(string collectionName, FilterDefinition<T>? filter = null, SortDefinition<T>? sort = null)
        {
            filter ??= Builders<T>.Filter.Where(p => true);
            await CreateCollection(collectionName);
            var collection = database.GetCollection<T>(collectionName);

            var data = await collection
                .Find(filter)
                .Sort(sort).FirstOrDefaultAsync();
            return data;
        }

        public async Task<List<T>> FindAll<T>(string collectionName, FilterDefinition<T>? filter = null)
        {
            filter ??= Builders<T>.Filter.Where(p => true);
            await CreateCollection(collectionName);
            var collection = database.GetCollection<T>(collectionName);

            return await collection.Find(filter).ToListAsync();
        }
    }
}
