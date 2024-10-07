using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace WalmartIntegrationDataApi.Common.Helpers
{
    public class PaginationUtility<T> where T : class
    {
        public PaginationUtility()
        {
            Pagination ??= new PaginationResult();
            Result ??= new List<T>();
        }

        public PaginationResult Pagination { get; set; }
        public List<T> Result { get; set; }

        public PaginationUtility(PaginationResult Pagination, List<T> Result)
        {
            this.Pagination = Pagination;
            this.Result = Result;
        }

        public PaginationUtility(List<T> items, long count, int pageNumber, int pageSize, int skip)
        {
            Result = items;
            Pagination = PaginationResult.Create(count, pageNumber, pageSize, skip);
        }

        public static async Task<PaginationUtility<T>> CreateAsync(IQueryable<T> source, int pageNumber = 1, int pageSize = 100, bool isPaging = true)
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

            return new PaginationUtility<T>(items, count, pageNumber, pageSize, skip);
        }

        public static PaginationUtility<T> Create(List<T> source, int pageNumber = 1, int pageSize = 100, bool isPaging = true)
        {
            var count = source.Count();
            var skip = (pageNumber - 1) * pageSize;
            var items = isPaging ? source.Skip(skip).Take(pageSize).ToList() : source.ToList();



            return new PaginationUtility<T>(items, count, pageNumber, pageSize, skip);
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
        /// Từ khoá tìm kiếm
        /// </summary>
        public string? KeySearch { get; set; } = string.Empty;
        /// <summary>
        /// Số trang
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Số lượng kết quả
        /// </summary>
        public int PageSize { get; set; } = 25;
        /// <summary>
        /// true => lấy ra sản phẩm mới update hôm nay
        /// false => lấy ra sản phẩm mới
        /// null => tất cả sản phẩm
        /// </summary>
        public bool? IsUpdate { get; set; }
        /// <summary>
        /// S-: stuller ; Q-: Qgold 
        /// </summary>
        public string? BrandProduct { get; set; }
    }
}
