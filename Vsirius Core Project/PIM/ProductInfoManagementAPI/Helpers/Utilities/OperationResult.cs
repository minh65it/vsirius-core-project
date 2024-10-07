namespace PimApi.Helpers.Utilities
{
    public class OperationResult
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
        public List<string> ValidateData { get; set; }

        public OperationResult()
        {
            Data = null;
        }

        public OperationResult(string message)
        {
            Message = message;
        }

        public OperationResult(bool success)
        {
            Success = success;
        }

        public OperationResult(bool success, string message)
        {
            Message = message;
            Success = success;
        }

        public OperationResult(bool success, string message, string caption)
        {
            Success = success;
            Message = message;
            Caption = caption;
        }

        public OperationResult(bool success, string message, string caption, object data)
        {
            Success = success;
            Message = message;
            Caption = caption;
            Data = data;
        }
        public OperationResult(bool success, string message, string caption, List<string> validateData)
        {
            Success = success;
            Message = message;
            Caption = caption;
            ValidateData = validateData;
        }

    }
}
