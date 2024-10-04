using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductAiApi._JobServices.Interfaces;
using ProductAiApi._Repositories.Interfaces;
using ProductAiApi._Services.Interfaces;
using ProductAiApi.Common;
using ProductAiApi.DTOs;
using System.Text;

namespace ProductAiApi._JobServices.JobServices
{
    public class CreateProductAiFromPimService : ICreateProductAiFromPimService
    {
        private readonly IConfiguration _configuration;
        private readonly IProductAIRepository _repository;
        private readonly IProductAIService _productAIService;

        public CreateProductAiFromPimService(
            IConfiguration configuration,
            IProductAIRepository repository,
            IProductAIService productAIService)
        {
            _configuration = configuration;
            _repository = repository;
            _productAIService = productAIService;
        }

        public async Task CreateProductAIFromPim()
        {
            Util.WriteLog("-----------------Start CreateProductAIFromPim -----------------");
            try
            {
                int amountProductCreate = 400;
                Util.WriteLog($"Start Pull Top Product Data");
                List<PimProductDto> lstData = await PullTopProductFromPIMAsync();
                lstData = lstData
                    .GroupBy(item => item.item_sku)
                    .Select(group => group.First())
                    .ToList();
                Util.WriteLog($"End Pull Top Product Data, total list: {lstData.Count}");

                Util.WriteLog("Push Data Start Bulk insert update to local API Walmart");
                amountProductCreate = await CreateAndInsertToDb(lstData, amountProductCreate);
                Util.WriteLog("Push Data End Bulk insert update to local API Walmart");

                if (amountProductCreate > 0)
                {
                    Util.WriteLog($"Start Pull Top Product Data");
                    lstData = await PullAllProductFromPIMAsync();
                    lstData = lstData
                        .Where(item => double.Parse(item.standard_price) < 1000)
                        .OrderBy(item => item.standard_price)
                        .ToList();
                    Util.WriteLog($"End Pull Top Product Data, total list: {lstData.Count}");

                    Util.WriteLog("Push Data Start Bulk insert update to local API Walmart");
                    amountProductCreate = await CreateAndInsertToDb(lstData, amountProductCreate);
                    Util.WriteLog("Push Data End Bulk insert update to local API Walmart");
                }

                lstData.Clear();
            }
            catch (Exception ex)
            {
                Util.WriteLog("Process Mapper Error:" + ex.Message);
            }

            Util.WriteLog("-----------------End CreateProductAIFromPim------------------");
        }

        private async Task<List<PimProductDto>> PullAllProductFromPIMAsync()
        {
            try
            {
                List<PimProductDto> allResults = new List<PimProductDto>();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationResultFromAllPimProductAsync(1, pageSize);
                var totalPages = 0;
                List<PimProductDto> firstData = new List<PimProductDto>();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData);
                    Util.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtil<PimProductDto>>> tasks = new List<Task<PaginationUtil<PimProductDto>>>();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationResultFromAllPimProductAsync(pageNumber, pageSize));

                    if (amountRunTask < 3 && i != totalPages)
                    {
                        amountRunTask++;
                    }
                    else
                    {
                        amountRunTask = 0;
                        var results = await Task.WhenAll(tasks);
                        foreach (var result in results)
                        {
                            if (result != null)
                            {
                                allResults.AddRange(result.Result);
                            }
                            else
                            {
                                Util.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Pull Data Failed with exception: {ex.Message}");
                return null;
            }
        }

        private async Task<PaginationUtil<PimProductDto>> GetPaginationResultFromAllPimProductAsync(int pageNumber, int pageSize)
        {
            string url = _configuration["PimApi:GetAllProduct"]!.ToString();

            PaginationUtil<PimProductDto> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}&isPaging=true";

            HttpClient client = new HttpClient();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                string json = JsonConvert.SerializeObject(new {});
                var contentJson = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, contentJson);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtil<PimProductDto>>(content);
                    Util.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    Util.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        private async Task<int> CreateAndInsertToDb(List<PimProductDto> lstData, int amountProductCreate)
        {
            List<string> lstAllSku = await _repository.FindAll()
                .AsNoTracking().Select(x => x.SKU).ToListAsync();

            foreach (PimProductDto item in lstData)
            {
                if (!lstAllSku.Contains(item.item_sku))
                {
                    try
                    {
                        CreateUpdateProductAiDto dto = new()
                        {
                            Brand = item.brand_name,
                            SKU = item.item_sku,
                            Description = item.product_description,
                            ImgUrl = string.Empty
                        };
                        var rs = await _productAIService.CreateProduct(dto);
                        if (rs.Success)
                        {
                            amountProductCreate--;
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.WriteLog($"Create Sku {item.item_sku} failed: {ex.Message}");
                    }
                }
            }

            return amountProductCreate;
        }

        private async Task<List<PimProductDto>> PullTopProductFromPIMAsync()
        {
            try
            {
                List<PimProductDto> allResults = new List<PimProductDto>();
                var pageSize = 3000;
                var dataFirstCall = await GetPaginationResultFromTopProductAsync(1, pageSize);
                var totalPages = 0;
                List<PimProductDto> firstData = new List<PimProductDto>();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    allResults.AddRange(firstData);
                    Util.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtil<PimProductDto>>> tasks = new List<Task<PaginationUtil<PimProductDto>>>();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationResultFromTopProductAsync(pageNumber, pageSize));

                    if (amountRunTask < 3 && i != totalPages)
                    {
                        amountRunTask++;
                    }
                    else
                    {
                        amountRunTask = 0;
                        var results = await Task.WhenAll(tasks);
                        foreach (var result in results)
                        {
                            if (result != null)
                            {
                                allResults.AddRange(result.Result);
                            }
                            else
                            {
                                Util.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }

                return allResults;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Pull Data Failed with exception: {ex.Message}");
                return null;
            }
        }

        private async Task<PaginationUtil<PimProductDto>> GetPaginationResultFromTopProductAsync(int pageNumber, int pageSize)
        {
            string url = _configuration["PimApi:GetTopProduct"]!.ToString();

            PaginationUtil<PimProductDto> result = null;

            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}&isPaging=true";

            HttpClient client = new HttpClient();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtil<PimProductDto>>(json);
                    Util.WriteLog($"Call Api success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    Util.WriteLog($"Call Api failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Call Api failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }
    }
}
