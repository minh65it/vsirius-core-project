using StullerDataApi.Utilities;

namespace StullerDataApi.IRepository
{
    public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        Task<TEntity> Add(TEntity obj);
        Task<TEntity> GetById(string id);
        Task<IEnumerable<TEntity>> GetAll();
        Task<PaginationResult<TEntity>> Filter(List<DictionaryOption> queryParameters, PaginationParam page);
        Task<TEntity> Update(string id, TEntity obj);
        Task<bool> Remove(string id);
    }
}
