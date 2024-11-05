using AutoMapper;
using Newtonsoft.Json;
using ProductApi.Helpers.Utilities;

namespace ProductApi.Helpers.httpClient
{
    public interface IProductAIClient
    {
        public Task<List<ProductAiDto>> GetProductAI();
    }
    public class ProductAIClient : IProductAIClient

    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private string urlGet;
        private readonly ProductAiApi._Services.Interfaces.IProductAIService _service;

        public ProductAIClient(IConfiguration configuration, ProductAiApi._Services.Interfaces.IProductAIService service, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
            urlGet = _configuration.GetSection("ProductAiApi:UrlGetAllData").Value!;
            _service = service;
        }

        public async Task<List<ProductAiDto>> GetProductAI()
        {
            var lstAllSkuProductAI = new List<ProductAiDto>();

            try
            {
                var pageSize = 500;
                PaginationUtility<ProductAiDto> dataFirstCall = await GetPaginationResult_RFT(1, pageSize);
                var totalPages = 0;
                List<ProductAiDto> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    lstAllSkuProductAI.AddRange(firstData!);
                    await Common.WriteLogAsync($"Total pages: {totalPages}");
                }
                else
                {
                    return null;
                }

                //List rest data
                List<Task<PaginationUtility<ProductAiDto>>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationResult_RFT(pageNumber, pageSize));

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
                                lstAllSkuProductAI.AddRange(result.Result);
                            }
                            else
                            {
                                await Common.WriteLogAsync($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync("Get All ProductAI failed. Err:" + ex.Message);
            }

            return lstAllSkuProductAI;
        }
        public async Task<PaginationUtility<ProductAiDto>> GetPaginationResult(int pageNumber, int pageSize)
        {

            PaginationUtility<ProductAiDto> result = null;

            string apiUrl = $"{urlGet}?PageNumber={pageNumber}&PageSize={pageSize}&isReview=true"; // isReview=true để lấy ra các content đã review

            HttpClient client = new();

            try
            {
                client.Timeout = TimeSpan.FromMinutes(60);
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<PaginationUtility<ProductAiDto>>(json);
                    await Common.WriteLogAsync($"Call API ProductAI success: {apiUrl} at page {pageNumber}");
                }
                else
                {
                    await Common.WriteLogAsync($"Call API ProductAI failed: {apiUrl} with error internal error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Call API ProductAI failed: {apiUrl} with exception {ex.Message} at page {pageNumber}");
            }

            return result;
        }

        public async Task<PaginationUtility<ProductAiDto>> GetPaginationResult_RFT(int pageNumber, int pageSize)
        {
            try
            {
                ProductAiApi.Common.PaginationParam pagination = new ProductAiApi.Common.PaginationParam();
                pagination.PageNumber = pageNumber;
                pagination.PageSize = pageSize;
                var data = await _service.GetAll(pagination, null, null);
                List<ProductAiDto> list = new List<ProductAiDto>();
                foreach (var item in data.Result)
                {
                    ProductAiDto dto = _mapper.Map<ProductAiDto>(item);
                    list.Add(dto);
                }
                PaginationUtility<ProductAiDto> result = new PaginationUtility<ProductAiDto>(
                    list,
                    data.Pagination.TotalCount,
                    data.Pagination.PageNumber,
                    data.Pagination.PageSize,
                    data.Pagination.Skip);
                await Common.WriteLogAsync($"Call service ProductAI success at page {pageNumber}");
                return result;
            }
            catch (Exception ex)
            {
                await Common.WriteLogAsync($"Call service ProductAI failed with exception {ex.Message} at page {pageNumber}");
                return new PaginationUtility<ProductAiDto>();
            }
            
        }

    }
    public class ProductAiDto
    {
        public string SKU { get; set; }
        public string ProductName { get; set; }
        public string Brand { get; set; }
        public string? BulletPoint1 { get; set; }
        public string? BulletPoint2 { get; set; }
        public string? BulletPoint3 { get; set; }
        public string? BulletPoint4 { get; set; }
        public string? BulletPoint5 { get; set; }
        public string? BulletPoint6 { get; set; }
        public string? BulletPoint7 { get; set; }
        public string? BulletPoint8 { get; set; }
        public string? BulletPoint9 { get; set; }
        public string? Keywords { get; set; }
        public string? Note { get; set; }
        public bool IsReview { get; set; }
    }
}
