using AmazonIntegrationDataApi.Helpers.MongoDB;
using MongoDB.Driver;

namespace AmazonIntegrationDataApi._Repositories
{
    public interface ICoreMongoDB
    {
        public IMongoDatabase database { get; set; }
        Task<bool> CollectionExist(string collectionName);
        Task CreateCollection(string collectionName);
        Task DropCollection(string CollectionName);
        Task RenameCollection(string OldName, string NewName);
        Task EmptyCollection(string CollectionName);
        Task DeleteMany<T>(string collectionName, FilterDefinition<T> filter);
        Task UpdateMany<T>(string collectionName, System.Linq.Expressions.Expression<Func<T, bool>> filter, UpdateDefinition<T> definition, UpdateOptions? options = null);
        Task ReplaceOne<T>(string collectionName, T source, FilterDefinition<T>? filter = null);
        Task<bool> Any<T>(string collectionName, FilterDefinition<T>? filter = null);
        Task<PaginationResult<T>> Find<T>(string collectionName, FilterDefinition<T>? filter = null, int page = 1, int PageSize = int.MaxValue, SortDefinition<T>? sort = null);
        Task<long> Count<T>(string collectionName, FilterDefinition<T> filter);
        Task InsertOne<T>(T o, string collectionName);
        Task InsertMany<T>(IEnumerable<T> list, string collectionName);
        Task<T?> FisrtOrDefault<T>(string collectionName, FilterDefinition<T>? filter = null, int page = 1, int PageSize = int.MaxValue, SortDefinition<T>? sort = null);
    }
}
