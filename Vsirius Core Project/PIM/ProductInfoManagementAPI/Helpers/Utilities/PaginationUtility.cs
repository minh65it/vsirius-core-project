using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System.Transactions;

namespace PimApi.Helpers.Utilities
{
    public class PaginationUtility<T> where T : class
    {
        public PaginationResult Pagination { get; set; }
        public List<T> Result { get; set; }

        public PaginationUtility(List<T> items, int count, int pageNumber, int pageSize, int skip)
        {
            Result = items;
            Pagination = PaginationResult.Create(count, pageNumber, pageSize, skip);
        }

        public static async Task<PaginationUtility<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize = 10, bool isPaging = true)
        {
            var count = await source.CountAsync();
            var skip = (pageNumber - 1) * pageSize;
            var items = isPaging ? await source.Skip(skip).Take(pageSize).ToListAsync() : await source.ToListAsync();

            return new PaginationUtility<T>(items, count, pageNumber, pageSize, skip);
        }
        public static async Task<PaginationUtility<T>> CreateAsync(IFindFluent<T, T> source, int pageNumber = 1, int pageSize = 100, bool isPaging = true)
        {
            var count = await source.CountDocumentsAsync();
            var skip = (pageNumber - 1) * pageSize;
            var items = isPaging ? await source.Skip(skip).Limit(pageSize).ToListAsync() : await source.ToListAsync();

            return new PaginationUtility<T>(items, (int)count, pageNumber, pageSize, skip);
        }
        public static async Task<PaginationUtility<T>> CreateNolockAsync(IQueryable<T> source, int pageNumber, int pageSize = 10, bool isPaging = true)
        {
            int count = 0;
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new System.Transactions.TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                count = await source.CountAsync();
                scope.Complete();
            }

            var skip = (pageNumber - 1) * pageSize;
            List<T> items = new();
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
            new System.Transactions.TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            },
            TransactionScopeAsyncFlowOption.Enabled))
            {
                items = isPaging ? await source.Skip(skip).Take(pageSize).ToListAsync() : await source.ToListAsync();
                scope.Complete();
            }
            return new PaginationUtility<T>(items, count, pageNumber, pageSize, skip);
        }

        public static PaginationUtility<T> Create(List<T> source, int pageNumber, int pageSize = 10, bool isPaging = true)
        {
            var count = source.Count();
            var skip = (pageNumber - 1) * pageSize;
            var items = isPaging ? source.Skip(skip).Take(pageSize).ToList() : source.ToList();

            return new PaginationUtility<T>(items, count, pageNumber, pageSize, skip);
        }

        public static async Task<PaginationUtility<T>> CreateCustomAsync(IQueryable<T> source, int pageNumber, int pageSize = 10, bool isPaging = true)
        {
            int count = 0;
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new System.Transactions.TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {
                count = await source.CountAsync();
                scope.Complete();
            }

            var skip = (pageNumber - 1) * pageSize;
            List<T> items = new();
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
            new System.Transactions.TransactionOptions()
            {
                IsolationLevel = IsolationLevel.ReadUncommitted
            },
            TransactionScopeAsyncFlowOption.Enabled))
            {
                items = isPaging ? await source.Skip(skip).Take(pageSize).ToListAsync() : await source.ToListAsync();
                scope.Complete();
            }
            return new PaginationUtility<T>(items, count, pageNumber, pageSize, skip);
        }

        public class PaginationResult
        {
            public int TotalCount { get; set; }
            public int TotalPage { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int Skip { get; set; }

            public PaginationResult(int count, int pageNumber, int pageSize, int skip)
            {
                TotalCount = count;
                TotalPage = (int)Math.Ceiling(TotalCount / (double)pageSize);
                PageNumber = pageNumber;
                PageSize = pageSize;
                Skip = skip;
            }

            public static PaginationResult Create(int count, int pageNumber, int pageSize, int skip)
            {
                return new PaginationResult(count, pageNumber, pageSize, skip);
            }
        }
    }

    public class PaginationParam
    {
        private const int MaxPageSize = 5000;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 20;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
    }
}
