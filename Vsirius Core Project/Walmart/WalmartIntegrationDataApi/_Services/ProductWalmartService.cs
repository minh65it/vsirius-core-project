using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WalmartIntegrationDataApi.Data;
using WalmartIntegrationDataApi.Models;
using System.Reflection;
using OfficeOpenXml;
using EFCore.BulkExtensions;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Common.Helpers;

namespace WalmartIntegrationDataApi._Services
{
    public class ProductWalmartService : IProductWalmartService
    {
        private readonly WalmartDbContext _context;
        private OperationResult _result;
        private readonly IMapper _mapper;
        private readonly IValidation _validatation;
        private readonly IWebHostEnvironment _env;

        public ProductWalmartService(IMapper map,
            IValidationWithMasterExcel validationWithMasterExcel,
            IWebHostEnvironment hostEnvironment,
            WalmartDbContext context,
            OperationResult result)
        {
            _context = context;
            _validatation = validationWithMasterExcel;
            _mapper = map;
            _result = result;
            _env = hostEnvironment;
        }

        public async Task<OperationResult> GetAll(PaginationParam param)
        {
            var query = _context.Walmarts.AsNoTracking().Where(x => x.isDeleted == false);
            if (!string.IsNullOrWhiteSpace(param.KeySearch))
            {
                query = query.Where(x => x.productName.Equals(param.KeySearch));
            }
            _result.Data = await PaginationUtility<Walmart>.CreateAsync(query, param.PageNumber, param.PageSize);

            return _result;
        }

        public async Task<OperationResult> Create(WalmartDTO data)
        {
            var msgErr = _validatation.Validate(data);

            if (!IsAllPropertiesNull(msgErr))
            {
                _result.Data = msgErr;
                _result.IsSuccess = false;
                _result.Error = msgErr.Note!;
                return _result;
            }
            else
            {
                var newData = _mapper.Map<Walmart>(data);
                try
                {
                    await _context.Walmarts.AddAsync(newData);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _result.Error = ex.Message;
                    _result.IsSuccess = false;
                }
                return _result;
            }
        }

        public async Task<OperationResult> Update(WalmartDTO data)
        {
            var msgErr = _validatation.Validate(data);
            if (!IsAllPropertiesNull(msgErr))
            {
                _result.Data = msgErr;
                _result.IsSuccess = false;
                _result.Error = msgErr.Note!;
                return _result;
            }
            else
            {
                var existData = _mapper.Map<Walmart>(data);
                try
                {
                    _context.Walmarts.Update(existData);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _result.Error = ex.Message;
                    _result.IsSuccess = false;
                }
            }
            return _result;
        }

        public async Task<OperationResult> Delete(List<string> id)
        {
            var batchs = Util.SplitListIntoBatches(id, 100);
            foreach (var item in batchs)
            {
                await _context.Walmarts.Where(x => item.Contains(x.SKU))
                      .ExecuteUpdateAsync(s => s
                      .SetProperty(i => i.isDeleted, i => true));
            }
            return _result;
        }

        public async Task<OperationResult> Export(PaginationParam param)
        {
            var data = new List<Walmart>();
            var query = _context.Walmarts.Where(x => x.isDeleted != true);
            if (!string.IsNullOrWhiteSpace(param.KeySearch))
            {
                var searchKey = param.KeySearch;
                query = query.Where(x => x.SKU.Contains(searchKey)
                || x.shortDescription!.Contains(searchKey)
                || x.productName!.Contains(searchKey));
            }

            if (!string.IsNullOrWhiteSpace(param.BrandProduct))
            {
                query = query.Where(x => x.SKU.StartsWith(param.BrandProduct));
            }

            if (param.IsUpdate == true)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == curDate);
            }

            if (param.IsUpdate == false)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == null);
            }

            var result = await PaginationUtility<Walmart>.CreateAsync(query.AsNoTracking(), param.PageNumber, param.PageSize);
            _result.Message = result.Pagination.TotalPage.ToString();
            data = result.Result;
            string templatePath = Path.Combine(_env.ContentRootPath, "Files", "WalmartProductTemplate.xlsx");
            var package = new ExcelPackage(new FileInfo(templatePath));
            var ws = package.Workbook.Worksheets["Jewelry"];

            int startRow = 7;
            foreach (var item in data)
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
                ws.Cells["AB" + startRow].Value = item.startDate;
                ws.Cells["AC" + startRow].Value = item.endDate;
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
                ++startRow;
            }

            var stream = new MemoryStream();
            await package.SaveAsAsync(stream);
            byte[] bytes = stream.ToArray();

            _result.Data = bytes;

            return _result;
        }

        public async Task<OperationResult> ImportFile(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);
            string[] arrNameFile = new string[] {
                ".xlsx", ".xls", ".csv"
            };

            if (!arrNameFile.Contains(fileExtension))
            {
                _result.IsSuccess = false;
                _result.Error = "File not correct format!";
                return _result;
            }
            var stream = file.OpenReadStream();
            var package = new ExcelPackage(stream);

            // Access the first worksheet
            var worksheet = package.Workbook.Worksheets["Jewelry"];

            var lstSuccess = new List<WalmartDTO>();
            var lstFailed = new List<WalmartDTO>();

            for (int row = 7; row <= worksheet.Dimension.End.Row; row++)
            {
                if (worksheet.Cells["D" + row].Value == null)
                {
                    break;
                }
                var product = new WalmartDTO();
                product.SKU = worksheet.Cells["D" + row].Value != null ? worksheet.Cells["D" + row].Value.ToString() : "";
                product.productIdType = worksheet.Cells["E" + row].Value != null ? worksheet.Cells["E" + row].Value.ToString() : "";
                product.productId = worksheet.Cells["F" + row].Value != null ? worksheet.Cells["F" + row].Value.ToString() : "";
                product.externalProductIdType = worksheet.Cells["G" + row].Value != null ? worksheet.Cells["G" + row].Value.ToString() : "";
                product.externalProductId = worksheet.Cells["H" + row].Value != null ? worksheet.Cells["H" + row].Value.ToString() : "";
                product.productName = worksheet.Cells["I" + row].Value != null ? worksheet.Cells["I" + row].Value.ToString() : "";
                product.brand = worksheet.Cells["J" + row].Value != null ? worksheet.Cells["J" + row].Value.ToString() : "";
                product.pricePerUnitQuantity = worksheet.Cells["K" + row].Value != null ? worksheet.Cells["K" + row].Value.ToString() : "";
                product.pricePerUnitUom = worksheet.Cells["L" + row].Value != null ? worksheet.Cells["L" + row].Value.ToString() : "";
                product.multipackQuantity = worksheet.Cells["M" + row].Value != null ? worksheet.Cells["M" + row].Value.ToString() : "";
                product.shortDescription = worksheet.Cells["N" + row].Value != null ? worksheet.Cells["N" + row].Value.ToString() : "";
                product.keyFeatures = worksheet.Cells["O" + row].Value != null ? worksheet.Cells["O" + row].Value.ToString() : "";
                product.keyFeatures = worksheet.Cells["P" + row].Value != null ? worksheet.Cells["P" + row].Value.ToString() : "";
                product.keyFeatures = worksheet.Cells["Q" + row].Value != null ? worksheet.Cells["Q" + row].Value.ToString() : "";
                product.mainImageUrl = worksheet.Cells["R" + row].Value != null ? worksheet.Cells["R" + row].Value.ToString() : "";
                product.productSecondaryImageURL = worksheet.Cells["S" + row].Value != null ? worksheet.Cells["S" + row].Value.ToString() : "";
                product.productSecondaryImageURL = worksheet.Cells["T" + row].Value != null ? worksheet.Cells["T" + row].Value.ToString() : "";
                product.productSecondaryImageURL = worksheet.Cells["U" + row].Value != null ? worksheet.Cells["U" + row].Value.ToString() : "";
                product.productSecondaryImageURL = worksheet.Cells["V" + row].Value != null ? worksheet.Cells["V" + row].Value.ToString() : "";
                product.electronicsIndicator = worksheet.Cells["W" + row].Value != null ? worksheet.Cells["W" + row].Value.ToString() : "";
                product.batteryTechnologyType = worksheet.Cells["X" + row].Value != null ? worksheet.Cells["X" + row].Value.ToString() : "";
                product.chemicalAerosolPesticide = worksheet.Cells["Y" + row].Value != null ? worksheet.Cells["Y" + row].Value.ToString() : "";
                product.prop65WarningText = worksheet.Cells["Z" + row].Value != null ? worksheet.Cells["Z" + row].Value.ToString() : "";
                product.price = worksheet.Cells["AA" + row].Value != null ? worksheet.Cells["AA" + row].Value.ToString() : "";
                product.startDate = worksheet.Cells["AB" + row].Value != null ? worksheet.Cells["AB" + row].Value.ToString() : "";
                product.endDate = worksheet.Cells["AC" + row].Value != null ? worksheet.Cells["AC" + row].Value.ToString() : "";
                product.additionalOfferAttributeName = worksheet.Cells["AD" + row].Value != null ? worksheet.Cells["AD" + row].Value.ToString() : "";
                product.additionalOfferAttributeValue = worksheet.Cells["AE" + row].Value != null ? worksheet.Cells["AE" + row].Value.ToString() : "";
                product.msrp = worksheet.Cells["AF" + row].Value != null ? worksheet.Cells["AF" + row].Value.ToString() : "";
                product.shipsInOriginalPackaging = worksheet.Cells["AG" + row].Value != null ? worksheet.Cells["AG" + row].Value.ToString() : "";
                product.MustShipAlone = worksheet.Cells["AH" + row].Value != null ? worksheet.Cells["AH" + row].Value.ToString() : "";
                product.ShippingWeight = worksheet.Cells["AI" + row].Value != null ? worksheet.Cells["AI" + row].Value.ToString() : "";
                product.stateRestrictionsText = worksheet.Cells["AJ" + row].Value != null ? worksheet.Cells["AJ" + row].Value.ToString() : "";
                product.states = worksheet.Cells["AK" + row].Value != null ? worksheet.Cells["AK" + row].Value.ToString() : "";
                product.zipCodes = worksheet.Cells["AL" + row].Value != null ? worksheet.Cells["AL" + row].Value.ToString() : "";
                product.smallPartsWarnings = worksheet.Cells["AM" + row].Value != null ? worksheet.Cells["AM" + row].Value.ToString() : "";
                product.gender = worksheet.Cells["AN" + row].Value != null ? worksheet.Cells["AN" + row].Value.ToString() : "";
                product.size = worksheet.Cells["AO" + row].Value != null ? worksheet.Cells["AO" + row].Value.ToString() : "";
                product.ageGroup = worksheet.Cells["AP" + row].Value != null ? worksheet.Cells["AP" + row].Value.ToString() : "";
                product.manufacturer = worksheet.Cells["AQ" + row].Value != null ? worksheet.Cells["AQ" + row].Value.ToString() : "";
                product.manufacturerPartNumber = worksheet.Cells["AR" + row].Value != null ? worksheet.Cells["AR" + row].Value.ToString() : "";
                product.modelNumber = worksheet.Cells["AS" + row].Value != null ? worksheet.Cells["AS" + row].Value.ToString() : "";
                product.countPerPack = worksheet.Cells["AT" + row].Value != null ? worksheet.Cells["AT" + row].Value.ToString() : "";
                product.count = worksheet.Cells["AU" + row].Value != null ? worksheet.Cells["AU" + row].Value.ToString() : "";
                product.color = worksheet.Cells["AV" + row].Value != null ? worksheet.Cells["AV" + row].Value.ToString() : "";
                product.colorCategory = worksheet.Cells["AW" + row].Value != null ? worksheet.Cells["AW" + row].Value.ToString() : "";
                product.ringStyle = worksheet.Cells["AX" + row].Value != null ? worksheet.Cells["AX" + row].Value.ToString() : "";
                product.ringSize = worksheet.Cells["AY" + row].Value != null ? worksheet.Cells["AY" + row].Value.ToString() : "";
                product.gemstone = worksheet.Cells["AZ" + row].Value != null ? worksheet.Cells["AZ" + row].Value.ToString() : "";
                product.secondaryStoneType = worksheet.Cells["BA" + row].Value != null ? worksheet.Cells["BA" + row].Value.ToString() : "";
                product.accentStone = worksheet.Cells["BB" + row].Value != null ? worksheet.Cells["BB" + row].Value.ToString() : "";
                product.numberOfGemstones = worksheet.Cells["BC" + row].Value != null ? worksheet.Cells["BC" + row].Value.ToString() : "";
                product.chainPattern = worksheet.Cells["BD" + row].Value != null ? worksheet.Cells["BD" + row].Value.ToString() : "";
                product.measureChain = worksheet.Cells["BE" + row].Value != null ? worksheet.Cells["BE" + row].Value.ToString() : "";
                product.unitChain = worksheet.Cells["BF" + row].Value != null ? worksheet.Cells["BF" + row].Value.ToString() : "";
                product.claspType = worksheet.Cells["BG" + row].Value != null ? worksheet.Cells["BG" + row].Value.ToString() : "";
                product.earringStyle = worksheet.Cells["BH" + row].Value != null ? worksheet.Cells["BH" + row].Value.ToString() : "";
                product.measureEar = worksheet.Cells["BI" + row].Value != null ? worksheet.Cells["BI" + row].Value.ToString() : "";
                product.unitEar = worksheet.Cells["BJ" + row].Value != null ? worksheet.Cells["BJ" + row].Value.ToString() : "";
                product.backFinding = worksheet.Cells["BK" + row].Value != null ? worksheet.Cells["BK" + row].Value.ToString() : "";
                product.braceletStyle = worksheet.Cells["BL" + row].Value != null ? worksheet.Cells["BL" + row].Value.ToString() : "";
                product.necklaceStyle = worksheet.Cells["BM" + row].Value != null ? worksheet.Cells["BM" + row].Value.ToString() : "";
                product.bodyJewelryStyle = worksheet.Cells["BN" + row].Value != null ? worksheet.Cells["BN" + row].Value.ToString() : "";
                product.braceletCharmStyle = worksheet.Cells["BO" + row].Value != null ? worksheet.Cells["BO" + row].Value.ToString() : "";
                product.jewelrySetting = worksheet.Cells["BP" + row].Value != null ? worksheet.Cells["BP" + row].Value.ToString() : "";
                product.certifyingAgent = worksheet.Cells["BQ" + row].Value != null ? worksheet.Cells["BQ" + row].Value.ToString() : "";
                product.diamondClarity = worksheet.Cells["BR" + row].Value != null ? worksheet.Cells["BR" + row].Value.ToString() : "";
                product.diamondColor = worksheet.Cells["BS" + row].Value != null ? worksheet.Cells["BS" + row].Value.ToString() : "";
                product.diamondCut = worksheet.Cells["BT" + row].Value != null ? worksheet.Cells["BT" + row].Value.ToString() : "";
                product.measurePrimary = worksheet.Cells["BU" + row].Value != null ? worksheet.Cells["BU" + row].Value.ToString() : "";
                product.unitPrimary = worksheet.Cells["BV" + row].Value != null ? worksheet.Cells["BV" + row].Value.ToString() : "";
                product.measureTotal = worksheet.Cells["BW" + row].Value != null ? worksheet.Cells["BW" + row].Value.ToString() : "";
                product.unitTotal = worksheet.Cells["BX" + row].Value != null ? worksheet.Cells["BX" + row].Value.ToString() : "";
                product.stoneTreatment = worksheet.Cells["BY" + row].Value != null ? worksheet.Cells["BY" + row].Value.ToString() : "";
                product.stoneCreationMethod = worksheet.Cells["BZ" + row].Value != null ? worksheet.Cells["BZ" + row].Value.ToString() : "";
                product.gemstoneShape = worksheet.Cells["CA" + row].Value != null ? worksheet.Cells["CA" + row].Value.ToString() : "";
                product.pearlType = worksheet.Cells["CB" + row].Value != null ? worksheet.Cells["CB" + row].Value.ToString() : "";
                product.measureSize = worksheet.Cells["CC" + row].Value != null ? worksheet.Cells["CC" + row].Value.ToString() : "";
                product.unitSize = worksheet.Cells["CD" + row].Value != null ? worksheet.Cells["CD" + row].Value.ToString() : "";
                product.isResizable = worksheet.Cells["CE" + row].Value != null ? worksheet.Cells["CE" + row].Value.ToString() : "";
                product.personalRelationship = worksheet.Cells["CF" + row].Value != null ? worksheet.Cells["CF" + row].Value.ToString() : "";
                product.metal = worksheet.Cells["CG" + row].Value != null ? worksheet.Cells["CG" + row].Value.ToString() : "";
                product.metalPurity = worksheet.Cells["CH" + row].Value != null ? worksheet.Cells["CH" + row].Value.ToString() : "";
                product.material = worksheet.Cells["CI" + row].Value != null ? worksheet.Cells["CI" + row].Value.ToString() : "";
                product.religion = worksheet.Cells["CJ" + row].Value != null ? worksheet.Cells["CJ" + row].Value.ToString() : "";
                product.birthstone = worksheet.Cells["CK" + row].Value != null ? worksheet.Cells["CK" + row].Value.ToString() : "";
                product.style = worksheet.Cells["CL" + row].Value != null ? worksheet.Cells["CL" + row].Value.ToString() : "";
                product.shape = worksheet.Cells["CM" + row].Value != null ? worksheet.Cells["CM" + row].Value.ToString() : "";
                product.occasion = worksheet.Cells["CN" + row].Value != null ? worksheet.Cells["CN" + row].Value.ToString() : "";
                product.bodyParts = worksheet.Cells["CO" + row].Value != null ? worksheet.Cells["CO" + row].Value.ToString() : "";
                product.isMadeFromRecycledMaterial = worksheet.Cells["CP" + row].Value != null ? worksheet.Cells["CP" + row].Value.ToString() : "";
                product.recycledMaterial = worksheet.Cells["CQ" + row].Value != null ? worksheet.Cells["CQ" + row].Value.ToString() : "";
                product.percentageOfRecycledMaterial = worksheet.Cells["CR" + row].Value != null ? worksheet.Cells["CR" + row].Value.ToString() : "";
                product.theme = worksheet.Cells["CS" + row].Value != null ? worksheet.Cells["CS" + row].Value.ToString() : "";
                product.measureDiameter = worksheet.Cells["CT" + row].Value != null ? worksheet.Cells["CT" + row].Value.ToString() : "";
                product.unitDiameter = worksheet.Cells["CU" + row].Value != null ? worksheet.Cells["CU" + row].Value.ToString() : "";
                product.character = worksheet.Cells["CV" + row].Value != null ? worksheet.Cells["CV" + row].Value.ToString() : "";
                product.sportsTeam = worksheet.Cells["CW" + row].Value != null ? worksheet.Cells["CW" + row].Value.ToString() : "";
                product.sportsLeague = worksheet.Cells["CX" + row].Value != null ? worksheet.Cells["CX" + row].Value.ToString() : "";
                product.warrantyText = worksheet.Cells["CY" + row].Value != null ? worksheet.Cells["CY" + row].Value.ToString() : "";
                product.warrantyURL = worksheet.Cells["CZ" + row].Value != null ? worksheet.Cells["CZ" + row].Value.ToString() : "";
                product.productAttributeName = worksheet.Cells["DA" + row].Value != null ? worksheet.Cells["DA" + row].Value.ToString() : "";
                product.productAttributeValue = worksheet.Cells["DB" + row].Value != null ? worksheet.Cells["DB" + row].Value.ToString() : "";
                product.seasonYear = worksheet.Cells["DC" + row].Value != null ? worksheet.Cells["DC" + row].Value.ToString() : "";
                product.seasonCode = worksheet.Cells["DD" + row].Value != null ? worksheet.Cells["DD" + row].Value.ToString() : "";
                product.variantGroupId = worksheet.Cells["DE" + row].Value != null ? worksheet.Cells["DE" + row].Value.ToString() : "";
                product.removeVariantGroup = worksheet.Cells["DF" + row].Value != null ? worksheet.Cells["DF" + row].Value.ToString() : "";
                product.variantAttributeNames = worksheet.Cells["DG" + row].Value != null ? worksheet.Cells["DG" + row].Value.ToString() : "";
                product.isPrimaryVariant = worksheet.Cells["DH" + row].Value != null ? worksheet.Cells["DH" + row].Value.ToString() : "";
                product.swatchVariantAttribute = worksheet.Cells["DI" + row].Value != null ? worksheet.Cells["DI" + row].Value.ToString() : "";
                product.swatchImageUrl = worksheet.Cells["DJ" + row].Value != null ? worksheet.Cells["DJ" + row].Value.ToString() : "";

                var msgErr = _validatation.Validate(product);
                if (IsAllPropertiesNull(msgErr) == false)
                {
                    msgErr.SKU = product.SKU;
                    lstFailed.Add(msgErr);
                }
                else
                {
                    product.isDeleted = false;
                    product.Note = string.Empty;
                    lstSuccess.Add(product);
                }
            };


            if (lstFailed.Count > 0)
            {
                _result.IsSuccess = false;
                _result.Data = lstFailed;
                _result.Message = "Failed " + lstFailed.Count + " datas!";
            }
            else
            {
                _result.IsSuccess = true;
            }
            var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var lstNeedBulkInsertUpdate = _mapper.Map<List<Walmart>>(lstSuccess);

            lstNeedBulkInsertUpdate.ToList().ForEach(x =>
            {
                x.UpdatedDate = curDate;
                x.isDeleted = false;
            });

            int batchSize = 10_000; // Số lượng tối đa trong mỗi danh sách con
            int numBatches = (int)Math.Ceiling((double)lstNeedBulkInsertUpdate.Count / batchSize);

            for (int i = 0; i < numBatches; i++)
            {
                var batch = lstNeedBulkInsertUpdate
                    .Skip(i * batchSize)
                    .Take(batchSize)
                    .ToList();

                await _context.BulkInsertOrUpdateAsync(batch);
            }

            _result.Message += " Success " + lstSuccess.Count + " datas!";
            return _result;
        }

        public async Task<OperationResult> GetAllId()
        {
            _result = new OperationResult()
            {
                Data = await _context.Walmarts.Where(x => x.isDeleted != true).Select(x => x.SKU).ToListAsync(),
                IsSuccess = true
            };
            return _result;
        }

        public async Task<OperationResult> BulkInsertUpdate(List<WalmartDTO> lstData)
        {
            var lstFailed = new List<WalmartDTO>();
            var lstSuccess = new List<WalmartDTO>();
            foreach (var item in lstData)
            {
                var msgErr = _validatation.Validate(item);
                if (IsAllPropertiesNull(msgErr) == false)
                {
                    msgErr.SKU = item.SKU;
                    lstFailed.Add(msgErr);
                }
                else
                {
                    item.isDeleted = false;
                    item.Note = string.Empty;
                    lstSuccess.Add(item);
                }
            }
            if (lstFailed.Count > 0)
            {
                _result.IsSuccess = false;
                _result.Data = lstFailed;
                _result.Message = "Failed " + lstFailed.Count + " datas!";
            }
            else
            {
                _result.IsSuccess = true;
            }
            var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            var lstNeedBulkInsertUpdate = _mapper.Map<List<Walmart>>(lstSuccess);

            lstNeedBulkInsertUpdate.ToList().ForEach(x =>
            {
                x.UpdatedDate = curDate;
                x.isDeleted = false;
            });

            int batchSize = 10_000; // Số lượng tối đa trong mỗi danh sách con
            int numBatches = (int)Math.Ceiling((double)lstNeedBulkInsertUpdate.Count / batchSize);

            for (int i = 0; i < numBatches; i++)
            {
                var batch = lstNeedBulkInsertUpdate
                    .Skip(i * batchSize)
                    .Take(batchSize)
                    .ToList();

                await _context.BulkInsertOrUpdateAsync(batch);
            }

            _result.Message += " Success " + lstSuccess.Count + " datas!";
            return _result;
        }

        public async Task<OperationResult> ExportQuantity(PaginationParam param)
        {
            var data = new List<Walmart>();
            var query = _context.Walmarts.Where(x => x.isDeleted != true);
            if (!string.IsNullOrWhiteSpace(param.KeySearch))
            {
                var searchKey = param.KeySearch;
                query = query.Where(x => x.SKU.Contains(searchKey)
                || x.shortDescription!.Contains(searchKey)
                || x.productName!.Contains(searchKey));
            }

            if (!string.IsNullOrWhiteSpace(param.BrandProduct))
            {
                query = query.Where(x => x.SKU.StartsWith(param.BrandProduct));
            }

            if (param.IsUpdate == true)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == curDate);
            }

            if (param.IsUpdate == false)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == null);
            }

            query = query.Select(x => new Walmart() { SKU = x.SKU, quantity = x.quantity });

            var result = await PaginationUtility<Walmart>.CreateAsync(query.AsNoTracking(), param.PageNumber, param.PageSize);
            _result.Message = result.Pagination.TotalPage.ToString();
            data = result.Result;
            string templatePath = Path.Combine(_env.ContentRootPath, "Files", "Inventory.xlsx");
            var package = new ExcelPackage(new FileInfo(templatePath));
            var ws = package.Workbook.Worksheets["Sheet1"];

            int startRow = 3;
            foreach (var item in data)
            {
                ws.Cells[startRow, 1].Value = item.SKU;
                ws.Cells[startRow, 2].Value = item.quantity;
                ++startRow;
            }

            var stream = new MemoryStream();

            await package.SaveAsAsync(stream);
            byte[] bytes = stream.ToArray();
            _result.Data = bytes;

            return _result;
        }

        public async Task<OperationResult> ExportPrice(PaginationParam param)
        {
            var data = new List<Walmart>();
            var query = _context.Walmarts.Where(x => x.isDeleted != true);
            if (!string.IsNullOrWhiteSpace(param.KeySearch))
            {
                var searchKey = param.KeySearch;
                query = query.Where(x => x.SKU.Contains(searchKey)
                || x.shortDescription!.Contains(searchKey)
                || x.productName!.Contains(searchKey));
            }

            if (!string.IsNullOrWhiteSpace(param.BrandProduct))
            {
                query = query.Where(x => x.SKU.StartsWith(param.BrandProduct));
            }

            if (param.IsUpdate == true)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == curDate);
            }

            if (param.IsUpdate == false)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == null);
            }

            query = query.Select(x => new Walmart() { SKU = x.SKU, price = x.price });

            var result = await PaginationUtility<Walmart>.CreateAsync(query.AsNoTracking(), param.PageNumber, param.PageSize);
            _result.Message = result.Pagination.TotalPage.ToString();
            data = result.Result;
            string templatePath = Path.Combine(_env.ContentRootPath, "Files", "Price.xlsm");
            var package = new ExcelPackage(new FileInfo(templatePath));
            var ws = package.Workbook.Worksheets["Offer"];

            int startRow = 5;
            foreach (var item in data)
            {
                ws.Cells[startRow, 3].Value = item.SKU;
                ws.Cells[startRow, 4].Value = item.price;
                ++startRow;
            }

            var stream = new MemoryStream();
            await package.SaveAsAsync(stream);
            byte[] bytes = stream.ToArray();

            _result.Data = bytes;

            return _result;
        }
        public bool IsAllPropertiesNull(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(obj);

                if (value != null)
                {
                    return false;
                }
            }

            return true;
        }

        public string GetElementOrDefault(string[] array, int index)
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

        public async Task<PaginationUtility<WalmartPriceInventoryDTO>> GetPriceInventory(PaginationParam param)
        {
            IQueryable<Walmart> query = _context.Walmarts.Where(x => x.isDeleted != true);
            if (!string.IsNullOrWhiteSpace(param.KeySearch))
            {
                var searchKey = param.KeySearch;
                query = query.Where(x => x.SKU.Contains(searchKey)
                || x.shortDescription!.Contains(searchKey)
                || x.productName!.Contains(searchKey));
            }

            if (!string.IsNullOrWhiteSpace(param.BrandProduct))
            {
                query = query.Where(x => x.SKU.StartsWith(param.BrandProduct));
            }

            if (param.IsUpdate == true)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == curDate);
            }

            if (param.IsUpdate == false)
            {
                var curDate = new DateOnly(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                query = query.Where(x => x.UpdatedDate == null);
            }

            var result = query.Select(x => new WalmartPriceInventoryDTO() { SKU = x.SKU, price = x.price, quantity = x.quantity });

            return await PaginationUtility<WalmartPriceInventoryDTO>.CreateAsync(result, param.PageNumber, param.PageSize);
        }
    }
}
