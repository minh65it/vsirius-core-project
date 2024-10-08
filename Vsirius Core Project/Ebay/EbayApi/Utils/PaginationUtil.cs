using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
namespace EbayApi.Utils
{
    public class PaginationUtil<T> where T : class
    {
        public PaginationUtil()
        {
            Pagination = Pagination ?? new PaginationResult();
            Result = Result ?? new List<T>();
        }

        public PaginationResult Pagination { get; set; }
        public List<T> Result { get; set; }

        public PaginationUtil(PaginationResult Pagination, List<T> Result)
        {
            this.Pagination = Pagination;
            this.Result = Result;
        }

        public PaginationUtil(List<T> items, long count, int pageNumber, int pageSize, int skip)
        {
            Result = items;
            Pagination = PaginationResult.Create(count, pageNumber, pageSize, skip);
        }

        public static async Task<PaginationUtil<T>> CreateAsync(IQueryable<T> source, int pageNumber = 1, int pageSize = 100, bool isPaging = true)
        {
            var count = await source.CountAsync();
            var skip = (pageNumber - 1) * pageSize;
            var items = isPaging ? await source.OrderBy(i => 0).Skip(skip).Take(pageSize).ToListAsync() : await source.ToListAsync();

            return new PaginationUtil<T>(items, count, pageNumber, pageSize, skip);
        }



        public static PaginationUtil<T> Create(List<T> source, int pageNumber = 1, int pageSize = 100, bool isPaging = true)
        {
            var count = source.Count();
            var skip = (pageNumber - 1) * pageSize;
            var items = isPaging ? source.OrderBy(i => 0).Skip(skip).Take(pageSize).ToList() : source.ToList();

            return new PaginationUtil<T>(items, count, pageNumber, pageSize, skip);
        }

        public class PaginationResult
        {
            public long TotalCount { get; set; }
            public int TotalPage { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int Skip { get; set; }
            public PaginationResult()
            {
            }
            public PaginationResult(long count, int pageNumber, int pageSize, int skip)
            {
                TotalCount = count;
                TotalPage = (int)Math.Ceiling(TotalCount / (double)pageSize);
                PageNumber = pageNumber;
                PageSize = pageSize;
                Skip = skip;
            }
            public static PaginationResult Create(long count, int pageNumber, int pageSize, int skip)
            {
                return new PaginationResult(count, pageNumber, pageSize, skip);
            }
        }

    }
    public class PaginationParam
    {
        /// <summary>
        /// Số trang
        /// </summary>
        public int PageNumber { get; set; }
        /// <summary>
        /// Số lượng kết quả
        /// </summary>
        public int PageSize { get; set; }
    }
}