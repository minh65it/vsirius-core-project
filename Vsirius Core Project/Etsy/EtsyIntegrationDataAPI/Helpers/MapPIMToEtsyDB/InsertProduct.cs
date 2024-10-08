using AutoMapper;
using EFCore.BulkExtensions;
using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Data;
using EtsyIntegrationDataAPI.Dtos.MapPIMToEtsyDB;
using EtsyIntegrationDataAPI.Models;
using EtsyIntegrationDataAPI.Utilities;
using VSiriusSystemLog.WriteLog;

namespace EtsyIntegrationDataAPI.Helpers.MapPIMToEtsyDB
{
    public interface IInsertProduct
    {
        public Task<OperationResult> Insert(List<ProductPIMDto> input, List<ExcludedProductDto>? lstExcludedProduct, List<ImageProduct> checkLstImg);
    }
    public class InsertProduct : IInsertProduct
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IExcludedProduct _excludedProduct;
        private readonly IPropertiesProductService _propertiesProductService;
        private ILoggerCustom _logService;

        public InsertProduct(DataContext context,
            IMapper mapper,
            IExcludedProduct excludedProduct,
            ILoggerCustom logService,
            IPropertiesProductService propertiesProductService)
        {
            _context = context;
            _mapper = mapper;
            _excludedProduct = excludedProduct;
            _logService = logService;
            _propertiesProductService = propertiesProductService;
        }

        public async Task<OperationResult> Insert(List<ProductPIMDto> input, List<ExcludedProductDto>? lstExcludedProduct, List<ImageProduct> checkLstImg)
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
                    Marketplace = "Etsy",
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
                    Marketplace = "Etsy",
                    MarketplaceSKU = item?.item_sku,
                    SupplierSKU = item?.item_sku[2..],
                    ExclusionType = "Deleted",
                    ExclusionNote = $"Etsy price over $3500.00: {item.item_name}"
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
            var lstProduct = new List<ProductEtsy>();
            var lstMaterial = new List<MaterialProduct>();
            var lstTag = new List<TagProduct>();
            var lstStyle = new List<StyleProduct>();
            var lstImage = new List<ImageProduct>();
            var lstImageUpdate = new List<ImageProduct>();



            for (int row = 0; row < filter?.Count(); row++)
            {
                try
                {
                    if (filter[row]?.item_sku != null)
                    {
                        var product = new ProductEtsy();
                        product.item_sku = filter[row]?.item_sku;
                        product.quantity = int.Parse(filter[row]?.quantity);
                        product.title = ConvertPIMToEtsyDB.GenTitle(filter[row]?.item_name);
                        product.description = ConvertPIMToEtsyDB.GenDescription(filter[row]);
                        product.price = decimal.Parse(filter[row]?.standard_price);
                        product.who_made = "i_did";
                        product.when_made = "2020_2024";
                        product.taxonomy_id = ConvertPIMToEtsyDB.TaxonomyId(filter[row]?.feed_product_type);
                        product.shipping_profile_id = ConvertPIMToEtsyDB.ShippingProfileId(product.title);
                        product.return_policy_id = ConvertPIMToEtsyDB.ReturnPolicyId(product.title); ;

                        //material
                        var material = new MaterialProduct();
                        material.item_sku = product.item_sku;
                        material.material = filter[row]?.material_type1;
                        lstMaterial.Add(material);
                        product.shop_section_id = null;
                        product.processing_min = null;
                        product.processing_max = null;

                        //tag
                        var tag = new TagProduct();
                        tag.item_sku = product.item_sku;
                        tag.tag = filter[row]?.item_type_name;
                        lstTag.Add(tag);

                        //style
                        var style = new StyleProduct();
                        style.item_sku = product.item_sku;
                        style.style = filter[row]?.style_name;
                        lstStyle.Add(style);
                        product.item_weight = ConvertPIMToEtsyDB.ItemWeigh(filter[row]?.item_weight);
                        //theo nguyen tắc dài >= rộng >= cao
                        product.item_length = ConvertPIMToEtsyDB.GetItemLength(filter[row]?.item_length, filter[row]?.item_width);
                        product.item_width = ConvertPIMToEtsyDB.GetItemWidth(filter[row]?.item_width);
                        product.item_height = ConvertPIMToEtsyDB.GetItemHeight(filter[row]?.item_height, filter[row]?.item_width);
                        product.item_weight_unit = ConvertPIMToEtsyDB.ItemWeightUnit(filter[row]?.item_weight_unit_of_measure, filter[row]?.item_weight);
                        product.item_dimensions_unit = ConvertPIMToEtsyDB.ItemDimensionsUnit(filter[row]?.item_width_unit_of_measure); ;
                        product.is_personalizable = false;
                        product.personalization_is_required = false;
                        product.personalization_char_count_max = null;
                        product.personalization_instructions = "";
                        product.production_partner_ids = null;

                        //Image
                        var checkUpdateImg = checkLstImg?.Where(x => x.item_sku == product.item_sku).ToList();
                        if (checkUpdateImg?.Count == 0)
                        {
                            var image = new ImageProduct();
                            image.item_sku = product.item_sku;
                            image.image = filter[row]?.main_image_url;
                            image.listing_image_id = 1;
                            image.rank = 1;
                            image.overwrite = false;
                            image.is_watermarked = false;
                            image.alt_text = product.item_sku;
                            lstImage.Add(image);
                            if (!string.IsNullOrWhiteSpace(filter[row]?.other_image_url1))
                            {
                                var image1 = new ImageProduct();
                                image1.item_sku = product.item_sku;
                                image1.image = filter[row]?.other_image_url1;
                                image1.rank = 2;
                                image1.overwrite = false;
                                image1.is_watermarked = false;
                                image1.alt_text = product.item_sku;
                                lstImage.Add(image1);
                                if (!string.IsNullOrWhiteSpace(filter[row]?.other_image_url2))
                                {
                                    var image2 = new ImageProduct();
                                    image2.item_sku = product.item_sku;
                                    image2.image = filter[row]?.other_image_url2;
                                    image2.rank = 3;
                                    image2.overwrite = false;
                                    image2.is_watermarked = false;
                                    image2.alt_text = product.item_sku;
                                    lstImage.Add(image2);
                                    if (!string.IsNullOrWhiteSpace(filter[row]?.other_image_url3))
                                    {
                                        var image3 = new ImageProduct();
                                        image3.item_sku = product.item_sku;
                                        image3.image = filter[row]?.other_image_url3;
                                        image3.rank = 4;
                                        image3.overwrite = false;
                                        image3.is_watermarked = false;
                                        image3.alt_text = product.item_sku;
                                        lstImage.Add(image3);
                                        if (!string.IsNullOrWhiteSpace(filter[row]?.other_image_url4))
                                        {
                                            var image4 = new ImageProduct();
                                            image4.item_sku = product.item_sku;
                                            image4.image = filter[row]?.other_image_url4;
                                            image4.rank = 5;
                                            image4.overwrite = false;
                                            image4.is_watermarked = false;
                                            image4.alt_text = product.item_sku;
                                            lstImage.Add(image4);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            int count = checkUpdateImg.Count();
                            checkUpdateImg[0].image = filter[row]?.main_image_url;
                            checkUpdateImg[0].rank = 1;
                            checkUpdateImg[0].alt_text = product.item_sku;
                            lstImageUpdate.Add(checkUpdateImg[0]);
                            if (count > 1)
                            {
                                checkUpdateImg[1].image = filter[row]?.other_image_url1;
                                checkUpdateImg[1].rank = 2;
                                checkUpdateImg[1].alt_text = product.item_sku;
                                lstImageUpdate.Add(checkUpdateImg[1]);
                            }
                            if (count > 2)
                            {
                                checkUpdateImg[2].image = filter[row]?.other_image_url2;
                                checkUpdateImg[2].rank = 3;
                                checkUpdateImg[2].alt_text = product.item_sku;
                                lstImageUpdate.Add(checkUpdateImg[2]);
                            }
                            if (count > 3)
                            {
                                checkUpdateImg[3].image = filter[row]?.other_image_url3;
                                checkUpdateImg[3].rank = 4;
                                checkUpdateImg[3].alt_text = product.item_sku;
                                lstImageUpdate.Add(checkUpdateImg[3]);
                            }
                            if (count > 4)
                            {
                                checkUpdateImg[4].image = filter[row]?.other_image_url4;
                                checkUpdateImg[4].rank = 5;
                                checkUpdateImg[4].alt_text = product.item_sku;
                                lstImageUpdate.Add(checkUpdateImg[4]);
                            }
                        }

                        product.is_supply = false;
                        product.is_customizable = false;
                        product.should_auto_renew = false;
                        product.is_taxable = true;
                        product.type = "physical";

                        //add
                        lstProduct.Add(product);

                    }
                }
                catch (Exception ex)
                {
                    continue;
                }
            };

            try
            {
                await InsertSQLProduct(lstProduct);
                await InsertSQL(lstMaterial);
                await InsertSQL(lstTag);
                await InsertSQL(lstStyle);
                if (lstImage.Count > 0)
                {
                    await InsertSQL(lstImage);
                }
                if (lstImageUpdate.Count > 0)
                {
                    await UpdateSQLImage(lstImageUpdate);
                }

                //Update Properties Product
                await _propertiesProductService.UpdatePropertiesProduct(filter);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                await _logService.CreateLogError($"Insert page error: {ex.ToString()}");
                return new OperationResult(false, message: ex.Message);
            }
        }
        public async Task<bool> InsertSQLProduct(List<ProductEtsy> lstProduct)
        {
            ProductEtsy model = new ProductEtsy();
            var curDate = DateTime.Now;
            lstProduct.ForEach(x => x.updatedDate = curDate);
            if (lstProduct.Count > 0)
            {
                int batchSize = 10000; // Số lượng tối đa trong mỗi danh sách con
                int numBatches = (int)Math.Ceiling((double)lstProduct.Count / batchSize);

                for (int i = 0; i < numBatches; i++)
                {
                    var batch = lstProduct
                        .Skip(i * batchSize)
                        .Take(batchSize)
                        .ToList();
                    try
                    {
                        var bulkConfig = new BulkConfig()
                        {

                            PropertiesToExcludeOnUpdate = new List<string> { nameof(model.listing_id) }

                        };

                        await _context.BulkInsertOrUpdateAsync(batch, bulkConfig);
                    }
                    catch (Exception ex)
                    {

                        await _logService.CreateLogError($"Insert Error {ex.ToString()} to Database Esty");
                        continue;
                    }
                }
                await _logService.CreateLogInformation($"Insert {lstProduct.Count} to Database Esty");
            }
            return true;
        }
        public async Task<bool> InsertSQL<T>(List<T> lstProduct) where T : class
        {
            if (lstProduct.Count > 0)
            {
                int batchSize = 10000; // Số lượng tối đa trong mỗi danh sách con
                int numBatches = (int)Math.Ceiling((double)lstProduct.Count / batchSize);

                for (int i = 0; i < numBatches; i++)
                {
                    var batch = lstProduct
                        .Skip(i * batchSize)
                        .Take(batchSize)
                        .ToList();

                    try
                    {

                        await _context.BulkInsertOrUpdateAsync(batch);
                    }
                    catch (Exception ex)
                    {
                        await _logService.CreateLogError($"Insert Error {ex.ToString()} to Database Esty");
                        continue;
                    }
                }
            }
            return true;
        }
        public async Task<bool> UpdateSQLImage<T>(List<T> lstProduct) where T : class
        {
            if (lstProduct.Count > 0)
            {
                int batchSize = 10000; // Số lượng tối đa trong mỗi danh sách con
                int numBatches = (int)Math.Ceiling((double)lstProduct.Count / batchSize);

                for (int i = 0; i < numBatches; i++)
                {
                    var batch = lstProduct
                        .Skip(i * batchSize)
                        .Take(batchSize)
                        .ToList();

                    try
                    {

                        await _context.BulkUpdateAsync(batch);
                    }
                    catch (Exception ex)
                    {
                        await _logService.CreateLogError($"Insert Error {ex.ToString()} to Database Esty");
                        continue;
                    }
                }
            }
            return true;
        }
    }
}
