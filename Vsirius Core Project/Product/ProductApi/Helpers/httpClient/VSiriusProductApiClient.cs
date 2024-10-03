using Newtonsoft.Json;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using System.Text;

namespace ProductApi.Helpers.httpClient
{
    public interface IVSiriusProductApiClient
    {
        public Task<PaginationUtility<VsiriusProductDto>> GetPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore);
        //public Task<bool> UpdateMarketplace(List<string> item_sku);
    }
    public class VSiriusProductApiClient : IVSiriusProductApiClient
    {
        private readonly IConfiguration _configuration;
        private string urlGet;
        //private string urlUpdateMartket;

        public VSiriusProductApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
            urlGet = _configuration.GetSection("PoductPIM:UrlGetAllData").Value!;
            //urlUpdateMartket = _configuration.GetSection("PoductPIM:UpdateMarketplace").Value!;
        }
        public async Task<PaginationUtility<VsiriusProductDto>> GetPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<VsiriusProductDto> result = new PaginationUtility<VsiriusProductDto>(null, 1, 1, 1, 1);
            string apiUrl = $"{urlGet}?PageNumber={pageNumber}&PageSize={pageSize}"; //All data
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    var httpRequestMessage = new HttpRequestMessage();
                    httpRequestMessage.RequestUri = new Uri(apiUrl);
                    httpRequestMessage.Method = HttpMethod.Post;
                    var jsonContent = new
                    {
                        //marketplace = 0 // marketplace = 0; Tức VSiriusWeb
                    };
                    httpRequestMessage.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(jsonContent), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PaginationUtility<VsiriusProductDto>>(json)!;
                    }
                    else
                    {
                        await Common.WriteLogAsync($"Call Api failed: {apiUrl} with Error internal error: {response.StatusCode}");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Call Api failed: {apiUrl} with exception {ex.Message}");
                return result;
            }
            finally
            {
                semaphore.Release();
            }

        }

        //public async Task<bool> UpdateMarketplace(List<string> item_sku)
        //{
        //    string apiUrl = $"{urlUpdateMartket}?martketplace=0&isValue=true"; //martketplace = 0 tức IsVSirius
        //    try
        //    {
        //        string accessToken = "";
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.Timeout = TimeSpan.FromMinutes(30);
        //            var httpRequestMessage = new HttpRequestMessage();
        //            httpRequestMessage.RequestUri = new Uri(apiUrl);
        //            httpRequestMessage.Method = HttpMethod.Put;
        //            httpRequestMessage.Headers.Add("Authorization", "Bearer " + accessToken);
        //            httpRequestMessage.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(item_sku), Encoding.UTF8, "application/json");
        //            HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                string json = await response.Content.ReadAsStringAsync();
        //                var result = JsonConvert.DeserializeObject<OperationResult>(json)!;
        //                return result.Success;
        //            }
        //            else
        //            {
        //                await Common.WriteLogAsync($"Call Api failed: {apiUrl} with Error internal error: {response.StatusCode}");
        //            }
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        await Common.WriteLogAsync($"Call Api failed: {apiUrl} with exception {ex.Message}");
        //        return false;
        //    }
        //}
    }
}
