using AutoMapper;
using MongoDB.Driver;
using PackageApi._Repositories.IRepositories;
using PackageApi._Services.IServices;
using PackageApi.CollectionMongo;
using PackageApi.Dtos;
using PackageApi.Dtos.Claim;
using PackageApi.Models;
using PackageApi.Utilities;
using static PackageApi.Utilities.Blob;

namespace PackageApi._Services.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IClaimRepository _claimRepository;
        private readonly IAzureStorage _azureStorage;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string? _collectionName = "";
        private readonly VsiriusSystem._Services.Interfaces.ITextractAwsService _extractAwsService;

        public ClaimService(IClaimRepository claimRepository, IMapper mapper, IConfiguration configuration, IAzureStorage azureStorage, VsiriusSystem._Services.Interfaces.ITextractAwsService extractAwsService)
        {
            _claimRepository = claimRepository;
            _mapper = mapper;
            _configuration = configuration;
            _collectionName = _configuration.GetSection("Connectionstrings:ClaimCollection").Value;
            _azureStorage = azureStorage;
            _extractAwsService = extractAwsService;
        }

        public async Task<List<ClaimDTO>> GetAllByRMA(List<string> rma)
        {
            FilterDefinition<Claim> filter = Builders<Claim>.Filter.Where(x => rma.Contains(x.RMA));
            var res = await _claimRepository.Find(CollectionName.PackageClaimName, filter);
            var result = _mapper.Map<List<ClaimDTO>>(res.Data);
            return result;
        }

        public async Task<PaginationResult<ClaimDTO>> GetClaim(PaginationParam paginationParam, ClaimSearchParam search)
        {
            FilterDefinition<Claim>? filter = null;

            if (search != null)
            {
                if (!string.IsNullOrEmpty(search.RMA))
                {
                    PackageCommon.AddAndFilter(ref filter, Builders<Claim>.Filter.Eq(x => x.RMA, search.RMA));
                }
                if (!string.IsNullOrEmpty(search.MarketplaceOrderId))
                {
                    PackageCommon.AddAndFilter(ref filter, Builders<Claim>.Filter.Eq(x => x.MarketplaceOrderId, search.MarketplaceOrderId));
                }
                if (!string.IsNullOrEmpty(search.Marketplace))
                {
                    PackageCommon.AddAndFilter(ref filter, Builders<Claim>.Filter.Eq(x => x.Marketplace, search.Marketplace));
                }
                if (search.DocumentType != null)
                {
                    PackageCommon.AddAndFilter(ref filter, Builders<Claim>.Filter.Eq(x => x.DocumentType, search.DocumentType.ToString()));
                }
            }

            var sort = Builders<Claim>.Sort.Descending(x => x.ScanDate);

            var rsDB = await _claimRepository.Find(CollectionName.PackageClaimName, filter,
                paginationParam.PageNumber, paginationParam.PageSize, sort);

            return new PaginationResult<ClaimDTO>()
            {
                PageSize = paginationParam.PageSize,
                PageNumber = paginationParam.PageNumber,
                PageCount = rsDB.PageCount,
                TotalCount = rsDB.TotalCount,
                Data = _mapper.Map<List<ClaimDTO>>(rsDB.Data)
            };
        }

        public async Task<ClaimDTO?> GetDetail(string id)
        {
            var detail = await _claimRepository.FisrtOrDefault(CollectionName.PackageClaimName, Builders<Claim>.Filter.Eq(x => x.RMA, id));
            return _mapper.Map<ClaimDTO>(detail);
        }

        public List<KeyValuePair<int, string>> GetDocTypes()
        {
            var listDocType = Enum.GetValues(typeof(DocumentType)).Cast<DocumentType>()
               .Select(x => new KeyValuePair<int, string>(key: (int)x, value: x.ToString())).ToList();
            return listDocType;
        }

        public async Task<OperationResult> Submit(ClaimParam model)
        {
            try
            {
                if (model.DocumentType == null)
                    return new OperationResult() { Success = false, Message = "Error created, DocumentType is not null", };
                //var dataAdd = _mapper.Map<Claim>(model);
                Claim dataAdd = new Claim();
                dataAdd.Notes = _mapper.Map<List<string>>(model.Notes);
                dataAdd.DocumentType = model.DocumentType?.ToString();
                dataAdd.Id = Guid.NewGuid().ToString();
                dataAdd.ScanDate = DateTime.Now;
                var listImg = new List<ImageInfo>() { };
                if (model.Images != null)
                {
                    foreach (var item in model.Images)
                    {
                        var tmp = await _azureStorage.UploadAsync(item, PackageCommon.RenameFile(item, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff_") + dataAdd.Id));
                        string textImg = await _extractAwsService.AmazoneScanForText(item);
                        listImg.Add(new ImageInfo()
                        {
                            Text = textImg,
                            Url = tmp.Blob.Uri
                            //Url = ""
                        });
                        if (string.IsNullOrWhiteSpace(dataAdd.Marketplace)
                            || string.IsNullOrWhiteSpace(dataAdd.RMA)
                            || string.IsNullOrWhiteSpace(dataAdd.MarketplaceOrderId))
                        {
                            if (textImg.Contains("Walmart Returns"))
                            {
                                dataAdd.Marketplace = "Walmart";
                                dataAdd.MarketplaceOrderId = PackageCommon.GetMarketplaceOrderId(textImg);
                                dataAdd.RMA = PackageCommon.GetRMA(textImg);
                            }
                            if (textImg.Contains("Amazon Return ID"))
                            {
                                dataAdd.Marketplace = "Amazon";
                                dataAdd.MarketplaceOrderId = PackageCommon.GetMarketplaceOrderId(textImg);
                                dataAdd.RMA = PackageCommon.GetRMA(textImg);
                            }
                        }

                    }
                }
                dataAdd.Images = listImg;
                await _claimRepository.InsertOne(dataAdd, _collectionName);
                return new OperationResult() { Success = true, Message = "Successfully created", Data = dataAdd };
            }
            catch (Exception ex)
            {
                await PackageCommon.WriteLogAsync($"Add Order Marketplace error: {ex.Message}", $"AddOrder_error_{DateTime.Now:yyyy-MM-dd_hh:mm:ss}.txt");
                return new OperationResult() { Success = false, Message = "System error", Data = model.DocumentType };
            }
        }

        public async Task<OperationResult> SaveTextFromImage(ClaimTextParam model)
        {
            try
            {
                if (model.DocumentType == null)
                {
                    return new OperationResult() { Success = false, Message = "Error created, DocumentType is not null", };
                }

                Claim dataAdd = new Claim();
                dataAdd.Notes = _mapper.Map<List<string>>(model.Notes);
                dataAdd.DocumentType = model.DocumentType?.ToString();
                dataAdd.Id = Guid.NewGuid().ToString();
                dataAdd.ScanDate = DateTime.Now;
                var listImg = new List<ImageInfo>() { };

                if (model.ImageInfo != null)
                {
                    foreach (var item in model.ImageInfo)
                    {
                        var tmp = await _azureStorage.UploadAsync(item.Image, PackageCommon.RenameFile(item.Image, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff_") + dataAdd.Id));
                        listImg.Add(new ImageInfo()
                        {
                            Text = item.TextExtract,
                            Url = tmp.Blob.Uri,
                        });
                        if (string.IsNullOrWhiteSpace(dataAdd.Marketplace)
                            || string.IsNullOrWhiteSpace(dataAdd.RMA)
                            || string.IsNullOrWhiteSpace(dataAdd.MarketplaceOrderId))
                        {
                            if (item.TextExtract.Contains("Walmart Returns"))
                            {
                                dataAdd.Marketplace = "Walmart";
                                dataAdd.MarketplaceOrderId = PackageCommon.GetMarketplaceOrderId(item.TextExtract);
                                dataAdd.RMA = PackageCommon.GetRMA(item.TextExtract);
                            }
                            if (item.TextExtract.Contains("Amazon Return ID"))
                            {
                                dataAdd.Marketplace = "Amazon";
                                dataAdd.MarketplaceOrderId = PackageCommon.GetMarketplaceOrderId(item.TextExtract);
                                dataAdd.RMA = PackageCommon.GetRMA(item.TextExtract);
                            }
                        }

                    }
                }
                dataAdd.Images = listImg;
                await _claimRepository.InsertOne(dataAdd, _collectionName);
                return new OperationResult() { Success = true, Message = "Successfully created", Data = dataAdd };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<string> CallApiGetTextFromImage(IFormFile item)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("clientId", $"{_configuration.GetValue<string>("TextExtractAPI:clientId")}");
            httpClient.DefaultRequestHeaders.Add("clientSecret", $"{_configuration.GetValue<string>("TextExtractAPI:clientSecret")}");

            var requestContent = new MultipartFormDataContent();
            var fileContent = new StreamContent(item.OpenReadStream());
            requestContent.Add(fileContent, "file", item.FileName);

            var response = await httpClient.PostAsync($"{_configuration.GetValue<string>("TextExtractAPI:GetTextURL")}", requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }
    }
}
