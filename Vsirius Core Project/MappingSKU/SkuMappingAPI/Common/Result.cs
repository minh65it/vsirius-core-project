namespace SkuMappingAPI.Common
{
    public class Result
    {
        public string? Error { set; get; }
        public bool IsSuccess { set; get; }
        public object? Data { set; get; }
    }
}
