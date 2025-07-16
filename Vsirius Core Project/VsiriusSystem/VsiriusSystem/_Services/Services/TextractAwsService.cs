using Amazon;
using Amazon.Runtime;
using Amazon.Textract;
using Amazon.Textract.Model;
using System.Text;
using VsiriusSystem._Services.Interfaces;
using VsiriusSystem.Helpers.Utilities;

namespace VsiriusSystem._Services.Services
{
    public class TextractAWSService : ITextractAwsService
    {
        public TextractAWSService() { }

        public string KeyorPath { get; set; } = "REMOVED_AWS_KEY";
        public string SecretKey { get; set; } = "0WkCMked5LQGCZAU58X66T/9B4+6nnxdzJc6MeKj";

        public async Task<string> AmazoneScanForText(IFormFile file)
        {
            try
            {
                var credentials = new BasicAWSCredentials(KeyorPath, SecretKey);
                StringBuilder sb = new ();

                MemoryStream stream = Common.ReadFile(file);
                byte[] byteArray = stream.ToArray();

                AmazonTextractClient textract = new AmazonTextractClient(credentials, RegionEndpoint.USEast1);
                DetectDocumentTextRequest request = new DetectDocumentTextRequest
                {
                    Document = new Document
                    {
                        Bytes = new MemoryStream(byteArray)
                    }
                };
                DetectDocumentTextResponse response = await textract.DetectDocumentTextAsync(request);

                foreach (var block in response.Blocks)
                {
                    if (block.BlockType == BlockType.LINE)
                    {
                        sb.AppendLine(block.Text);
                    }
                }

                return sb.ToString();
            }
            catch (Exception e) { return string.Empty; }
        }
    }
}
