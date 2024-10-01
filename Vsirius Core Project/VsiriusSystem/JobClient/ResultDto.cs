namespace QuartzInfrastructure
{
    internal class ResultDto<T>
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> ValidateData { get; set; }

        public ResultDto()
        {
            this.Data = default;
        }

        public ResultDto(string message)
        {
            this.Message = message;
        }

        public ResultDto(bool success)
        {
            this.Success = success;
        }

        public ResultDto(bool success, string message)
        {
            this.Message = message;
            this.Success = success;
        }

        public ResultDto(bool success, string message, string caption)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
        }

        public ResultDto(bool success, string message, string caption, T data)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
            this.Data = data;
        }
        public ResultDto(bool success, string message, string caption, List<string> validateData)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
            this.ValidateData = validateData;
        }
    }
}
