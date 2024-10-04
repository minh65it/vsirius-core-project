
using System.Linq.Expressions;

namespace AmazonIntegrationDataApi._Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> FindById(object id);

        Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void AddMultiple(List<T> entities);

        void Update(T entity);

        void UpdateMultiple(List<T> entities);

        void Remove(T entity);

        void Remove(object id);

        void RemoveMultiple(List<T> entities);

        bool Any(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        T FirstOrDefault(params Expression<Func<T, object>>[] includeProperties);

        T FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T> FirstOrDefaultAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        //bool BulkInsert(List<T> list);
    }
}