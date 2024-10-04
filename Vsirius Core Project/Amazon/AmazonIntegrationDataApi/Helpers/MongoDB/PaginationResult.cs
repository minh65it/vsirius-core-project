namespace AmazonIntegrationDataApi.Helpers.MongoDB
{
    public class PaginationResult<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalCount { get; set; }
        public int PageCount { get; set; }
    }
    public class PaginationParam
    {
        private const int MaxPageSize = 5000;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 25;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; }
        }
    }
}
