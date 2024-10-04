namespace AmazonIntegrationDataApi.Helpers.MongoDB
{
    public class MongoOperationResult
    {
        public string Caption { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public object Data { get; set; }
        public List<string> ValidateData { get; set; }

        public MongoOperationResult()
        {
            this.Data = null;
        }

        public MongoOperationResult(string message)
        {
            this.Message = message;
        }

        public MongoOperationResult(bool success)
        {
            this.Success = success;
        }

        public MongoOperationResult(bool success, string message)
        {
            this.Message = message;
            this.Success = success;
        }

        public MongoOperationResult(bool success, string message, string caption)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
        }

        public MongoOperationResult(bool success, string message, string caption, object data)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
            this.Data = data;
        }
        public MongoOperationResult(bool success, string message, string caption, List<string> validateData)
        {
            this.Success = success;
            this.Message = message;
            this.Caption = caption;
            this.ValidateData = validateData;
        }

    }
}
