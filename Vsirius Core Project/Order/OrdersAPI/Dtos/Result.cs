namespace OrdersAPI.Dtos
{
    public class Result
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public Result(bool isSuccess = true)
        {
            IsSuccess = isSuccess;
        }
        public Result(string message, bool isSuccess = true)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
