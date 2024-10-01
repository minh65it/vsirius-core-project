namespace VsiriusSystem._Services.Interfaces
{
    public interface ITextractAwsService
    {
        public string KeyorPath { get; set; }
        public string SecretKey { get; set; }
        Task<string> AmazoneScanForText(IFormFile file);
    }
}
