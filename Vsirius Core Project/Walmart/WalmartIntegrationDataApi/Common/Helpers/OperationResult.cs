namespace WalmartIntegrationDataApi.Common.Helpers
{
    public class OperationResult
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { set; get; } = true;
        public string Error { set; get; } = string.Empty;
        public object Data { set; get; } = null!;
    }
}
