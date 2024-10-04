using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using OrdersAPI._Services.Interfaces;
using OrdersAPI.Dtos;

namespace OrdersAPI._Services.Services
{
    public class SendMailService : ISendMailService
    {
        private readonly IConfiguration _configuration;
        private readonly string? _accessKey = "";
        private readonly string? _secretKey = "";
        private readonly string _cc = "";

        public SendMailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _accessKey = _configuration.GetSection("AWS:AccessKey").Value;
            _secretKey = _configuration.GetSection("AWS:SecretKey").Value;
            _cc = _configuration.GetSection("Email:CC").Value!;
        }

        public async Task<Result> SendToMultipleEmail(string from, List<string> to, string subject, string body)
        {
            using (var client = new AmazonSimpleEmailServiceClient(_accessKey, _secretKey, RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = from,
                    Destination = new Destination
                    {
                        ToAddresses = to
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = body
                            }
                            //Text = new Content
                            //{
                            //    Charset = "UTF-8",
                            //    Data = body
                            //}
                        }
                    },

                };
                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new Result("Send success");
                    }
                    else
                    {
                        return new Result("Send failed", false);
                    }

                }
                catch (Exception ex)
                {
                    return new Result(ex.Message, false);
                }
            }

        }

        public async Task<Result> SendToSingleEmail(string from, string to, string subject, string body)
        {
            var list = new List<string> { to };
            return await SendToMultipleEmail(from, list, subject, body);
        }
        public async Task<Result> SendToMultipleEmailCC(string from, List<string> to, List<string>? cc, string subject, string body)
        {
            using (var client = new AmazonSimpleEmailServiceClient(_accessKey, _secretKey, RegionEndpoint.USEast1))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = from,
                    Destination = new Destination
                    {
                        ToAddresses = to,
                        CcAddresses = cc == null ? new List<string>(_cc.Split(",")) : cc,
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = body
                            }
                            //Text = new Content
                            //{
                            //    Charset = "UTF-8",
                            //    Data = body
                            //}
                        }
                    },

                };
                try
                {
                    var response = await client.SendEmailAsync(sendRequest);
                    if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new Result("Send success");
                    }
                    else
                    {
                        return new Result("Send failed", false);
                    }

                }
                catch (Exception ex)
                {
                    return new Result(ex.Message, false);
                }
            }
        }

        public async Task<Result> SendToSingleEmailCC(string from, string to, List<string>? cc, string subject, string body)
        {
            var list = new List<string> { to };
            return await SendToMultipleEmailCC(from, list, cc, subject, body);
        }
    }
}
