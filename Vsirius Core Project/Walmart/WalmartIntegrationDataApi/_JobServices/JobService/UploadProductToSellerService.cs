using AutoMapper;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using WalmartIntegrationDataApi._JobServices.Interfaces;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Common.Helpers;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Dtos.SellerAuthen;
using WalmartIntegrationDataApi.Dtos.SellerItem;
using WalmartIntegrationDataApi.Dtos.Vsirius;
using WalmartIntegrationDataApi.Models;

namespace WalmartIntegrationDataApi._JobServices.JobService
{
    public class UploadProductToSellerService : IUploadProductToSellerService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly IInteractWalmartSellerCommonService _interactWalmart;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _updatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    "Storage", "Update", DateTime.Now.ToString("yyyyMMdd"));
        private readonly string _templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Template");
        private readonly string _insertPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Insert", DateTime.Now.ToString("yyyyMMdd"));
        private readonly string _deletePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Delete", DateTime.Now.ToString("yyyyMMdd"));
        private readonly ExcludedProductAPI._Services.Interfaces.IExcludedProductService _excludedProductService;


        public UploadProductToSellerService(
            IMapper mapper,
            IConfiguration config,
            ExcludedProductAPI._Services.Interfaces.IExcludedProductService excludedProductService,
            IInteractWalmartSellerCommonService interactWalmart,
            IServiceProvider serviceProvider)
        {
            _config = config;
            _mapper = mapper;
            _interactWalmart = interactWalmart;
            _serviceProvider = serviceProvider;
            _excludedProductService = excludedProductService;
        }

        public async Task UploadProductToSeller()
        {
            Util.WriteLog("---------------------------Upload Product To Seller Process Start-------------------------------------");
            try
            {
                List<ExcludedProductDto> lstExcluded = new();
                //Pull all data in walmart (seller)
                Util.WriteLog("GetDataSeller Start");
                List<Item> lstSeller = await _interactWalmart.GetItemsFromSeller(false, false);
                Util.WriteLog($"Total Item when get finish: {lstSeller.Count}");
                Util.WriteLog("GetDataSeller End");

                //Pull all data walmart (local)
                Util.WriteLog("GetDataLocal Start");
                List<Walmart> lstLocal = await GetDataFromLocal();
                List<Walmart> lstDoubleLocal = new();
                Parallel.ForEach(lstLocal, x =>
                {
                    decimal price = decimal.Parse(x.price);
                    if (price <= 7.5m)
                    {
                        x.price = (price + 8.5m).ToString();
                    }
                    var itemSeller = lstSeller.FirstOrDefault(a => a.sku == x.SKU);

                    if (x.productIdType == "UPC")
                    {
                        if (x.SKU.StartsWith("Q-"))
                        {
                            return;
                        }
                        if (x.SKU.StartsWith("QU-"))
                        {
                            if (x.productId == itemSeller?.upc)
                            {
                                return;
                            }
                            else
                            {
                                Walmart clone = Util.CloneObject(x);
                                clone.SKU = $"Q-{Util.SKUQgold(x.SKU)}";
                                if (!string.IsNullOrEmpty(x.variantGroupId))
                                {
                                    clone.variantGroupId = $"Q-{Util.GetOriginSKU(clone.variantGroupId)}";
                                }
                                lstDoubleLocal.Add(clone);

                                x.productIdType = "GTIN";
                                x.productId = string.IsNullOrEmpty(itemSeller?.gtin) ? "CUSTOM" : itemSeller.gtin;
                            }

                        }
                    }
                    else
                    {
                        x.productIdType = "GTIN";
                        x.productId = string.IsNullOrEmpty(itemSeller?.gtin) ? "CUSTOM" : itemSeller.gtin;
                    }

                });
                Util.WriteLog($"Total Item when get finish: {lstLocal.Count}");
                lstLocal.AddRange(lstDoubleLocal);
                Util.WriteLog("GetDataLocal End");


                //Lấy danh sách cần retire trong seller walmart điều kiện: (seller)
                //1. những sku trên seller có lifecycleStatus != RETIRED và vi phạm chính sách walmart
                var lstSkuLocal = lstLocal.Select(x => x.SKU).ToList();

                //list những sku phải retired vi phạm chính sách walmart(đã retired + chưa retired),
                List<string?> lstSkuMustRetired = new();

                //list những sku vi phạm chính sách walmart chưa retired nhưng cần retired 
                List<string?> lstSkuNeedRetired = new();

                //Lọc những sản phẩm cần xoá
                Parallel.ForEach(lstSeller, itemSeller =>
                {
                    var itemLocal = lstLocal.FirstOrDefault(x => x.SKU!.Equals(itemSeller.sku));
                    if (itemLocal != null)
                    {
                        //Xoá những sản phẩm trong seller có UPC nhưng khác tên với trong db
                        if (itemLocal.productIdType == "UPC" && itemLocal.productId == itemSeller.upc)
                        {
                            if (!Util.IsSimilar(itemLocal.productName, itemSeller.productName))
                            {
                                lstSkuMustRetired.Add(itemSeller.sku);
                                if (itemSeller.lifecycleStatus != "RETIRED")
                                {
                                    lstSkuNeedRetired.Add(itemSeller.sku);
                                }
                                return;
                            }
                        }

                        //Xoá những sản phẩm vi phạm chính sách
                        if (
                            itemSeller.unpublishedReasons is not null &&
                            itemSeller.unpublishedReasons.reason!.Any(a => a.Contains("code BIZ", StringComparison.OrdinalIgnoreCase)
                            || a.Contains("code COMP", StringComparison.OrdinalIgnoreCase))
                        )
                        {
                            lstExcluded.Add(new ExcludedProductDto()
                            {
                                MarketplaceSKU = itemSeller.sku,
                                Marketplace = "Walmart",
                                ExclusionType = ExclusionType.Deleted.ToString(),
                                ExclusionNote = string.Join("|", itemSeller.unpublishedReasons.reason!),
                                SupplierSKU = Util.GetOriginSKU(itemSeller.sku)
                            });
                            lstSkuMustRetired.Add(itemSeller.sku);
                            if (itemSeller.lifecycleStatus != "RETIRED")
                            {
                                lstSkuNeedRetired.Add(itemSeller.sku);
                            }
                        }
                    }
                    //itemSeller khong co trong local
                    else
                    {
                        lstSkuMustRetired.Add(itemSeller.sku);
                        if (itemSeller.lifecycleStatus != "RETIRED")
                        {
                            lstSkuNeedRetired.Add(itemSeller.sku);
                        }
                    }
                });

                //Gọi api seller để retire (seller)
                Util.WriteLog("Retire Item In Seller Start");
                if (lstSkuNeedRetired != null)
                {
                    if (lstSkuNeedRetired.Count > 0)
                    {
                        Util.WriteLog($"Retired {lstSkuNeedRetired.Count} items");
                        await RetireMutipleItems(lstSkuNeedRetired);
                    }
                }
                else
                {
                    Util.WriteLog("No item need retired");
                }
                Util.WriteLog("Retire Item In Seller End");

                //Gọi api local excluded để loại trừ sản phẩm bị cấm bán trên walmart
                Util.WriteLog("Start call excluded product API");
                if (lstExcluded.Any())
                {
                    await BulkUpdateInsertExcludedProduct(lstExcluded);
                }
                Util.WriteLog("End call excluded product API");

                //Lấy danh sách cần import vào seller điều kiện: (local)
                //1. những sku có trong local nhưng chưa có trên walmart seller
                List<string?> lstSkuSeller = lstSeller.Select(x => x.sku).ToList();
                var lstImport = lstLocal.Where(x => !lstSkuSeller.Contains(x.SKU)
                && !lstSkuMustRetired.Contains(x.SKU)).ToList();

                Util.WriteLog("Export Excel To Bulk Insert Item In Seller Start");
                await ExportExcelToBulkInsert(lstImport);
                Util.WriteLog("Export Excel To Bulk Insert Item In Seller End");

                //Gọi API Import với MP_ITEM (seller)
                Util.WriteLog("Bulk Insert Item In Seller Start");
                await BulkInsertItem();
                Util.WriteLog("Bulk Insert Item In Seller End");

                //Lấy danh sách cần update với điều kiện: (local) thường là những sản phẩm bị lỗi cần update
                //1. những sku trên seller có trạng thái publishedStatus != PUBLISHED
                //2. những sku có trong seller lẫn local
                List<string?> lstSkuSellerError =
                    lstSeller.Where(x => x.publishedStatus != "PUBLISHED")
                    .Select(x => x.sku)
                    .ToList();

                var lstVariation = lstLocal.Where(x =>
                !string.IsNullOrEmpty(x.variantGroupId)
                && !lstSkuMustRetired.Contains(x.SKU)
                ).ToList();

                var lstUpdate = lstLocal.Where(x =>
                lstSkuSellerError.Contains(x.SKU)
                && !lstSkuMustRetired.Contains(x.SKU)
                ).ToList();

                lstUpdate.AddRange(lstVariation);
                lstUpdate = lstUpdate.Distinct().ToList();

                Util.WriteLog("Export Excel To Bulk Update Item In Seller Start");

                foreach (var item in lstSeller)
                {
                    try
                    {
                        var objLocal = lstLocal.FirstOrDefault(x => x.SKU == item.sku);
                        if (objLocal != null)
                        {
                            if (!objLocal.productName.Equals(item.productName, StringComparison.OrdinalIgnoreCase) &&
                                objLocal.productIdType.Equals("GTIN") && objLocal.SKU.StartsWith("QU-"))
                            {
                                if (lstUpdate.Any(x => x.SKU.Equals(item.sku)) == false)
                                {
                                    lstUpdate.Add(objLocal);
                                    continue;
                                }
                            }
                        }
                        //if (item.unpublishedReasons != null && item.unpublishedReasons.reason!.Any(a => a.Contains("an acceptable range")))
                        //{
                        //    var objUpdate = lstUpdate.FirstOrDefault(x => x.SKU.Equals(item.sku));
                        //    if (objUpdate != null)
                        //    {
                        //        decimal price = (decimal)item.price!.amount!;
                        //        price -= (price * 5m / 100m);
                        //        price = Math.Round(price, 2);
                        //        objUpdate.price = price.ToString();
                        //    }
                        //}
                    }
                    catch (Exception ex)
                    {
                        Util.WriteLog($"Exception in foreach: {ex.Message}");
                        continue;
                    }
                }
                await ExportExcelToBulkUpdate(lstUpdate);
                Util.WriteLog("Export Excel To Bulk Update Item In Seller End");

                //Gọi API Update với MP_MAINTAIN (seller)
                Util.WriteLog("Bulk Update Item In Seller Start");
                await BulkUpdateItem();
                Util.WriteLog("Bulk Update Item In Seller End");

                lstSeller = null;
                lstLocal = null;
                lstSkuLocal = null;
                lstSkuMustRetired = null;
                lstSkuNeedRetired = null;
                lstSkuSeller = null;
                lstImport = null;
                lstSkuSellerError = null;
                lstUpdate = null;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Exception when process run: {ex.Message}");
            }

            Util.WriteLog("---------------------------Upload Product To Seller Process End-------------------------------------");
        }

        private async Task BulkUpdateInsertExcludedProduct(List<ExcludedProductDto> lstExcluded)
        {
            try
            {
                var batches = Util.SplitListIntoBatches(lstExcluded, 5000);

                foreach (var batch in batches)
                {
                    await CallApiToCreateUpdateExcludedProduct_RFT(batch);
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"Error exception: {ex.Message}");
            }
        }

        private async Task CallApiToCreateUpdateExcludedProduct(List<ExcludedProductDto> batch)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = _config.GetValue<string>("APIExcludedProduct:BulkCreateUpdate");
                var postData = new StringContent(JsonConvert.SerializeObject(batch), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, postData);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    Util.WriteLog("Response from server: " + responseBody);
                }
                else
                {
                    Util.WriteLog("Error occurred, status code: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Error exception: {ex.Message}");
            }
        }

        private async Task CallApiToCreateUpdateExcludedProduct_RFT(List<ExcludedProductDto> batch)
        {
            var list = new List<ExcludedProductAPI.DTOs.ExcludedProductDTO>();
            foreach (var item in batch)
            {
                var dto = _mapper.Map<ExcludedProductAPI.DTOs.ExcludedProductDTO>(item);
                list.Add(dto);
            }
            var result = await _excludedProductService.BulkCreateUpdate(list);
            Util.WriteLog("Response from service: " + result);
        }

        private async Task BulkUpdateItem()
        {
            try
            {
                var fileInfos = GetNewFileToUpload(isUpdate: true);
                if (fileInfos == null)
                {
                    Util.WriteLog($"Can not find file to bulk update item");
                }
                else
                {
                    for (int i = 0; i < fileInfos.Count; i++)
                    {
                        int currentIndexQuotaAPI = i + 1;
                        var item = fileInfos[i];

                        await UploadSingleFileAsync(item, isUpdate: true);

                        if (currentIndexQuotaAPI % 4 == 0 && currentIndexQuotaAPI != fileInfos.Count)
                        {
                            await Task.Delay(TimeSpan.FromHours(4));
                        }
                        await Task.Delay(TimeSpan.FromSeconds(2));
                    }
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Bulk Update failed with exception {ex.Message}");
            }
        }

        private async Task ExportExcelToBulkUpdate(List<Walmart> lstUpdate)
        {
            try
            {
                string templatePath = Path.Combine(_templatePath, "UpdateWalmartProductTemplate.xlsx");
                Util.CreateFolder(_updatePath);
                int batchSize = 10000;
                int totalItems = lstUpdate.Count;
                int totalBatches = (int)Math.Ceiling((double)totalItems / batchSize);
                for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
                {
                    var package = new ExcelPackage(new FileInfo(templatePath));
                    var ws = package.Workbook.Worksheets["Jewelry"];
                    int startRow = 7;
                    var batch = lstUpdate.Skip(batchSize * batchIndex).Take(batchSize).ToList();
                    foreach (var item in batch)
                    {
                        ws.Cells["D" + startRow].Value = item.SKU;
                        ws.Cells["E" + startRow].Value = item.productIdType;
                        ws.Cells["F" + startRow].Value = item.productId;
                        ws.Cells["G" + startRow].Value = item.externalProductIdType;
                        ws.Cells["H" + startRow].Value = item.externalProductId;
                        ws.Cells["I" + startRow].Value = item.productName;
                        ws.Cells["J" + startRow].Value = item.brand;
                        ws.Cells["K" + startRow].Value = item.pricePerUnitQuantity;
                        ws.Cells["L" + startRow].Value = item.pricePerUnitUom;
                        ws.Cells["M" + startRow].Value = item.multipackQuantity;
                        ws.Cells["N" + startRow].Value = item.shortDescription;

                        string[] keyFeatures = item.keyFeatures?.Split("|") ?? new string[0];
                        ws.Cells["O" + startRow].Value = GetElementOrDefault(keyFeatures, 0);
                        ws.Cells["P" + startRow].Value = GetElementOrDefault(keyFeatures, 1);
                        ws.Cells["Q" + startRow].Value = GetElementOrDefault(keyFeatures, 2);

                        ws.Cells["R" + startRow].Value = item.mainImageUrl;

                        string[] secondaryImageURLs = item.productSecondaryImageURL?.Split("|") ?? new string[0];
                        ws.Cells["S" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 0);
                        ws.Cells["T" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 1);
                        ws.Cells["U" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 2);
                        ws.Cells["V" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 3);

                        ws.Cells["W" + startRow].Value = item.electronicsIndicator;
                        ws.Cells["X" + startRow].Value = item.batteryTechnologyType;
                        ws.Cells["Y" + startRow].Value = item.chemicalAerosolPesticide;
                        ws.Cells["Z" + startRow].Value = item.prop65WarningText;
                        ws.Cells["AA" + startRow].Value = item.price;
                        //ws.Cells["AB" + startRow].Value = item.startDate;
                        ws.Cells["AB" + startRow].Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                        //ws.Cells["AC" + startRow].Value = item.endDate;
                        ws.Cells["AC" + startRow].Value = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                        ws.Cells["AD" + startRow].Value = item.additionalOfferAttributeName;
                        ws.Cells["AE" + startRow].Value = item.additionalOfferAttributeValue;
                        ws.Cells["AF" + startRow].Value = item.msrp;
                        ws.Cells["AG" + startRow].Value = item.shipsInOriginalPackaging;
                        ws.Cells["AH" + startRow].Value = item.MustShipAlone;
                        ws.Cells["AI" + startRow].Value = item.ShippingWeight;
                        ws.Cells["AJ" + startRow].Value = item.stateRestrictionsText;
                        ws.Cells["AK" + startRow].Value = item.states;
                        ws.Cells["AL" + startRow].Value = item.zipCodes;
                        ws.Cells["AM" + startRow].Value = item.smallPartsWarnings;
                        ws.Cells["AN" + startRow].Value = item.gender;
                        ws.Cells["AO" + startRow].Value = item.size;
                        ws.Cells["AP" + startRow].Value = item.ageGroup;
                        ws.Cells["AQ" + startRow].Value = item.manufacturer;
                        ws.Cells["AR" + startRow].Value = item.manufacturerPartNumber;
                        ws.Cells["AS" + startRow].Value = item.modelNumber;
                        ws.Cells["AT" + startRow].Value = item.countPerPack;
                        ws.Cells["AU" + startRow].Value = item.count;
                        ws.Cells["AV" + startRow].Value = item.color;
                        ws.Cells["AW" + startRow].Value = item.colorCategory;
                        ws.Cells["AX" + startRow].Value = item.ringStyle;
                        ws.Cells["AY" + startRow].Value = item.ringSize;
                        ws.Cells["AZ" + startRow].Value = item.gemstone;
                        ws.Cells["BA" + startRow].Value = item.secondaryStoneType;
                        ws.Cells["BB" + startRow].Value = item.accentStone;
                        ws.Cells["BC" + startRow].Value = item.numberOfGemstones;
                        ws.Cells["BD" + startRow].Value = item.chainPattern;
                        ws.Cells["BE" + startRow].Value = item.measureChain;
                        ws.Cells["BF" + startRow].Value = item.unitChain;
                        ws.Cells["BG" + startRow].Value = item.claspType;
                        ws.Cells["BH" + startRow].Value = item.earringStyle;
                        ws.Cells["BI" + startRow].Value = item.measureEar;
                        ws.Cells["BJ" + startRow].Value = item.unitEar;
                        ws.Cells["BK" + startRow].Value = item.backFinding;
                        ws.Cells["BL" + startRow].Value = item.braceletStyle;
                        ws.Cells["BM" + startRow].Value = item.necklaceStyle;
                        ws.Cells["BN" + startRow].Value = item.bodyJewelryStyle;
                        ws.Cells["BO" + startRow].Value = item.braceletCharmStyle;
                        ws.Cells["BP" + startRow].Value = item.jewelrySetting;
                        ws.Cells["BQ" + startRow].Value = item.certifyingAgent;
                        ws.Cells["BR" + startRow].Value = item.diamondClarity;
                        ws.Cells["BS" + startRow].Value = item.diamondColor;
                        ws.Cells["BT" + startRow].Value = item.diamondCut;
                        ws.Cells["BU" + startRow].Value = item.measurePrimary;
                        ws.Cells["BV" + startRow].Value = item.unitPrimary;
                        ws.Cells["BW" + startRow].Value = item.measureTotal;
                        ws.Cells["BX" + startRow].Value = item.unitTotal;
                        ws.Cells["BY" + startRow].Value = item.stoneTreatment;
                        ws.Cells["BZ" + startRow].Value = item.stoneCreationMethod;
                        ws.Cells["CA" + startRow].Value = item.gemstoneShape;
                        ws.Cells["CB" + startRow].Value = item.pearlType;
                        ws.Cells["CC" + startRow].Value = item.measureSize;
                        ws.Cells["CD" + startRow].Value = item.unitSize;
                        ws.Cells["CE" + startRow].Value = item.isResizable;
                        ws.Cells["CF" + startRow].Value = item.personalRelationship;
                        ws.Cells["CG" + startRow].Value = item.metal;
                        ws.Cells["CH" + startRow].Value = item.metalPurity;
                        ws.Cells["CI" + startRow].Value = item.material;
                        ws.Cells["CJ" + startRow].Value = item.religion;
                        ws.Cells["CK" + startRow].Value = item.birthstone;
                        ws.Cells["CL" + startRow].Value = item.style;
                        ws.Cells["CM" + startRow].Value = item.shape;
                        ws.Cells["CN" + startRow].Value = item.occasion;
                        ws.Cells["CO" + startRow].Value = item.bodyParts;
                        ws.Cells["CP" + startRow].Value = item.isMadeFromRecycledMaterial;
                        ws.Cells["CQ" + startRow].Value = item.recycledMaterial;
                        ws.Cells["CR" + startRow].Value = item.percentageOfRecycledMaterial;
                        ws.Cells["CS" + startRow].Value = item.theme;
                        ws.Cells["CT" + startRow].Value = item.measureDiameter;
                        ws.Cells["CU" + startRow].Value = item.unitDiameter;
                        ws.Cells["CV" + startRow].Value = item.character;
                        ws.Cells["CW" + startRow].Value = item.sportsTeam;
                        ws.Cells["CX" + startRow].Value = item.sportsLeague;
                        ws.Cells["CY" + startRow].Value = item.warrantyText;
                        ws.Cells["CZ" + startRow].Value = item.warrantyURL;
                        ws.Cells["DA" + startRow].Value = item.productAttributeName;
                        ws.Cells["DB" + startRow].Value = item.productAttributeValue;
                        ws.Cells["DC" + startRow].Value = item.seasonYear;
                        ws.Cells["DD" + startRow].Value = item.seasonCode;
                        ws.Cells["DE" + startRow].Value = item.variantGroupId;
                        ws.Cells["DF" + startRow].Value = item.removeVariantGroup;
                        ws.Cells["DG" + startRow].Value = item.variantAttributeNames;
                        ws.Cells["DH" + startRow].Value = item.isPrimaryVariant;
                        ws.Cells["DI" + startRow].Value = item.swatchVariantAttribute;
                        ws.Cells["DJ" + startRow].Value = item.swatchImageUrl;
                        ws.Cells["DK" + startRow].Value = (item.productIdType == "GTIN" ? item.productId : "");
                        ws.Cells["DL" + startRow].Value = (item.productIdType == "GTIN" ? "Y" : "");
                        ++startRow;
                    }
                    string filePath = Path.Combine(_updatePath, $"UpdateWalmartProduct_{batchIndex}.xlsx");
                    await package.SaveAsAsync(new FileInfo(filePath));
                    package.Dispose();
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"ExportExcelToBulkInsert failed Err: {ex.Message}");
            }
        }

        private async Task BulkInsertItem()
        {
            try
            {
                var fileInfos = GetNewFileToUpload(isUpdate: false);
                if (fileInfos == null)
                {
                    Util.WriteLog($"Can not find file to bulk insert item");
                }
                else
                {
                    for (int i = 0; i < fileInfos.Count; i++)
                    {
                        var currentIndexQuotaAPI = i + 1;
                        var item = fileInfos[i];
                        await UploadSingleFileAsync(item, isUpdate: false);
                        if (currentIndexQuotaAPI % 4 == 0 && currentIndexQuotaAPI != fileInfos.Count)
                        {
                            await Task.Delay(TimeSpan.FromHours(4));
                        }
                        await Task.Delay(TimeSpan.FromSeconds(10));

                    }
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"Bulk Insert failed with exception {ex.Message}");
            }
        }

        private async Task UploadSingleFileAsync(FileInfo fileInfo, bool isUpdate)
        {
            try
            {
                TokenResult tokenResult = await _interactWalmart.GetTokenFromWalmartSeller();

                //var feedType = isUpdate ? "MP_MAINTENANCE" : "item";
                //var feedType = isUpdate ? "MP_MAINTENANCE" : "MP_ITEM";
                var feedType = "MP_MAINTENANCE";

                if (!string.IsNullOrEmpty(tokenResult.AccessToken))
                {
                    var url = $"{_config.GetValue<string>("Walmart:BulkItemWalmart")}?feedType={feedType}";
                    using HttpClient client = new();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                    client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);
                    client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                        $"multipart/form-data; boundary={tokenResult.CorrelationId}");

                    var formData = new MultipartFormDataContent();
                    using (StreamContent fileContent = new StreamContent(fileInfo.OpenRead()))
                    {
                        fileContent.Headers.ContentType =
                            new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        formData.Add(fileContent, "file", fileInfo.Name);
                        var request = new HttpRequestMessage(HttpMethod.Post, url);
                        request.Content = formData;
                        HttpResponseMessage response = await client.SendAsync(request);
                        if (response.IsSuccessStatusCode)
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            var result = JsonConvert.DeserializeObject<ResultFeed>(responseBody);
                            string feedId = result!.FeedId;
                            Util.WriteLog($"Upload Success File: {fileInfo.Name}. FeedId: {feedId}");
                        } else
                        {
                            string responseBody = await response.Content.ReadAsStringAsync();
                            Util.WriteLog($"Upload Failed File: {fileInfo.Name}. Status: {response.StatusCode}. Message: {responseBody}");
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Upload Failed File {fileInfo.Name}.Err: {ex.Message}");
            }

        }

        private List<FileInfo>? GetNewFileToUpload(bool isUpdate)
        {
            List<FileInfo>? excelFiles = null;
            if (isUpdate)
            {
                excelFiles = new DirectoryInfo(_updatePath).GetFiles()
                .Where(f => Regex.IsMatch(f.Name, @"^.+\.(xlsx|xls|csv)$")).OrderBy(f => f.CreationTime).ToList();
            }
            else
            {
                excelFiles = new DirectoryInfo(_insertPath).GetFiles()
                .Where(f => Regex.IsMatch(f.Name, @"^.+\.(xlsx|xls|csv)$")).OrderBy(f => f.CreationTime).ToList();
            }

            return excelFiles;

        }

        private async Task ExportExcelToBulkInsert(List<Walmart> lstImport)
        {
            try
            {
                string templatePath = Path.Combine(_templatePath, "InsertWalmartProductTemplate.xlsx");
                Util.CreateFolder(_insertPath);

                int batchSize = 10000;
                int totalItems = lstImport.Count;
                int totalBatches = (int)Math.Ceiling((double)totalItems / batchSize);
                for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
                {
                    var package = new ExcelPackage(new FileInfo(templatePath));
                    var ws = package.Workbook.Worksheets["Jewelry"];
                    int startRow = 7;
                    var batch = lstImport.Skip(batchSize * batchIndex).Take(batchSize).ToList();
                    foreach (var item in batch)
                    {
                        ws.Cells["D" + startRow].Value = item.SKU;
                        ws.Cells["E" + startRow].Value = item.productIdType;
                        ws.Cells["F" + startRow].Value = item.productId;
                        ws.Cells["G" + startRow].Value = item.externalProductIdType;
                        ws.Cells["H" + startRow].Value = item.externalProductId;
                        ws.Cells["I" + startRow].Value = item.productName;
                        ws.Cells["J" + startRow].Value = item.brand;
                        ws.Cells["K" + startRow].Value = item.pricePerUnitQuantity;
                        ws.Cells["L" + startRow].Value = item.pricePerUnitUom;
                        ws.Cells["M" + startRow].Value = item.multipackQuantity;
                        ws.Cells["N" + startRow].Value = item.shortDescription;

                        string[] keyFeatures = item.keyFeatures?.Split("|") ?? new string[0];
                        ws.Cells["O" + startRow].Value = GetElementOrDefault(keyFeatures, 0);
                        ws.Cells["P" + startRow].Value = GetElementOrDefault(keyFeatures, 1);
                        ws.Cells["Q" + startRow].Value = GetElementOrDefault(keyFeatures, 2);

                        ws.Cells["R" + startRow].Value = item.mainImageUrl;

                        string[] secondaryImageURLs = item.productSecondaryImageURL?.Split("|") ?? new string[0];
                        ws.Cells["S" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 0);
                        ws.Cells["T" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 1);
                        ws.Cells["U" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 2);
                        ws.Cells["V" + startRow].Value = GetElementOrDefault(secondaryImageURLs, 3);

                        ws.Cells["W" + startRow].Value = item.electronicsIndicator;
                        ws.Cells["X" + startRow].Value = item.batteryTechnologyType;
                        ws.Cells["Y" + startRow].Value = item.chemicalAerosolPesticide;
                        ws.Cells["Z" + startRow].Value = item.prop65WarningText;
                        ws.Cells["AA" + startRow].Value = item.price;
                        //ws.Cells["AB" + startRow].Value = item.startDate;
                        //ws.Cells["AC" + startRow].Value = item.endDate; 
                        ws.Cells["AB" + startRow].Value = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                        ws.Cells["AC" + startRow].Value = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
                        ws.Cells["AD" + startRow].Value = item.additionalOfferAttributeName;
                        ws.Cells["AE" + startRow].Value = item.additionalOfferAttributeValue;
                        ws.Cells["AF" + startRow].Value = item.msrp;
                        ws.Cells["AG" + startRow].Value = item.shipsInOriginalPackaging;
                        ws.Cells["AH" + startRow].Value = item.MustShipAlone;
                        ws.Cells["AI" + startRow].Value = item.ShippingWeight;
                        ws.Cells["AJ" + startRow].Value = item.stateRestrictionsText;
                        ws.Cells["AK" + startRow].Value = item.states;
                        ws.Cells["AL" + startRow].Value = item.zipCodes;
                        ws.Cells["AM" + startRow].Value = item.smallPartsWarnings;
                        ws.Cells["AN" + startRow].Value = item.gender;
                        ws.Cells["AO" + startRow].Value = item.size;
                        ws.Cells["AP" + startRow].Value = item.ageGroup;
                        ws.Cells["AQ" + startRow].Value = item.manufacturer;
                        ws.Cells["AR" + startRow].Value = item.manufacturerPartNumber;
                        ws.Cells["AS" + startRow].Value = item.modelNumber;
                        ws.Cells["AT" + startRow].Value = item.countPerPack;
                        ws.Cells["AU" + startRow].Value = item.count;
                        ws.Cells["AV" + startRow].Value = item.color;
                        ws.Cells["AW" + startRow].Value = item.colorCategory;
                        ws.Cells["AX" + startRow].Value = item.ringStyle;
                        ws.Cells["AY" + startRow].Value = item.ringSize;
                        ws.Cells["AZ" + startRow].Value = item.gemstone;
                        ws.Cells["BA" + startRow].Value = item.secondaryStoneType;
                        ws.Cells["BB" + startRow].Value = item.accentStone;
                        ws.Cells["BC" + startRow].Value = item.numberOfGemstones;
                        ws.Cells["BD" + startRow].Value = item.chainPattern;
                        ws.Cells["BE" + startRow].Value = item.measureChain;
                        ws.Cells["BF" + startRow].Value = item.unitChain;
                        ws.Cells["BG" + startRow].Value = item.claspType;
                        ws.Cells["BH" + startRow].Value = item.earringStyle;
                        ws.Cells["BI" + startRow].Value = item.measureEar;
                        ws.Cells["BJ" + startRow].Value = item.unitEar;
                        ws.Cells["BK" + startRow].Value = item.backFinding;
                        ws.Cells["BL" + startRow].Value = item.braceletStyle;
                        ws.Cells["BM" + startRow].Value = item.necklaceStyle;
                        ws.Cells["BN" + startRow].Value = item.bodyJewelryStyle;
                        ws.Cells["BO" + startRow].Value = item.braceletCharmStyle;
                        ws.Cells["BP" + startRow].Value = item.jewelrySetting;
                        ws.Cells["BQ" + startRow].Value = item.certifyingAgent;
                        ws.Cells["BR" + startRow].Value = item.diamondClarity;
                        ws.Cells["BS" + startRow].Value = item.diamondColor;
                        ws.Cells["BT" + startRow].Value = item.diamondCut;
                        ws.Cells["BU" + startRow].Value = item.measurePrimary;
                        ws.Cells["BV" + startRow].Value = item.unitPrimary;
                        ws.Cells["BW" + startRow].Value = item.measureTotal;
                        ws.Cells["BX" + startRow].Value = item.unitTotal;
                        ws.Cells["BY" + startRow].Value = item.stoneTreatment;
                        ws.Cells["BZ" + startRow].Value = item.stoneCreationMethod;
                        ws.Cells["CA" + startRow].Value = item.gemstoneShape;
                        ws.Cells["CB" + startRow].Value = item.pearlType;
                        ws.Cells["CC" + startRow].Value = item.measureSize;
                        ws.Cells["CD" + startRow].Value = item.unitSize;
                        ws.Cells["CE" + startRow].Value = item.isResizable;
                        ws.Cells["CF" + startRow].Value = item.personalRelationship;
                        ws.Cells["CG" + startRow].Value = item.metal;
                        ws.Cells["CH" + startRow].Value = item.metalPurity;
                        ws.Cells["CI" + startRow].Value = item.material;
                        ws.Cells["CJ" + startRow].Value = item.religion;
                        ws.Cells["CK" + startRow].Value = item.birthstone;
                        ws.Cells["CL" + startRow].Value = item.style;
                        ws.Cells["CM" + startRow].Value = item.shape;
                        ws.Cells["CN" + startRow].Value = item.occasion;
                        ws.Cells["CO" + startRow].Value = item.bodyParts;
                        ws.Cells["CP" + startRow].Value = item.isMadeFromRecycledMaterial;
                        ws.Cells["CQ" + startRow].Value = item.recycledMaterial;
                        ws.Cells["CR" + startRow].Value = item.percentageOfRecycledMaterial;
                        ws.Cells["CS" + startRow].Value = item.theme;
                        ws.Cells["CT" + startRow].Value = item.measureDiameter;
                        ws.Cells["CU" + startRow].Value = item.unitDiameter;
                        ws.Cells["CV" + startRow].Value = item.character;
                        ws.Cells["CW" + startRow].Value = item.sportsTeam;
                        ws.Cells["CX" + startRow].Value = item.sportsLeague;
                        ws.Cells["CY" + startRow].Value = item.warrantyText;
                        ws.Cells["CZ" + startRow].Value = item.warrantyURL;
                        ws.Cells["DA" + startRow].Value = item.productAttributeName;
                        ws.Cells["DB" + startRow].Value = item.productAttributeValue;
                        ws.Cells["DC" + startRow].Value = item.seasonYear;
                        ws.Cells["DD" + startRow].Value = item.seasonCode;
                        ws.Cells["DE" + startRow].Value = item.variantGroupId;
                        ws.Cells["DF" + startRow].Value = item.removeVariantGroup;
                        ws.Cells["DG" + startRow].Value = item.variantAttributeNames;
                        ws.Cells["DH" + startRow].Value = item.isPrimaryVariant;
                        ws.Cells["DI" + startRow].Value = item.swatchVariantAttribute;
                        ws.Cells["DJ" + startRow].Value = item.swatchImageUrl;
                        ws.Cells["DK" + startRow].Value = (item.productIdType == "GTIN" ? item.productId : "");
                        ws.Cells["DL" + startRow].Value = (item.productIdType == "GTIN" ? "Y" : "");
                        ++startRow;
                    }
                    string filePath = Path.Combine(_insertPath, $"InsertWalmartProduct_{batchIndex}.xlsx");
                    await package.SaveAsAsync(new FileInfo(filePath));
                    package.Dispose();
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"ExportExcelToBulkInsert failed. Err: {ex.Message}");
            }
        }

        private string GetElementOrDefault(string[] array, int index)
        {
            if (index >= 0 && index < array.Length)
            {
                return array[index];
            }
            else
            {
                return "";
            }
        }

        public async Task SaveAndChangeSKU(List<string> lstSKUWrongUPC)
        {
            var param = new List<QgoldWalmartSKUParam>();

            foreach (var sku in lstSKUWrongUPC)
            {
                if (sku.StartsWith("Q"))
                {
                    var skuQgold = Util.SKUQgold(sku);
                    param.Add(new QgoldWalmartSKUParam()
                    {
                        SKUQgold = skuQgold,
                        SKUWalmart = $"QU-{skuQgold}",
                    });
                }
            }

            var batchs = Util.SplitListIntoBatches(param, 500);

            foreach (var batch in batchs)
            {
                var client = new HttpClient();

                var response = await client.PostAsync(
                    _config.GetValue<string>("SkuMappingAPI:BulkSkuWalmartQgold"),
                    new StringContent(JsonConvert.SerializeObject(batch), Encoding.UTF8, "application/json")
                    );
                if (!response.IsSuccessStatusCode)
                {
                    Util.WriteLog($"SaveAndChangeSKU failed with err:\n {response.Content}");
                }
            }
        }

        private async Task RetireMutipleItems(List<string?> lstSkuRetire)
        {
            TokenResult tokenResult = await _interactWalmart.GetTokenFromWalmartSeller();

            var batchs = Util.SplitListIntoBatches(lstSkuRetire, 10_000);

            for (int i = 0; i < batchs.Count; i++)
            {
                await RetireBulkItemWalmart(batchs[i], tokenResult);
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }

        private async Task RetireBulkItemWalmart(List<string?> lstSkuRetire, TokenResult tokenResult)
        {
            try
            {
                var fileInfo = await WriteJsonFileToDelete(lstSkuRetire);

                await Task.Delay(TimeSpan.FromSeconds(5));

                var url = $"{_config.GetValue<string>("Walmart:BulkItemWalmart")}?feedType=RETIRE_ITEM";
                using HttpClient client = new();
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("WM_SEC.ACCESS_TOKEN", tokenResult.AccessToken);
                client.DefaultRequestHeaders.Add("WM_QOS.CORRELATION_ID", tokenResult.CorrelationId);
                client.DefaultRequestHeaders.Add("WM_SVC.NAME", "Walmart Marketplace");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type",
                        $"multipart/form-data; boundary={tokenResult.CorrelationId}");

                var formData = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileInfo.OpenRead());
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                formData.Add(fileContent, "file");

                HttpResponseMessage response = await client.PostAsync(url, formData);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<ResultFeed>(responseBody);
                    string feedId = result!.FeedId;
                    Util.WriteLog($"Retired success File {fileInfo.Name}. FeedId: {feedId}");
                }
                else
                {
                    Util.WriteLog($"Retired failed Exception: {response.ReasonPhrase}");
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"Retired failed Exception: {ex.Message}");
            }
        }

        private async Task<FileInfo> WriteJsonFileToDelete(List<string?> lstSkuRetire)
        {
            try
            {
                Util.CreateFolder(_deletePath);
                var objRetire = new DeleteSKU()
                {
                    RetireItemHeader = new RetireItemHeader()
                    {
                        feedDate = DateTime.Now,
                        version = "1.0"
                    },
                    RetireItem = lstSkuRetire.Select(x => new RetireItem()
                    {
                        sku = x
                    }).ToList()
                };
                var json = System.Text.Json.JsonSerializer.Serialize(objRetire);
                string filePath = Path.Combine(_deletePath, $"RetireWalmart_{DateTime.Now:hhMMssfff}.json");
                await File.WriteAllTextAsync(filePath, json);
                FileInfo fileInfo = new(filePath);
                return fileInfo;
            }
            catch (Exception ex)
            {
                Util.WriteLog($"WriteJsonFileToDelete. Err: {ex.Message}");
                throw;
            }
        }

        private async Task<List<Walmart>> GetDataFromLocal()
        {
            var lstData = new List<Walmart>();
            try
            {
                var pageSize = 5000;
                var dataFirstCall = await GetPaginationLocalResult(1, pageSize);
                var totalPages = 0;
                List<Walmart> firstData = new();
                //first data
                if (dataFirstCall != null)
                {
                    firstData = dataFirstCall.Result;
                    totalPages = dataFirstCall.Pagination.TotalPage;
                    lstData.AddRange(firstData!);
                    Util.WriteLog($"Total pages: {totalPages}");
                }
                else
                {
                    return lstData;
                }

                //List rest data
                List<Task<PaginationUtility<Walmart>?>> tasks = new();
                int amountRunTask = 0;
                for (int i = 2; i <= totalPages; i++)
                {
                    int pageNumber = i;

                    tasks.Add(GetPaginationLocalResult(pageNumber, pageSize));

                    if (amountRunTask < 5 && i != totalPages)
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
                                lstData.AddRange(result.Result);
                            }
                            else
                            {
                                Util.WriteLog($"Failed when add record");
                            }
                        }
                        tasks.RemoveAll(x => x.IsCompleted);
                    }
                }

            }
            catch (Exception ex)
            {
                Util.WriteLog($"GetDataFromLocal. Error :{ex.Message}");
            }
            return lstData;
        }

        private async Task<PaginationUtility<Walmart>?> GetPaginationLocalResult(int pageNumber, int pageSize)
        {
            PaginationUtility<Walmart>? result = null;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<IProductWalmartService>();
                    result = (PaginationUtility<Walmart>?)(await service.GetAll(new PaginationParam()
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    })).Data;
                }
            }
            catch (Exception ex)
            {
                Util.WriteLog($"Get product walmart. Exception: {ex.Message}");
                throw;
            }

            return result;
        }
    }
}


