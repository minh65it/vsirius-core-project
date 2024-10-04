using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using Newtonsoft.Json;
using System.Text;

namespace AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB
{
    public interface IVSiriusProductApiClient
    {
        public Task<PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>> GetPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore);
    }
    public class VSiriusProductApiClient : IVSiriusProductApiClient
    {
        private readonly IConfiguration _configuration;
        private string url;

        public VSiriusProductApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
            url = _configuration.GetSection("PoductPIM:UrlGetAllData").Value!;
        }
        public async Task<PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>> GetPaginationResult(int pageNumber, int pageSize, SemaphoreSlim semaphore)
        {
            await semaphore.WaitAsync();
            PaginationUtility<AmazonJewelryDataFeedItemV3_Dto> result = new PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>(null, 1, 1, 1, 1);
            //string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}&keyword=S-81724:248018:P"; // search keyword
            string apiUrl = $"{url}?PageNumber={pageNumber}&PageSize={pageSize}"; //All data
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromMinutes(30);
                    var jsonContent = new
                    {
                        //marketplace = 2 // marketplace = 2; Tức Amazon
                    };
                    HttpContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(jsonContent), Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>>(json)!;
                    }
                    else
                    {
                        await Utilities2.WriteLogAsync($"Call Api failed: {apiUrl} with Error internal error: {response.StatusCode}");
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Call Api failed: {apiUrl} with exception {ex.Message}");
                return result;
            }
            finally
            {
                semaphore.Release();
            }

        }
    }
}
