using System.Linq.Expressions;
using AmazonIntegrationDataApi.Data;
using Microsoft.EntityFrameworkCore;

namespace AmazonIntegrationDataApi._Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DBContext _context;

        public Repository(DBContext context)
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Add(entity);
        }

        public void AddMultiple(List<T> entities)
        {
            _context.AddRange(entities);
        }

        public IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties)
        {
            return QueryableEntity(includeProperties);
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return QueryableEntity(includeProperties).Where(predicate);
        }

        public async Task<T> FindById(object id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).FirstOrDefaultAsync(predicate);
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Remove(object id)
        {
            Remove(FindById(id));
        }

        public void RemoveMultiple(List<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

        public void UpdateMultiple(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
        }

        public bool Any(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return QueryableEntity(includeProperties).Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await QueryableEntity(includeProperties).AnyAsync(predicate);
        }

        public T FirstOrDefault(params Expression<Func<T, object>>[] includeProperties)
        {
            return QueryableEntity(includeProperties).FirstOrDefault();
        }

        public T FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return QueryableEntity(includeProperties).FirstOrDefault(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return await QueryableEntity(includeProperties).FirstOrDefaultAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await QueryableEntity(includeProperties).FirstOrDefaultAsync(predicate);
        }

        private IQueryable<T> QueryableEntity(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> items = _context.Set<T>();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        //public bool BulkInsert(List<T> list)
        //{
        //    try
        //    {
        //        _context.BulkInsert(list);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;

        //    }

        //}
    }
}