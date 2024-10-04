using AmazonIntegrationDataApi._Repositories.Interfaces;
using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Dtos.MongoDB;
using AmazonIntegrationDataApi.Helpers.MongoDB;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.Utilities;
using AmazonIntegrationDataApi.Models;
using AutoMapper;
using FikaAmazonAPI;
using FikaAmazonAPI.ReportGeneration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace AmazonIntegrationDataApi._Services.Services
{
    public class ReturnOrderAmazon : IReturnOrderAmazon
    {
        private readonly IConfiguration _config;
        private readonly IAmazonMongoRepository _amazonMongoRepository;
        private string _amazonReturnOrderCollection = "";
        private readonly IMapper _mapper;

        public ReturnOrderAmazon(IConfiguration config,
            IAmazonMongoRepository amazonMongoRepository,
            IMapper mapper)
        {
            _config = config;
            _amazonMongoRepository = amazonMongoRepository;
            _amazonReturnOrderCollection = _config.GetSection("AmazonLocal:orderReturnCollection").Value!;
            _mapper = mapper;
        }

        public async Task<OperationResult> GetNewReturnOrders()
        {
            var amazonConnection = new AmazonConnection(new AmazonCredential()
            {
                AccessKey = _config.GetSection("FikaAmazonAPI:AccessKey").Value,
                SecretKey = _config.GetSection("FikaAmazonAPI:SecretKey").Value,
                RoleArn = _config.GetSection("FikaAmazonAPI:RoleArn").Value,
                ClientId = _config.GetSection("FikaAmazonAPI:ClientId").Value,
                ClientSecret = _config.GetSection("FikaAmazonAPI:ClientSecret").Value,
                RefreshToken = _config.GetSection("FikaAmazonAPI:RefreshToken").Value,
                MarketPlaceID = _config.GetSection("FikaAmazonAPI:MarketPlaceID").Value,
                SellerID = _config.GetSection("FikaAmazonAPI:SellerId").Value,
                IsDebugMode = true,
                //Environment = Environments.Sandbox
            });
            try
            {
                ReportManager reportManager = new ReportManager(amazonConnection);
                var returnMFNOrder = reportManager.GetReturnMFNOrder(10);
                if( returnMFNOrder != null ) 
                {
                    FilterDefinition<ReturnFBMOrderRow> filter = Builders<ReturnFBMOrderRow>.Filter.Where(x => returnMFNOrder.Select(i => i.OrderID).Contains(x.OrderID));
                    await _amazonMongoRepository.DeleteMany(_amazonReturnOrderCollection, filter);

                    await _amazonMongoRepository.InsertMany(returnMFNOrder, _amazonReturnOrderCollection);
                }
                return new OperationResult() {IsSuccess = true };
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Get Return Orders error: {ex.ToString()}", $"GetReturnOrders_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new OperationResult() { IsSuccess = false, Error = ex.ToString() };
            }
        }

        public async Task<PaginationResult<ReturnFBMOrderRowDto>> GetReturnOrders(Helpers.MongoDB.PaginationParam paginationParam, ReturnFBMOrderParams seachParam)
        {
            var result = new PaginationResult<ReturnFBMOrderRowDto>();

            FilterDefinition<ReturnFBMOrder>? filterOrder = null;
            if (seachParam != null)
            {
                if (!string.IsNullOrWhiteSpace(seachParam.OrderID))
                {
                    var filter = Builders<ReturnFBMOrder>.Filter.Where(x =>
                                        Regex.IsMatch(x.OrderID, seachParam.OrderID, RegexOptions.IgnoreCase));
                    Utilities2.AddAndFilter(ref filterOrder, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.AmazonRMAID))
                {
                    var filter = Builders<ReturnFBMOrder>.Filter.Where(x =>
                                        Regex.IsMatch(x.AmazonRMAID, seachParam.AmazonRMAID, RegexOptions.IgnoreCase));
                    Utilities2.AddAndFilter(ref filterOrder, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.ItemName))
                {
                    var filter = Builders<ReturnFBMOrder>.Filter.Where(x =>
                                        Regex.IsMatch(x.ItemName, seachParam.ItemName, RegexOptions.IgnoreCase));
                    Utilities2.AddAndFilter(ref filterOrder, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.TrackingID))
                {
                    var filter = Builders<ReturnFBMOrder>.Filter.Where(x =>
                                        Regex.IsMatch(x.TrackingID, seachParam.TrackingID, RegexOptions.IgnoreCase));
                    Utilities2.AddAndFilter(ref filterOrder, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.OrderDateFrom) && !string.IsNullOrWhiteSpace(seachParam.OrderDateTo))
                {
                    var fromDate = DateTime.Parse(seachParam.OrderDateFrom);
                    var toDate = DateTime.Parse(seachParam.OrderDateTo).AddDays(1);
                    var filter = Builders<ReturnFBMOrder>.Filter.And(
                        Builders<ReturnFBMOrder>.Filter.Gte("OrderDate",  fromDate),
                        Builders<ReturnFBMOrder>.Filter.Lte("OrderDate", toDate)
                    );
                    Utilities2.AddAndFilter(ref filterOrder, filter);
                }
                if (!string.IsNullOrWhiteSpace(seachParam.ReturnRequestDateFrom) && !string.IsNullOrWhiteSpace(seachParam.ReturnRequestDateTo))
                {
                    var fromDate = DateTime.Parse(seachParam.ReturnRequestDateFrom);
                    var toDate = DateTime.Parse(seachParam.ReturnRequestDateTo).AddDays(1);
                    var filter = Builders<ReturnFBMOrder>.Filter.And(
                        Builders<ReturnFBMOrder>.Filter.Gte("ReturnRequestDate", fromDate),
                        Builders<ReturnFBMOrder>.Filter.Lte("ReturnRequestDate", toDate)
                    );
                    Utilities2.AddAndFilter(ref filterOrder, filter);
                }
            }

            var res = new PaginationResult<ReturnFBMOrder>();
            var listDataMap = new List<ReturnFBMOrderRowDto>();
            var dataClaims = new List<ClaimDto>();

            if (seachParam?.IsClaim != null)
            {
                res = await _amazonMongoRepository.Find(_amazonReturnOrderCollection, filterOrder);
                listDataMap = _mapper.Map<List<ReturnFBMOrderRowDto>>(res.Data);
                dataClaims = await GetListClaim(listDataMap.Select(x => x.AmazonRMAID).ToList());

                foreach (var item in listDataMap)
                {
                    var dataClaim = dataClaims.Where(x => x.RMA == item.AmazonRMAID).FirstOrDefault();
                    if (dataClaim is not null)
                    {
                        item.DocumentType = dataClaim.DocumentType;
                        item.Marketplace = dataClaim.Marketplace;
                        item.MarketplaceOrderId = dataClaim.MarketplaceOrderId;
                        item.RMA = dataClaim.RMA;
                        item.Notes = dataClaim.Notes;
                        item.Images = dataClaim.Images;
                        item.ScanDate = dataClaim.ScanDate;
                    }
                }
                if (seachParam.IsClaim == true) listDataMap = listDataMap.Where(x => x.RMA != null).ToList();
                else listDataMap = listDataMap.Where(x => x.RMA == null).ToList();

                res.TotalCount = listDataMap.Count();
                res.PageCount = (int)Math.Ceiling(res.TotalCount / (double)paginationParam.PageSize);
                res.PageNumber = paginationParam.PageNumber;
                res.PageSize = paginationParam.PageSize;

                listDataMap = listDataMap.Skip((paginationParam.PageNumber - 1) * paginationParam.PageSize).Take(paginationParam.PageSize).ToList();
            }
            else
            {
                res = await _amazonMongoRepository.Find(_amazonReturnOrderCollection, filterOrder, paginationParam.PageNumber, paginationParam.PageSize);
                listDataMap =  _mapper.Map<List<ReturnFBMOrderRowDto>>(res.Data);
                dataClaims = await GetListClaim(listDataMap.Select(x => x.AmazonRMAID).ToList());

                foreach (var item in listDataMap)
                {
                    var dataClaim = dataClaims.Where(x => x.RMA == item.AmazonRMAID).FirstOrDefault();
                    if (dataClaim is not null)
                    {
                        item.DocumentType = dataClaim.DocumentType;
                        item.Marketplace = dataClaim.Marketplace;
                        item.MarketplaceOrderId = dataClaim.MarketplaceOrderId;
                        item.RMA = dataClaim.RMA;
                        item.Notes = dataClaim.Notes;
                        item.Images = dataClaim.Images;
                        item.ScanDate = dataClaim.ScanDate;
                    }
                }
            }


            result.Data = listDataMap;
            result.PageNumber = res.PageNumber;
            result.PageSize = res.PageSize;
            result.TotalCount = res.TotalCount;
            result.PageCount = res.PageCount;
            return result;
        }

        public async Task<ReturnFBMOrderRowDto> GetDetail(string amazonRMAID)
        {
            ReturnFBMOrderRowDto result = new ReturnFBMOrderRowDto() { };
            FilterDefinition<ReturnFBMOrder> filter = Builders<ReturnFBMOrder>.Filter.Eq(x => x.AmazonRMAID, amazonRMAID);

            var order = await _amazonMongoRepository.FisrtOrDefault(_amazonReturnOrderCollection, filter);
            result = _mapper.Map<ReturnFBMOrderRowDto>(order);
            if(order != null)
            {
                var dataClaim = await GetDetailClaim(order.AmazonRMAID);
                if(dataClaim != null)
                {
                    result.DocumentType = dataClaim.DocumentType;
                    result.Marketplace = dataClaim.Marketplace;
                    result.MarketplaceOrderId = dataClaim.MarketplaceOrderId;
                    result.RMA = dataClaim.RMA;
                    result.Notes = dataClaim.Notes;
                    result.Images = dataClaim.Images;
                    result.ScanDate = dataClaim.ScanDate;
                }
                else
                {
                    result.ScanDate = null;
                }
            }
            return result;
        }
        public async Task<ClaimDto> GetDetailClaim(string id)
        {
            try
            {
                ClaimDto? result = new ClaimDto() { };
                string token = await LoginIdentity();
                if (token == null)
                    return result;

                // call api package
                //string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcwNjk1Mjk4MiwiaXNzIjoiaHR0cHM6Ly92c2lyaXVzLmNvbS8iLCJhdWQiOiJodHRwczovL3ZzaXJpdXMuY29tLyJ9.oHIjDQHMzPjY4hqkctjCwY_TIsxA_OIeMPdjXrqQi-o";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var response = await httpClient.GetAsync($"{_config.GetValue<string>("PackageAPI:URL")}?id={id}");

                var responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent != null)
                    result = JsonConvert.DeserializeObject<ClaimDto>(responseContent);
                return result;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Get Detail Claim Error: {ex.ToString()}", $"GetDetailClaim_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return null;
            }
            
        }
        public async Task<List<ClaimDto>> GetListClaim(List<string> rma)
        {
            try
            {
                string ids = string.Join(",", rma);

                List<ClaimDto>? result = new List<ClaimDto>() { };
                //string token = await LoginIdentity();
                //if (token == null)
                //    return result;

                // call api package
                string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcwNjk1Mjk4MiwiaXNzIjoiaHR0cHM6Ly92c2lyaXVzLmNvbS8iLCJhdWQiOiJodHRwczovL3ZzaXJpdXMuY29tLyJ9.oHIjDQHMzPjY4hqkctjCwY_TIsxA_OIeMPdjXrqQi-o";
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                httpClient.DefaultRequestHeaders.Add("rma", ids);

                var response = await httpClient.GetAsync($"{_config.GetValue<string>("PackageAPI:URLGetAllByRMA")}");

                var responseContent = await response.Content.ReadAsStringAsync();
                if (responseContent != null)
                    result = JsonConvert.DeserializeObject<List<ClaimDto>>(responseContent);
                return result;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Get List Claim Error: {ex.ToString()}", $"GetListClaim_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return null;
            }

        }
        public async Task<string> LoginIdentity()
        {
            try
            {
                string urlLogin = _config.GetValue<string>("UserVsirius:URL")!;
                string email = _config.GetValue<string>("UserVsirius:user")!;
                string password = _config.GetValue<string>("UserVsirius:PassWord")!;

                using (var httpClient = new HttpClient())
                {
                    var parameters = new Dictionary<string, string>
                    {
                        { "email", email },
                        { "password", password }
                    };

                    // Content post
                    var json = JsonConvert.SerializeObject(parameters);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    // Send request to urlLogin
                    var response = await httpClient.PostAsync(urlLogin, content);

                    // Get token from urlLogin
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        dynamic token = JsonConvert.DeserializeObject(result);
                        return token?.token;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Login Identity Error: {ex.ToString()}", $"LoginIdentity_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return null;
            }
            
        }
    }
}
