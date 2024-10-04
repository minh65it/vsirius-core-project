using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Azure;
using Azure.Communication.Sms;

namespace OrdersAPI.Helpers.Utilities
{
    public interface IFunctionUtilities
    {
        Task<bool> SendSMSWithAzure(string ToPhoneNumber, string Message);
        Task<bool> SendSMSWithAWS(string ToPhoneNumber, string Message);
    }
    public class FunctionUtilities : IFunctionUtilities
    {
        //private readonly string _path;
        private readonly IConfiguration _configuration;

        public FunctionUtilities(IConfiguration configuration)
        {
            //_path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            _configuration = configuration;
        }

        public async Task<bool> SendSMSWithAWS(string ToPhoneNumber, string Message)
        {
            var credentials = new BasicAWSCredentials(_configuration["AmazonSMS:AccessKey"], _configuration["AmazonSMS:SecretKey"]);
            var snsClient = new AmazonSimpleNotificationServiceClient(credentials, RegionEndpoint.USWest2);
            var messageAttributes = new Dictionary<string, MessageAttributeValue>();
            var smsType = new MessageAttributeValue
            {
                DataType = "String",
                StringValue = "Transactional"
            };

            messageAttributes.Add("AWS.SNS.SMS.SMSType", smsType);

            var request = new PublishRequest
            {
                Message = Message,
                PhoneNumber = ToPhoneNumber,
                MessageAttributes = messageAttributes
            };

            try
            {
                var response = await snsClient.PublishAsync(request);
                Console.WriteLine("Message sent! Message ID: " + response.MessageId);
                if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendSMSWithAzure(string ToPhoneNumber, string Message)
        {
            string connectionString = _configuration["AzureSMS:Connectionstring"];

            SmsClient smsClient = new SmsClient(connectionString);
            Response<IReadOnlyList<SmsSendResult>> response = await smsClient.SendAsync(
                from: _configuration["AzureSMS:FromPhoneNumber"],
                to: new string[] { ToPhoneNumber },
                message: Message,
                options: new SmsSendOptions(enableDeliveryReport: true) 
                {
                });

            IEnumerable<SmsSendResult> results = response.Value;

            return results.Any(x => x.Successful == false) == false;
        }
    }
}
