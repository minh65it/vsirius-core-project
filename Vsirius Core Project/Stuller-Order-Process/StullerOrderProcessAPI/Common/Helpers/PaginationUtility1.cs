namespace StullerOrderProcessAPI.Common.Helpers
{
    public class PaginationUtility1<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int PageCount { get; set; }
        public List<T> Data { get; set; } = new ();
    }
}
