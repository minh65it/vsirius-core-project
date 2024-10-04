using AmazonIntegrationDataApi.Data;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.Utilities;
using AmazonIntegrationDataApi.Models;
using AutoMapper;
using EFCore.BulkExtensions;
using System.Reflection;

namespace AmazonIntegrationDataApi.Helpers.MapVSiriusDBToAmazonDB
{
    public interface IInsertProduct
    {
        public Task<OperationResult> Insert(List<AmazonJewelryDataFeedItemV3_Dto> input0, List<ExcludedProductDto>? lstExcludedProduct);
    }
    public class InsertProduct : IInsertProduct
    {
        private readonly DBContext _context;
        private readonly ValidationAmazon _valid;
        private readonly IMapper _mapper;
        private readonly IExcludedProduct _excludedProduct;

        public InsertProduct(DBContext context,
            ValidationAmazon valid,
            IMapper mapper,
            IExcludedProduct excludedProduct)
        {
            _context = context;
            _valid = valid;
            _mapper = mapper;
            _excludedProduct = excludedProduct;
        }
        public async Task<OperationResult> Insert(List<AmazonJewelryDataFeedItemV3_Dto> input, List<ExcludedProductDto>? lstExcludedProduct)
        {
            if (input == null)
                return new OperationResult(true);
            var filter = Filters.filtersSKU(input);
            //ExcludedProduct========================================
            var createdExcluded = input.Except(filter).ToList();
            List<ExcludedProductDto> listUpdate = new List<ExcludedProductDto>();
            foreach (var item in createdExcluded)
            {
                listUpdate.Add(new ExcludedProductDto()
                {
                    Marketplace = "Amazon",
                    MarketplaceSKU = item?.item_sku,
                    SupplierSKU = item?.item_sku[2..],
                    ExclusionType = "Deleted",
                    ExclusionNote = $"Suspected Intellectual Property Violations: {item.item_name}"
                });
            }
            //Price
            filter = Filters.filterPrice(filter);

            var createdExcludedPrice = input.Except(filter).ToList();

            foreach (var item in createdExcludedPrice)
            {
                listUpdate.Add(new ExcludedProductDto()
                {
                    Marketplace = "Amazon",
                    MarketplaceSKU = item?.item_sku,
                    SupplierSKU = item?.item_sku[2..],
                    ExclusionType = "Deleted",
                    ExclusionNote = $"Amazon price over $3500.00: {item.item_name}"
                });
            }
            listUpdate = listUpdate
                .GroupBy(p => p.MarketplaceSKU)
                .Select(g => g.First())
                .ToList();
            await _excludedProduct.UpdateExcludedProducts(listUpdate);

            if (lstExcludedProduct != null)
            {
                var inputSkuCluded = filter.Select(x => x.item_sku).ToList().Except(lstExcludedProduct.Select(x => x.MarketplaceSKU).ToList());
                filter = filter.Where(x => inputSkuCluded.Contains(x.item_sku)).ToList();
            }
            //End ExcludedProduct========================================
            var products = new List<AmazonJewelryDataFeedItemV3_Dto>();
            var notifications = new List<AmazonJewelryDataFeedItemV3_Dto>();

            for (int row = 0; row < filter?.Count(); row++)
            {
                filter[row].external_product_id = null;
                filter[row].external_product_id_type = null;
                if (!string.IsNullOrWhiteSpace(filter[row].brand_name))
                {
                    var msg = _valid.IsValid(filter[row]);
                    if (IsAllPropertiesNull(msg))
                    {
                        products.Add(filter[row]);
                    }
                    else
                    {
                        msg.item_sku = filter[row].item_sku;
                        notifications.Add(msg);
                    }
                }
            };

            try
            {
                if (notifications.Count > 0)
                {
                    await Utilities2.WriteLogAsync($"Notifications invalid {notifications.Count}");
                    //return new OperationResult { IsSuccess = false, Data = notifications };
                }

                var lstProduct = _mapper.Map<List<AmazonJewelryDataFeedItemV3>>(products);

                await InsertSQLDatabase(lstProduct);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                await Utilities2.WriteLogAsync($"Insert page error: {ex.ToString()}");
                return new OperationResult(false, error: ex.Message);
            }
        }
        public bool IsAllPropertiesNull(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);

                if (value != null)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<bool> InsertSQLDatabase(List<AmazonJewelryDataFeedItemV3> lstProduct)
        {
            var curDate = DateTime.Now;
            lstProduct.ForEach(x => x.UpdatedDate = curDate);
            if (lstProduct.Count > 0)
            {
                int batchSize = 1000; // Số lượng tối đa trong mỗi danh sách con
                int numBatches = (int)Math.Ceiling((double)lstProduct.Count / batchSize);

                for (int i = 0; i < numBatches; i++)
                {
                    var batch = lstProduct
                        .Skip(i * batchSize)
                        .Take(batchSize)
                        .ToList();

                    await _context.BulkInsertOrUpdateAsync(batch);
                }
                await Utilities2.WriteLogAsync($"Insert {lstProduct.Count} to Database Amazon");
            }
            return true;
        }
    }
}
