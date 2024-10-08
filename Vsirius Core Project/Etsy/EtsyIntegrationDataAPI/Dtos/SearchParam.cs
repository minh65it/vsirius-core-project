namespace EtsyIntegrationDataAPI.Dtos
{
    public class SearchParam
    {
        /// <summary>
        /// có thể tìm kiếm nhiều kết quả bằng list ID theo format: id1|id2|id3|id4
        /// </summary>
        public long Ids { get; set; }
        public string? KeySearch { get; set; }
    }

    public class SearchOrderParam
    {
        public string? ids { get; set; }
        public string? name { get; set; }
        public string? address { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string? buyer_email { get; set; }
        public bool? is_paid { get; set; }
        public bool? is_shipped { get; set; }
    }
}
