using Newtonsoft.Json;
using OrderProcessApi._Services.IServices.Stuller;
using OrderProcessApi.Utilities;
using OrderProcessDomain.Stuller;
using System.Text;

namespace OrderProcessApi._Services.Services.Stuller
{
    public class SubmitOrderStullerService : ISubmitOrderStullerService
    {
        private readonly IConfiguration _configuration;

        private readonly string? _accountStuller = "";
        private readonly bool _testmode;
        private readonly string? _userStuller = "";
        private readonly string? _passStuller = "";

        public SubmitOrderStullerService(IConfiguration configuration)
        {
            _configuration = configuration;

            _accountStuller = _configuration.GetSection("Stuller:accountNumber").Value;
            _userStuller = _configuration.GetSection("Stuller:user").Value;
            _passStuller = _configuration.GetSection("Stuller:password").Value;
            _testmode = bool.Parse(_configuration.GetSection("Stuller:TestMode").Value ?? "true");
        }
        public async Task<bool> SubmitStuller(SubmitOrderParam param)
        {
            SubmitOrderDTO dto;

            dto = new SubmitOrderDTO();
            dto.OrderID = Guid.NewGuid().ToString();
            dto.Version = 1.0m;
            dto.Type = "3pl";
            dto.Account = _accountStuller;
            dto.TestMode = _testmode;
            dto.Payment = new Payment()
            {
                Type = "Terms"
            };
            dto.CustomerData = new CustomerData()
            {
                OrderNumber = param.SellerOrderID,
                OrderDate = DateTime.Now.Date,
            };
            dto.Contact = new Contact()
            {
                Name = param.ShipToName,
                Phone = param.ShipToPhone,
            };
            dto.ShipToAddress = new ShipToAddress()
            {
                ShipComplete = true,
                ShipMethodType = param.ShipMethodCode,
                RemovePricing = true,
                SignatureRequired = true,
                Address = new Address()
                {
                    Name = param.ShipToName,
                    AddressLine1 = param.ShipToAddress1,
                    AddressLine2 = param.ShipToAddress2,
                    City = param.ShipToCity,
                    State = param.ShipToState,
                    PostalCode = param.ShipToZip,
                    Country = param.ShipToCountryCode
                }
            };
            dto.BillToAddress = new BillToAddress()
            {
                SameAsShipTo = true
            };

            dto.Lines = new List<Line>();
            foreach (var product in param.Item)
            {
                dto.Lines.Add(new Line()
                {
                    Item = product.Item,
                    Description = product.Item,
                    Quantity = int.Parse(product.Qty),
                    Serial = new Serial()
                    {
                        Instructions = "",
                        Value = 0
                    }
                });
            }

            return await CallApiStullerSubmit(dto);
        }
        private async Task<bool> CallApiStullerSubmit(SubmitOrderDTO dto)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            string credentials =
                    Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userStuller}:{_passStuller}"));
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {credentials}");

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var apiUrl = _configuration.GetSection("Stuller:SubmitApi").Value;

            try
            {
                var response = await httpClient.PostAsync(apiUrl, content);

                var responseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    dynamic result = JsonConvert.DeserializeObject<dynamic>(responseContent);
                    if (result != null)
                    {
                        if ((bool)result.Created)
                        {
                            Common.SaveFileTxt($"Success:{JsonConvert.SerializeObject(dto)}. Err:{JsonConvert.SerializeObject(response)}",
                            $"SubmitStuller_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");

                        }
                        else
                        {
                            Common.SaveFileTxt($"Success:{JsonConvert.SerializeObject(dto)}. Err:{JsonConvert.SerializeObject(response)}",
                            $"SubmitStuller_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                        }
                        return (bool)result.Created;
                    }

                    return false;
                }
                else
                {
                    Common.SaveFileTxt($"Failed:{JsonConvert.SerializeObject(dto)}. Err:{JsonConvert.SerializeObject(response)}"
                        , $"SubmitStuller_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                }
            }
            catch (Exception ex)
            {
                Common.SaveFileTxt($"Failed:{JsonConvert.SerializeObject(dto)}. Err:{ex.Message}",
                    $"SubmitStuller_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return false;
            }
            finally
            {
                httpClient.Dispose();
            }

            return false;
        }
    }
}
