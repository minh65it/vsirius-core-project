namespace VsiriusSystem.Helpers.Utilities
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
            this.Data = null;
        }

        public OperationResult(string message)
        {
            this.Message = message;
        }

        public OperationResult(bool success)
        {
            this.Success = success;
        }

        public OperationResult(bool success, string message)
        {
            this.Message = message;
            this.Success = success;
        }

        public OperationResult(bool success, string message, string caption)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
        }

        public OperationResult(bool success, string message, string caption, object data)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
            this.Data = data;
        }
        public OperationResult(bool success, string message, string caption, List<string> validateData)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
            this.ValidateData = validateData;
        }

    }
}
