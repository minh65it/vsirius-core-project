using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using PimApi._Services.Interfaces;
using PimApi.Context;
using PimApi.Dtos.Variation;
using PimApi.Helpers.Utilities;
using PimApi.Models;
using VSiriusSystemLog.WriteLog;


namespace PimApi._Services.Services
{
    public class VariationService : IVariationService
    {
        private readonly DataContext _dataContext;
        private readonly ILoggerCustom _logService;
        private readonly IMemoryCache _memoryCache;

        public VariationService(
            DataContext dataContext,
            ILoggerCustom logService,
            IMemoryCache memoryCache)
        {
            _dataContext = dataContext;
            _logService = logService;
            _memoryCache = memoryCache;
        }

        public async Task<List<VariationDto>> GetAll()
        {
            var result = new List<VariationDto>();
            string cacheKey = "CacheVariation";
            if (_memoryCache.TryGetValue(cacheKey, out result) == false)
            {
                result = await _dataContext
                .Variations
                .AsNoTracking()
                .Select(x =>
                new VariationDto()
                {
                    ItemSku = x.ItemSku,
                    VariationId = x.VariationId,
                    VariationName = x.VariationName,
                })
                .ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(5));

                // Save data in cache
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);
            }

            return result!;
        }

        public async Task UploadVariationFromPIM()
        {
            try
            {
                var lstPimProduct = new List<PimMapProductDto>();

                //Mở đoạn dưới lên khi dev nếu muốn nhanh
                //var filePath = @"D:\New\product.json";
                //try
                //{
                //    string jsonString = File.ReadAllText(filePath);
                //    if (!string.IsNullOrEmpty(jsonString))
                //    {
                //        lstPimProduct = JsonConvert.DeserializeObject<List<PimMapProductDto>>(jsonString);
                //    }
                //    else
                //    {
                //        lstPimProduct = await GetAllQgoldProductFromPim();
                //        File.WriteAllText(filePath, JsonConvert.SerializeObject(lstPimProduct));
                //    }
                //}
                //catch (Exception)
                //{

                //}
                lstPimProduct = await GetAllQgoldProductFromPim();
                var lstInsertUpdateVariation = new List<Variation>();

                #region ring, jewelryset => (metal, ringsize)
                var trimmedGroupList = lstPimProduct
                .Where(
                    x => (x.feed_product_type == "ring" || x.feed_product_type == "jewelryset")
                         && x.item_sku.Count(c => c == '-') > 1
                )
                .Select(item =>
                {
                    var lastHyphenIndex = item.item_sku.LastIndexOf('-');
                    var substringAfterLastHyphen = item.item_sku[(lastHyphenIndex + 1)..];
                    bool isNumber = double.TryParse(substringAfterLastHyphen, out double rs);

                    return new RingJewelrySetDto()
                    {
                        VariationId = item.item_sku[..lastHyphenIndex],
                        Suffix = substringAfterLastHyphen,
                        ItemSku = item.item_sku,
                        Size = item.ring_size,
                        MetalType = item.metal_type,
                        IsNumber = isNumber
                    };
                })
                .GroupBy(x => x.VariationId)
                .Where(g => g.Count() > 1)
                .ToList();

                foreach (var group in trimmedGroupList)
                {
                    var lstItemNotNumber = group.Where(x => x.IsNumber == false).ToList();

                    //Nhóm theo metaltype
                    if (lstItemNotNumber.Any() && lstItemNotNumber.Select(x => x.MetalType).Distinct().Count() == lstItemNotNumber.Count)
                    {
                        foreach (var item in lstItemNotNumber)
                        {
                            lstInsertUpdateVariation.Add(new()
                            {
                                VariationId = item.VariationId,
                                ItemSku = item.ItemSku,
                                VariationName = "Metal",
                            });
                        }
                    }
                    else
                    {
                        //Nhóm theo ringsize
                        foreach (var item in lstItemNotNumber)
                        {
                            lstInsertUpdateVariation.Add(new()
                            {
                                VariationId = item.VariationId,
                                ItemSku = item.ItemSku,
                                VariationName = "RingSize",
                            });
                        }
                    }

                    //nhóm theo ringsize
                    var lstItemIsNumber = group.Where(x => x.IsNumber == true).ToList();
                    if (lstItemIsNumber.Any())
                    {
                        foreach (var item in lstItemIsNumber)
                        {
                            lstInsertUpdateVariation.Add(new()
                            {
                                VariationId = item.VariationId,
                                ItemSku = item.ItemSku,
                                VariationName = "RingSize",
                            });
                        }
                    }
                }
                #endregion

                #region earring => Size(carat), metal
                var trimmedGroupList1 = lstPimProduct
                .Where(
                    x => (x.feed_product_type == "earring")
                         && x.item_sku.Count(c => c == '-') > 1
                )
                .Select(item =>
                {
                    var lastHyphenIndex = item.item_sku.LastIndexOf('-');
                    var substringAfterLastHyphen = item.item_sku[(lastHyphenIndex + 1)..];
                    bool isNumber = double.TryParse(substringAfterLastHyphen, out double rs);

                    return new EarringDto()
                    {
                        VariationId = item.item_sku[..lastHyphenIndex],
                        Suffix = substringAfterLastHyphen,
                        ItemSku = item.item_sku,
                        Size = item.item_size,
                        MetalType = item.metal_type,
                        IsNumber = isNumber
                    };
                })
                .GroupBy(x => x.VariationId)
                .Where(g => g.Count() > 1)
                .ToList();

                foreach (var group in trimmedGroupList1)
                {
                    //Nhóm theo metaltype
                    var lstItemNotNumber = group.Where(x => x.IsNumber == false).ToList();
                    if (lstItemNotNumber.Any())
                    {
                        //Nhóm theo metaltype
                        if (lstItemNotNumber.Select(x => x.MetalType).Distinct().Count() > 1)
                        {
                            foreach (var item in lstItemNotNumber)
                            {
                                lstInsertUpdateVariation.Add(new()
                                {
                                    VariationId = item.VariationId,
                                    ItemSku = item.ItemSku,
                                    VariationName = "Metal",
                                });
                            }
                        }
                        //Nhóm theo Size Carat
                        else if (lstItemNotNumber.Select(x => x.Size).Distinct().Count() > 1)
                        {
                            foreach (var item in lstItemNotNumber)
                            {
                                lstInsertUpdateVariation.Add(new()
                                {
                                    VariationId = item.VariationId,
                                    ItemSku = item.ItemSku,
                                    VariationName = "Size",
                                });
                            }
                        }
                    }

                    var lstItemIsNumber = group.Where(x => x.IsNumber == true).ToList();
                    if (lstItemIsNumber.Any())
                    {
                        if (lstItemIsNumber.Select(x => x.Size).Distinct().Count() > 1)
                        {
                            foreach (var item in lstItemIsNumber)
                            {
                                lstInsertUpdateVariation.Add(new()
                                {
                                    VariationId = item.VariationId,
                                    ItemSku = item.ItemSku,
                                    VariationName = "Size",
                                });
                            }
                        }
                    }
                }
                #endregion

                #region bracelet, necklace
                var trimmedGroupList2 = lstPimProduct
                .Where(
                    x => (x.feed_product_type == "necklace" || x.feed_product_type == "bracelet")
                         && x.item_sku.Count(c => c == '-') > 1
                )
                .Select(item =>
                {
                    var lastHyphenIndex = item.item_sku.LastIndexOf('-');
                    var substringAfterLastHyphen = item.item_sku[(lastHyphenIndex + 1)..];
                    bool isNumber = double.TryParse(substringAfterLastHyphen, out double rs);

                    return new ChainDto()
                    {
                        VariationId = item.item_sku[..lastHyphenIndex],
                        Suffix = substringAfterLastHyphen,
                        ItemSku = item.item_sku,
                        ChainLength = item.item_length,
                        MetalType = item.metal_type,
                        IsNumber = isNumber
                    };
                })
                .GroupBy(x => x.VariationId)
                .Where(g => g.Count() > 1)
                .ToList();

                foreach (var group in trimmedGroupList2)
                {
                    //Nhóm theo metaltype
                    var lstItemNotNumber = group.Where(x => x.IsNumber == false).ToList();
                    if (lstItemNotNumber.Any())
                    {
                        //Nhóm theo metaltype
                        if (lstItemNotNumber.Select(x => x.MetalType).Distinct().Count() > 1)
                        {
                            foreach (var item in lstItemNotNumber)
                            {
                                lstInsertUpdateVariation.Add(new()
                                {
                                    VariationId = item.VariationId,
                                    ItemSku = item.ItemSku,
                                    VariationName = "Metal",
                                });
                            }
                        }
                        //Nhóm theo Size Carat
                        else if (lstItemNotNumber.Select(x => x.ChainLength).Distinct().Count() > 1)
                        {
                            foreach (var item in lstItemNotNumber)
                            {
                                if (!string.IsNullOrWhiteSpace(item.ChainLength)
                                    && double.Parse(item.ChainLength) > 0)
                                {
                                    lstInsertUpdateVariation.Add(new()
                                    {
                                        VariationId = item.VariationId,
                                        ItemSku = item.ItemSku,
                                        VariationName = "ChainLength",
                                    });
                                }
                            }
                        }
                    }

                    var lstItemIsNumber = group.Where(x => x.IsNumber == true).ToList();
                    if (lstItemIsNumber.Any())
                    {
                        if (lstItemIsNumber.Select(x => x.ChainLength).Distinct().Count() > 1)
                        {
                            foreach (var item in lstItemIsNumber)
                            {
                                lstInsertUpdateVariation.Add(new()
                                {
                                    VariationId = item.VariationId,
                                    ItemSku = item.ItemSku,
                                    VariationName = "ChainLength",
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{group.Key} in Qgold have wrong data");
                        }
                    }
                }
                #endregion

                #region charm
                var trimmedGroupList3 = lstPimProduct
                .Where(
                    x => (x.feed_product_type == "charm")
                         && x.item_sku.Count(c => c == '-') > 1
                )
                .Select(item =>
                {
                    var lastHyphenIndex = item.item_sku.LastIndexOf('-');
                    var substringAfterLastHyphen = item.item_sku[(lastHyphenIndex + 1)..];
                    bool isNumber = double.TryParse(substringAfterLastHyphen, out double rs);

                    return new CharmDto()
                    {
                        VariationId = item.item_sku[..lastHyphenIndex],
                        Suffix = substringAfterLastHyphen,
                        ItemSku = item.item_sku,
                        MetalType = item.metal_type,
                        IsNumber = isNumber
                    };
                })
                .GroupBy(x => x.VariationId)
                .Where(g => g.Count() > 1)
                .ToList();

                foreach (var group in trimmedGroupList3)
                {
                    //Nhóm theo metaltype
                    var lstItemNotNumber = group.Where(x => x.IsNumber == false).ToList();
                    if (lstItemNotNumber.Any())
                    {
                        //Nhóm theo metaltype
                        if (lstItemNotNumber.Select(x => x.MetalType).Distinct().Count() > 1)
                        {
                            foreach (var item in lstItemNotNumber)
                            {
                                lstInsertUpdateVariation.Add(new()
                                {
                                    VariationId = item.VariationId,
                                    ItemSku = item.ItemSku,
                                    VariationName = "Metal",
                                });
                            }
                        }
                        else
                        {
                            Console.WriteLine($"VariantId: {group.Key} have Letter Or Symbol can not group");
                        }
                    }
                }
                #endregion

                await _dataContext.Variations.ExecuteDeleteAsync();

                _dataContext.Variations.AddRange(lstInsertUpdateVariation);
                await _dataContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await _logService.CreateLogInformation($"UploadVariationFromPIM Trigger Error: {ex.Message}");
            }
        }

        private async Task<List<PimMapProductDto>> GetAllQgoldProductFromPim()
        {
            var lstPimProduct = new List<PimMapProductDto>();
            var queryProductFromPIM =
                _dataContext
                .Products
                .AsNoTracking()
                .Where(x =>
                x.IsDelete != true
                && x.item_sku.StartsWith("Q")
                );

            int pageNumber = 1;
            int pageSize = 10_000;
            var firstData = await PaginationUtility<PimMapProductDto>
                .CreateAsync(queryProductFromPIM
                .Select(x => new PimMapProductDto()
                {
                    item_sku = x.item_sku,
                    color_name = x.color_name,
                    feed_product_type = x.feed_product_type,
                    item_length = x.item_length,
                    item_weight = x.item_weight,
                    metal_type = x.metal_type,
                    ring_size = x.ring_size,
                    style_name = x.style_name,
                    item_size = x.size_name
                }), pageNumber, pageSize);

            lstPimProduct.AddRange(firstData.Result);

            for (pageNumber = 2; pageNumber <= firstData.Pagination.TotalPage; pageNumber++)
            {
                var data = await PaginationUtility<PimMapProductDto>
                .CreateAsync(queryProductFromPIM
                .Select(x => new PimMapProductDto()
                {
                    item_sku = x.item_sku,
                    color_name = x.color_name,
                    feed_product_type = x.feed_product_type,
                    item_length = x.item_length,
                    item_weight = x.item_weight,
                    metal_type = x.metal_type,
                    ring_size = x.ring_size,
                    style_name = x.style_name
                }), pageNumber, pageSize);

                lstPimProduct.AddRange(data.Result);
            }

            return lstPimProduct;
        }
    }
}
