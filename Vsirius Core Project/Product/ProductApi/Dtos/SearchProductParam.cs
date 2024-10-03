namespace ProductApi.Dtos
{
    public class SearchProductParam
    {
        public string? Keyword {  get; set; }
        public decimal? MinPrice {  get; set; }
        public decimal? MaxPrice {  get; set; }
        public bool? IncludeNoStock { get; set; }
    }

}
