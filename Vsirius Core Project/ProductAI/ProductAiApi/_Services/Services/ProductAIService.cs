using Microsoft.EntityFrameworkCore;
using ProductAiApi._Repositories.Interfaces;
using ProductAiApi._Services.Interfaces;
using ProductAiApi.Common;
using ProductAiApi.DTOs;
using ProductAiApi.Models;
using System.Text.Json;

namespace ProductAiApi._Services.Services
{
    public class ProductAIService : IProductAIService
    {
        private readonly IProductAIRepository _productAIRepository;
        private readonly IConfiguration _configuration;

        public ProductAIService(
            IProductAIRepository productAIRepository,
            IConfiguration configuration)
        {
            _productAIRepository = productAIRepository;
            _configuration = configuration;
        }
        public async Task<PaginationUtil<ProductAiDto>> GetAll(PaginationParam pagination, string? search, bool? isReview)
        {
            IQueryable<ProductAI> query = _productAIRepository.FindAll().AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query
                    .Where(x =>
                            x.ProductName.ToLower().Contains(search)
                            || x.SKU.ToLower().Contains(search)
                            || x.Keywords.ToLower().Contains(search)
                            || x.Note.ToLower().Contains(search)
                );
            }
            if (isReview != null)
            {
                query = query
                    .Where(x => x.IsReview == isReview);
            }

            IQueryable<ProductAiDto?> queryDto = query.AsNoTracking().Select(x => CustomMapper.ChangeProductAIToDTO(x));
            var rs = await PaginationUtil<ProductAiDto>.CreateAsync(queryDto, pagination.PageNumber, pagination.PageSize);

            return rs;
        }

        public async Task<ProductAiDto?> GetDetail(string sku)
        {
            var entity = await _productAIRepository.FindById(sku);
            var dto = CustomMapper.ChangeProductAIToDTO(entity);
            return dto;
        }

        public async Task<OperationResult<ProductAiDto?>> CreateProduct(CreateUpdateProductAiDto dto)
        {
            var entity = await _productAIRepository.FindById(dto.SKU);
            if (entity != null)
            {
                return new OperationResult<ProductAiDto?>
                {
                    Success = false,
                    Message = "Product has exist",
                    Data = CustomMapper.ChangeProductAIToDTO(entity)
                };
            }
            else
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("clientId", _configuration.GetValue<string>("ChatGptApi:clientId"));
                httpClient.DefaultRequestHeaders.Add("clientSecret", _configuration.GetValue<string>("ChatGptApi:clientSecret"));

                string content =
                    $"This is my product information: {dto.Description}" +
                    $"\nPlease create the product name (maximum 200 characters and without special characters), " +
                    $"\nkeyword product, bulletpoints (maximum 9 points and maximum 1000 characters every point)" +
                    $"\n to make the product suitable for SEO on amazon, ebay, google... . " +
                    $"\nReply me with the following format: " +
                    $"\n|title: ..." +
                    $"\n|brand: ...(if exist)" +
                    $"\n|keywords: abc, xyz, ..." +
                    $"\n|bulletpoint1: ..." +
                    $"\n|bulletpoint2: ..." +
                    $"\n|...";

                var encodedContent = Uri.EscapeDataString(content);

                var url = $"{_configuration.GetValue<string>("ChatGptApi:ChatBot")}?inputChat={encodedContent}";

                var response = await httpClient.PostAsync(url, null);

                var contentResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<List<ChatGptReponse>>(contentResponse);

                    if (result != null && result.Count > 0)
                    {
                        string contentMessage = result[^1].content!;
                        var brandAI = GetValueFromContent("|brand:", contentMessage);
                        var myBrand = string.IsNullOrWhiteSpace(dto.Brand) ? "Generic" : dto.Brand;

                        entity = new()
                        {
                            SKU = dto.SKU,
                            ProductName = GetValueFromContent("|title:", contentMessage),
                            Keywords = GetValueFromContent("|keywords:", contentMessage),
                            BulletPoint1 = GetValueFromContent("|bulletpoint1:", contentMessage),
                            BulletPoint2 = GetValueFromContent("|bulletpoint2:", contentMessage),
                            BulletPoint3 = GetValueFromContent("|bulletpoint3:", contentMessage),
                            BulletPoint4 = GetValueFromContent("|bulletpoint4:", contentMessage),
                            BulletPoint5 = GetValueFromContent("|bulletpoint5:", contentMessage),
                            BulletPoint6 = GetValueFromContent("|bulletpoint6:", contentMessage),
                            BulletPoint7 = GetValueFromContent("|bulletpoint7:", contentMessage),
                            BulletPoint8 = GetValueFromContent("|bulletpoint8:", contentMessage),
                            BulletPoint9 = GetValueFromContent("|bulletpoint9:", contentMessage),
                            IsReview = false,
                            Note = contentMessage,
                            TimeStamp = DateTime.Now,
                        };


                        if (!entity.ProductName.Contains(brandAI, StringComparison.OrdinalIgnoreCase))
                        {
                            entity.ProductName = $"{myBrand} {entity.ProductName}";
                            entity.Brand = myBrand;
                        }
                        else
                        {
                            entity.Brand = brandAI;
                        }

                        _productAIRepository.Add(entity);
                        await _productAIRepository.SaveChangesAsync();

                        return new OperationResult<ProductAiDto?>
                        {
                            Success = true,
                            Data = CustomMapper.ChangeProductAIToDTO(entity)
                        };
                    }
                    else
                    {
                        return new OperationResult<ProductAiDto?>
                        {
                            Success = false,
                            Message = "Limitted request today",
                            Data = null
                        };
                    }

                }
                else
                {
                    return new OperationResult<ProductAiDto?>
                    {
                        Success = false,
                        Message = "Connect To Open AI failed",
                        Data = null
                    };
                }


            }
        }

        private string GetValueFromContent(string keyword, string content)
        {
            // Tìm vị trí của keyword trong content
            int index = content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);

            // Nếu không tìm thấy keyword, trả về chuỗi rỗng
            if (index == -1)
                return string.Empty;

            // Lấy phần tử từ vị trí của keyword đến hết chuỗi
            string substring = content.Substring(index + keyword.Length);

            // Tách chuỗi bằng dấu chấm phẩy hoặc dấu chấm phẩy có khoảng trắng
            string[] values = substring.Split('|', StringSplitOptions.RemoveEmptyEntries);

            // Lấy giá trị đầu tiên (nếu có)
            string? result = values.FirstOrDefault()?.Trim();

            return result ?? string.Empty;

        }

        public async Task<OperationResult<object?>> Delete(string sku)
        {
            var entity = await _productAIRepository.FindById(sku);

            _productAIRepository.Remove(entity);
            return new OperationResult<object?>
            {
                Success = await _productAIRepository.SaveChangesAsync()
            };
        }

        public async Task<OperationResult<object?>> ConfirmProducts(List<string> skus)
        {
            var lstConfirm = await _productAIRepository.FindAll(x => skus.Contains(x.SKU)).ToListAsync();
            lstConfirm.ForEach(x =>
            {
                x.IsReview = true;
            });
            return new OperationResult<object?>
            {
                Success = await _productAIRepository.SaveChangesAsync()
            };
        }

        public async Task<OperationResult<ProductAiDto?>> UpdateFromAI(CreateUpdateProductAiDto dto)
        {
            var entity = await _productAIRepository.FindById(dto.SKU);
            if (entity == null)
            {
                return new OperationResult<ProductAiDto?>
                {
                    Success = false,
                    Message = "Product not exist",
                    Data = CustomMapper.ChangeProductAIToDTO(entity)
                };
            }
            else
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("clientId", _configuration.GetValue<string>("ChatGptApi:clientId"));
                httpClient.DefaultRequestHeaders.Add("clientSecret", _configuration.GetValue<string>("ChatGptApi:clientSecret"));

                string content =
                    $"This is my product information: {dto.Description}" +
                    $"\nPlease create the product name (maximum 200 characters and without special characters), " +
                    $"\nkeyword product, bulletpoints (maximum 9 points and maximum 1000 characters every point)" +
                    $"\n to make the product suitable for SEO on amazon, ebay, google... . " +
                    $"\nReply me with the following format: " +
                    $"\n|title: ..." +
                    $"\n|brand: ...(if exist)" +
                    $"\n|keywords: abc, xyz, ..." +
                    $"\n|bulletpoint1: ..." +
                    $"\n|bulletpoint2: ..." +
                    $"\n|...";

                var encodedContent = Uri.EscapeDataString(content);

                var url = $"{_configuration.GetValue<string>("ChatGptApi:ChatBot")}?inputChat={encodedContent}";

                var response = await httpClient.PostAsync(url, null);

                var contentResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<List<ChatGptReponse>>(contentResponse);

                    if (result != null && result.Count > 0)
                    {
                        string contentMessage = result[^1].content!;
                        var brandAI = GetValueFromContent("|brand:", contentMessage);
                        var myBrand = string.IsNullOrWhiteSpace(dto.Brand) ? "Generic" : dto.Brand;

                        entity = new()
                        {
                            SKU = dto.SKU,
                            ProductName = GetValueFromContent("|title:", contentMessage),
                            Keywords = GetValueFromContent("|keywords:", contentMessage),
                            BulletPoint1 = GetValueFromContent("|bulletpoint1:", contentMessage),
                            BulletPoint2 = GetValueFromContent("|bulletpoint2:", contentMessage),
                            BulletPoint3 = GetValueFromContent("|bulletpoint3:", contentMessage),
                            BulletPoint4 = GetValueFromContent("|bulletpoint4:", contentMessage),
                            BulletPoint5 = GetValueFromContent("|bulletpoint5:", contentMessage),
                            BulletPoint6 = GetValueFromContent("|bulletpoint6:", contentMessage),
                            BulletPoint7 = GetValueFromContent("|bulletpoint7:", contentMessage),
                            BulletPoint8 = GetValueFromContent("|bulletpoint8:", contentMessage),
                            BulletPoint9 = GetValueFromContent("|bulletpoint9:", contentMessage),
                            IsReview = false,
                            Note = contentMessage,
                            TimeStamp = DateTime.Now,
                        };


                        if (!entity.ProductName.Contains(brandAI, StringComparison.OrdinalIgnoreCase))
                        {
                            entity.ProductName = $"{myBrand} {entity.ProductName}";
                            entity.Brand = myBrand;
                        }
                        else
                        {
                            entity.Brand = brandAI;
                        }

                        _productAIRepository.Update(entity);

                        return new OperationResult<ProductAiDto?>
                        {
                            Success = await _productAIRepository.SaveChangesAsync(),
                            Data = CustomMapper.ChangeProductAIToDTO(entity)
                        };
                    }
                    else
                    {
                        return new OperationResult<ProductAiDto?>
                        {
                            Success = false,
                            Message = "Limitted request today",
                            Data = null
                        };
                    }

                }
                else
                {
                    return new OperationResult<ProductAiDto?>
                    {
                        Success = false,
                        Message = "Connect To Open AI failed",
                        Data = null
                    };
                }


            }
        }

        public async Task<OperationResult<ProductAiDto?>> UpdateFromUser(ProductAiDto dto)
        {
            var entity = await _productAIRepository.FindById(dto.SKU);
            if (entity == null)
            {
                return new OperationResult<ProductAiDto?>()
                {
                    Success = false,
                    Message = "Product not exists"
                };
            }
            else
            {
                entity.ProductName = dto.ProductName;
                entity.Brand = dto.Brand;
                entity.BulletPoint1 = dto.BulletPoint1;
                entity.BulletPoint2 = dto.BulletPoint2;
                entity.BulletPoint3 = dto.BulletPoint3;
                entity.BulletPoint4 = dto.BulletPoint3;
                entity.BulletPoint5 = dto.BulletPoint3;
                entity.BulletPoint6 = dto.BulletPoint3;
                entity.BulletPoint7 = dto.BulletPoint3;
                entity.BulletPoint8 = dto.BulletPoint3;
                entity.BulletPoint9 = dto.BulletPoint3;
                entity.Keywords = dto.Keywords;
                entity.Note = dto.Note;
                entity.TimeStamp = DateTime.Now;
                entity.IsReview = true;
            }

            _productAIRepository.Update(entity);

            return new OperationResult<ProductAiDto?>()
            {
                Success = await _productAIRepository.SaveChangesAsync(),
                Data = dto
            };
        }

        public async Task<PaginationUtil<string>?> GetAllSku(PaginationParam pagination)
        {
            var result = await PaginationUtil<string>.CreateAsync(
                _productAIRepository.FindAll().AsNoTracking().Select(x => x.SKU), 
                pagination.PageNumber, 
                pagination.PageSize);
            return result;
        }
    }
}
