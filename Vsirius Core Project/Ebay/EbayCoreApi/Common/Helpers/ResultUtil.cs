namespace EbayCoreApi.Common.Helpers
{
    public class ResultUtil<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<KeyValuePair<string, object>> Validation { get; set; }
        public ResultUtil() { }
        public ResultUtil(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
        public ResultUtil(bool isSuccess, string msg)
        {
            IsSuccess = isSuccess;
            Message = msg;
        }
        public ResultUtil(bool isSuccess, T Data)
        {
            IsSuccess = isSuccess;
            this.Data = Data;
        }
        public ResultUtil(bool isSuccess, string msg, T Data)
        {
            IsSuccess = isSuccess;
            Message = msg;
            this.Data = Data;
        }
        public ResultUtil(bool isSuccess, string msg, T Data, List<KeyValuePair<string, object>> Validation)
        {
            IsSuccess = isSuccess;
            Message = msg;
            this.Data = Data;
            this.Validation = Validation;
        }
    }
}
