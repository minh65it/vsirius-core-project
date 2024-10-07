namespace WalmartIntegrationDataApi.Common.Helpers
{
    public class PaginationUtil1<T> where T : class
    {
        public PaginationUtil1()
        {
            PageNumber = PageNumber == null ? 1 : PageNumber;
            PageSize = PageSize == null ? 25 : PageSize;
            TotalCount = TotalCount == null ? 0 : TotalCount;
            PageCount = PageCount == null ? 0 : PageCount;
            Data ??= new List<T>();
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<T> Data { get; set; }
    }
}
