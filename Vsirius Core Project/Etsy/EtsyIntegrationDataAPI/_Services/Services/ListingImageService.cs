using AutoMapper;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos.ProductEtsyDto;
using EtsyIntegrationDataAPI.Helpers.ListingsByShop;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI._Services.Services
{
    public class ListingImageService : IListingImageService
    {
        private ILoggerCustom _logService;
        private readonly IConfiguration _config;
        private readonly IAuthenticationEtsyService _authenticationEtsyService;
        private readonly IProductEtsyService _productEtsyService;
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly bool _isProduction;
        private readonly string _urlListing;
        private readonly string _xApiKey;
        private readonly string _contentType = "multipart/form-data";

        public ListingImageService(ILoggerCustom logService, IConfiguration config,
            IAuthenticationEtsyService authenticationEtsyService,
            IProductEtsyService productEtsyService,
            DataContext context, IMapper mapper)
        {
            _logService = logService;
            _config = config;
            _authenticationEtsyService = authenticationEtsyService;
            _productEtsyService = productEtsyService;
            _context = context;
            _isProduction = _config.GetSection("Etsy:Environment").Value == "Production" ? true : false;
            _urlListing = $"{_config.GetSection("Etsy:OpenAPI").Value}/shops/{_config.GetSection("Etsy:ShopId").Value}/listings";
            _xApiKey = $"{_config.GetSection("Etsy:KeysString").Value}";
            _contentType = "multipart/form-data";
            _mapper = mapper;
        }

        public async Task<List<ImageProductDto>> UploadListingImage(string authorization, long listing_id)
        {
            try
            {
                var dataProduct = await _context.ProductEtsy.FirstOrDefaultAsync(x => x.listing_id == listing_id);
                if (dataProduct == null)
                {
                    return null;
                }
                var dataImage = await _context.ImageProduct.Where(x => x.item_sku == dataProduct.item_sku).ToListAsync();
                List<ImageProductDto> result = new List<ImageProductDto>();
                foreach (var item in dataImage)
                {
                    var imgBytes = await UtilitiesListing.GetImageBytesFromUrlAsync(item.image);
                    //Call API
                    HttpClient client = new HttpClient();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(_contentType));
                    client.DefaultRequestHeaders.Add("x-api-key", _xApiKey);
                    client.DefaultRequestHeaders.Add("Authorization", authorization);
                    var content = new MultipartFormDataContent();
                    var imageContent = new ByteArrayContent(imgBytes);
                    content.Add(imageContent, "image", $"{item.item_sku}-{item.rank}.jpg");
                    content.Add(new StringContent("listing_image_id"), item.listing_image_id.ToString());
                    content.Add(new StringContent("rank"), item.rank.ToString());
                    content.Add(new StringContent("overwrite"), item.overwrite.ToString());
                    content.Add(new StringContent("is_watermarked"), item.is_watermarked.ToString());
                    content.Add(new StringContent("alt_text"), item.alt_text);
                    HttpResponseMessage response = await client.PostAsync($"{_urlListing}/{listing_id}/images", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        ImageProductDto res = JsonConvert.DeserializeObject<ImageProductDto>(responseBody)!;
                        result.Add(res);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Service: ListingImageService, Action: UploadListingImage", ex.ToString());
                return null;
            }
            
        }
    }
}
