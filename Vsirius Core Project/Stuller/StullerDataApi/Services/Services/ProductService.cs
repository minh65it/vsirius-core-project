using StullerDataApi.IRepository;
using StullerApiClient.Models;
using StullerDataApi.Utilities;
using StullerDataApi.Params;
using StullerDataApi.Services.IServices;

namespace StullerDataApi.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> DeleteProduct(string SKU)
        {
            try
            {
                var dataDelete = await _productRepository.GetById(SKU);
                await _productRepository.Remove(SKU);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public async Task<GetProductsResponse> GetProduct(ProductsRequestPostFilter filter)
        //{
        //    try
        //    {

        //        string _username = "ha.vsirius.inc";
        //        string _password = "Welcome123$1";
        //        string _baseUrl = "https://www.stuller.com/api/v2/";

        //        string url = _baseUrl + "products";

        //        using var httpClient = new HttpClient();


        //        var httpRequestMessage = new HttpRequestMessage();
        //        httpRequestMessage.Method = HttpMethod.Post;
        //        httpRequestMessage.RequestUri = new Uri(url);

        //        var authValue = new AuthenticationHeaderValue(
        //            "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}")));
        //        httpRequestMessage.Headers.Authorization = authValue;

        //        //Add Body params
        //        var json = JsonSerializer.Serialize(filter);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        httpRequestMessage.Content = content;

        //        //string jsoncontent = "{ \"page\": \"1\", \"pageSize\": \"10\" }";
        //        //var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
        //        //httpRequestMessage.Content = httpContent;

        //        // Action Post
        //        var response = await httpClient.SendAsync(httpRequestMessage);
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        var result = JsonSerializer.Deserialize<GetProductsResponse>(responseContent);
        //        return result;
        //    }
        //    catch (Exception e)
        //    {

        //        throw;
        //    }
        //}

        public async Task<Product> DetailProduct(string SKU)
        {

            var a = await _productRepository.GetById(SKU);
            return a;
        }

        public async Task<bool> UpdateProduct(string SKU, Product model)
        {
            try
            {
                var dataUpdate = await _productRepository.GetById(SKU);
                if (dataUpdate == null)
                {
                    return false;
                }
                model.SKU = SKU;
                await _productRepository.Update(SKU, model);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<PaginationResult<Product>> Filter(ProductParams param, PaginationParam page)
        {
            var queryParameters = new List<DictionaryOption>();
            var properties = typeof(ProductParams).GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(param)?.ToString();
                if (value != null)
                {

                    if (property.Name != "ProductType" && property.Name != "ShortDescription")
                    {
                        queryParameters.Add(new DictionaryOption()
                        {
                            Name = "DescriptiveElementGroup.DescriptiveElements.Value",
                            Value = value
                        });
                    }
                    else
                    {
                        queryParameters.Add(new DictionaryOption()
                        {
                            Name = property.Name,
                            Value = value
                        });
                    }
                }
            }
            var result = await _productRepository.Filter(queryParameters, page);
            return result;
        }
    }
}
