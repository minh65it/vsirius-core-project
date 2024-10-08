using System.Collections.Generic;

namespace EbayApi.Utils
{
    public class PaginationDto<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
    }
}