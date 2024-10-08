namespace EbayCoreApi.Dtos
{
    public class SearchParam
    {
        /// <summary>
        /// có thể tìm kiếm nhiều kết quả bằng list ID theo format: id1|id2|id3|id4
        /// </summary>
        public string Ids { get; set; }
        public string KeySearch { get; set; }
    }

    public class SearchOrderParam
    {
        /// <summary>
        /// có thể tìm kiếm nhiều kết quả bằng list ID theo format: id1|id2|id3|id4
        /// </summary>
        public string ids { get; set; }
        public string supplier { get; set; }
        public string sKU { get; set; }
        public string buyerName { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public bool? submited { get; set; }
    }
}