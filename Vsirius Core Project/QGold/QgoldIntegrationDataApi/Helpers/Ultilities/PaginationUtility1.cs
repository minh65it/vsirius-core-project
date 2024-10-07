using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace QgoldIntegrationDataApi.Helpers.Ultilities
{
    public class PaginationUtility1<T> where T : class
    {
        public PaginationUtility1()
        {
        }
        public int PageNumber {  get; set; }
        public int PageSize {  get; set; }
        public int TotalCount {  get; set; }
        public int PageCount {  get; set; }
        public List<T> Data { get; set; }
    }
}
