namespace EbayApi.Dtos
{
    public class SearchParam
    {
        /// <summary>
        /// có thể tìm kiếm nhiều kết quả bằng list ID theo format: id1|id2|id3|id4
        /// </summary>
        public string Ids { get; set; }
        public string KeySearch { get; set; }
    }
}