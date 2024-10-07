using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi.Dtos.Return
{
    public class DataReturnDTO
    {
        public Meta meta { get; set; }
        public List<ReturnOrder> returnOrders { get; set; }
    }

    public class Meta
    {
        public int totalCount { get; set; }
        public int limit { get; set; }
        public string nextCursor { get; set; }
    }
}
