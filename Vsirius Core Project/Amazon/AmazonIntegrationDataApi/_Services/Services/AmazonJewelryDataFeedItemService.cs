
using AmazonIntegrationDataApi._Repositories;
using AmazonIntegrationDataApi._Repositories.Interfaces;
using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Data;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using AmazonIntegrationDataApi.Helpers.Utilities;
using AmazonIntegrationDataApi.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.BulkExtensions;
using Korzh.EasyQuery.Services;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Reflection;

namespace AmazonIntegrationDataApi._Services.Services
{
    public class AmazonJewelryDataFeedItemService : IAmazonJewelryDataFeedItemService
    {
        private readonly IRepositoryAccessor _repositoryAccessor;
        private readonly IAmazonJewelryDataFeedItemRepository _amazonJewelryDataFeedItemRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly ValidationAmazon _valid;
        private readonly DBContext _context;
        public AmazonJewelryDataFeedItemService(IRepositoryAccessor repositoryAccessor,
            IMapper mapper, MapperConfiguration mapperConfiguration, ValidationAmazon valid, DBContext context,
            IAmazonJewelryDataFeedItemRepository amazonJewelryDataFeedItemRepository)
        {
            _repositoryAccessor = repositoryAccessor;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _valid = valid;
            _context = context;
            _amazonJewelryDataFeedItemRepository = amazonJewelryDataFeedItemRepository;
        }

        public async Task<OperationResult> Add(AmazonJewelryDataFeedItemV3_Dto dto)
        {
            AmazonJewelryDataFeedItemV3_Dto msg = _valid.IsValid(dto);

            foreach (FieldInfo field in typeof(AmazonJewelryDataFeedItemV3_Dto).GetFields())
            {
                if (field.GetValue(msg) != null)
                {
                    // Nếu tất trường null => trả về true
                    return new OperationResult { IsSuccess = false, Data = msg };
                }
            }
            var data = _mapper.Map<AmazonJewelryDataFeedItemV3>(dto);
            _repositoryAccessor.AmazonJewelryDataFeedItems.Add(data);
            try
            {
                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult { IsSuccess = true, Data = data };
            }
            catch
            {
                throw;
            }
        }


        public async Task<OperationResult> Delete(AmazonJewelryDataFeedItemV3_Dto dto)
        {
            var item = await _repositoryAccessor.AmazonJewelryDataFeedItems.FindSingle(x => x.item_sku == dto.item_sku);
            _repositoryAccessor.AmazonJewelryDataFeedItems.Remove(item);
            try
            {
                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                throw;
            }
        }

        public ExcelWorksheet? ReadFile(IFormFile fileExcel)
        {
            string fileExtension = Path.GetExtension(fileExcel.FileName);
            string[] arrNameFile = new string[] {
                ".xlsx", ".xls",".csv"
            };

            if (!arrNameFile.Contains(fileExtension))
            {
                return null;
            }
            var stream = fileExcel.OpenReadStream();
            var workbook = new ExcelPackage(stream);

            // Access the first worksheet
            var worksheet = workbook.Workbook.Worksheets[0];

            return worksheet;
        }

        public async Task<OperationResult> GenDataByExcel(IFormFile fileExcel)
        {
            ExcelWorksheet? worksheet = ReadFile(fileExcel);

            if (worksheet == null)
            {
                return new OperationResult(false, "Not file excel");
            }
            var products = new List<AmazonJewelryDataFeedItemV3_Dto>();
            var notifications = new List<AmazonJewelryDataFeedItemV3_Dto>();

            for (int row = 4; row < worksheet.Dimension.End.Row; row++)
            {

                //var msg = await _valid.IsValid(product);
                //if (CheckAllPropertiesNull(msg) == false)
                //{
                //    MessageValidation notification = new()
                //    {
                //        Line = products.Count,
                //        Msg = msg
                //    };
                //    notifications.Add(notification);
                //}

                var product = new AmazonJewelryDataFeedItemV3_Dto();
                var properties = product.GetType().GetProperties(); // lấy danh sách các property của đối tượng

                int coulm = 1;
                foreach (var prop in properties)
                {
                    var columnName = prop.Name; // lấy tên của property

                    var cellValue = worksheet.Cells[row, coulm].Value; // lấy giá trị của cell

                    if (cellValue != null && cellValue.ToString() != "")
                    {
                        prop.SetValue(product, Convert.ChangeType(cellValue, prop.PropertyType), null);
                    }
                    coulm++;

                }

                var msg = _valid.IsValid(product);
                if (IsAllPropertiesNull(msg))
                {
                    products.Add(product);
                }
                else
                {
                    msg.item_sku = product.item_sku;
                    notifications.Add(msg);
                }
            };

            try
            {
                if (notifications.Count > 0)
                {
                    return new OperationResult { IsSuccess = false, Data = notifications };
                }

                //var lstProduct = _mapper.Map<List<AmazonJewelryDataFeedItemV3>>(notifications);
                var lstProduct = _mapper.Map<List<AmazonJewelryDataFeedItemV3>>(products);

                await InsertSQLDatabase(lstProduct);

                return new OperationResult(true);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, error: ex.Message);
            }
        }

        public async Task<bool> InsertSQLDatabase(List<AmazonJewelryDataFeedItemV3> lstProduct)
        {
            var curDate = DateTime.Now;
            lstProduct.ForEach(x => x.UpdatedDate = curDate);
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

                    await _context.BulkInsertOrUpdateAsync(batch);
                }
            }
            return true;
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

        public async Task<PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>> GetData(PaginationParam pagination, string keyword, bool isPagging)
        {

            try
            {

                var predicate = PredicateBuilder.New<AmazonJewelryDataFeedItemV3>(x => x.IsDeleted == false);
                if (!string.IsNullOrEmpty(keyword))
                {
                    predicate.And(x => x.item_name.Contains(keyword) || x.standard_price.Contains(keyword)
                        || x.feed_product_type.Contains(keyword) || x.item_sku.Contains(keyword)
                        || x.item_type.Contains(keyword) || x.metal_type.Contains(keyword)
                        || x.model_name.Contains(keyword) || x.color_name.Contains(keyword)
                        || x.item_type_name.Contains(keyword));
                }

                var query = _repositoryAccessor.AmazonJewelryDataFeedItems.FindAll(predicate).ProjectTo<AmazonJewelryDataFeedItemV3_Dto>(_mapperConfiguration);
                var result = await PaginationUtility<AmazonJewelryDataFeedItemV3_Dto>.CreateCustomAsync(query, pagination.PageNumber, pagination.PageSize, isPagging);

                return result;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<OperationResult> Update(AmazonJewelryDataFeedItemV3_Dto dto)
        {
            if (string.IsNullOrEmpty(dto.external_product_id) || string.IsNullOrEmpty(dto.external_product_id_type))
            {
                return new OperationResult { IsSuccess = false, Error = "external_product_id or external_product_id_type not null" };
            }

            AmazonJewelryDataFeedItemV3_Dto msg = _valid.IsValid(dto);

            foreach (FieldInfo field in typeof(AmazonJewelryDataFeedItemV3_Dto).GetFields())
            {
                if (field.GetValue(msg) != null)
                {
                    return new OperationResult { IsSuccess = false, Data = msg }; // Nếu có trường nào null, trả về true
                }
            }
            var data = _mapper.Map<AmazonJewelryDataFeedItemV3>(dto);
            _repositoryAccessor.AmazonJewelryDataFeedItems.Update(data);
            try
            {
                await _repositoryAccessor.SaveChangesAsync();
                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<string>> GetAllItemSKU(string item_sku_category)
        {
            var predicate = PredicateBuilder.New<AmazonJewelryDataFeedItemV3>(x => x.IsDeleted != true);
            if (string.IsNullOrWhiteSpace(item_sku_category))
            {
                return null;
            }
            if (item_sku_category.ToUpper() == "Q")
                predicate.And(x => x.item_sku.StartsWith("Q-"));
            if (item_sku_category.ToUpper() == "S")
                predicate.And(x => x.item_sku.StartsWith("S-"));
            var data = await _repositoryAccessor.AmazonJewelryDataFeedItems.FindAll(predicate).Select(x => x.item_sku).ToListAsync();
            return data;
        }

        public async Task<OperationResult> RemoveMultipleProduct(List<string> listSKU)
        {
            try
            {
                var curDate = DateTime.Now;
                await _context.AmazonJewelryDataFeedItems.Where(x => listSKU.Contains(x.item_sku))
                     .ExecuteUpdateAsync(s => s
                     .SetProperty(i => i.IsDeleted, i => true)
                     .SetProperty(i => i.UpdatedDate, i => curDate));

                return new OperationResult { IsSuccess = true };
            }
            catch
            {
                throw;
            }
        }

        public async Task<PaginationUtility<AmazonJewelryDataToWalmart_Dto>> GetDataForWalmart(PaginationParam pagination, string keyword, bool isPaging)
        {
            var predicate = PredicateBuilder.New<AmazonJewelryDataFeedItemV3>(x => x.IsDeleted != true);
            if (!string.IsNullOrEmpty(keyword))
            {
                predicate.And(x => x.item_name.Contains(keyword) || x.standard_price.Contains(keyword)
                    || x.feed_product_type.Contains(keyword) || x.item_sku.Contains(keyword)
                    || x.item_type.Contains(keyword) || x.metal_type.Contains(keyword)
                    || x.model_name.Contains(keyword) || x.color_name.Contains(keyword)
                    || x.item_type_name.Contains(keyword));
            }

            try
            {
                var data = _amazonJewelryDataFeedItemRepository.FindAll(predicate).Select(x => new AmazonJewelryDataToWalmart_Dto()
                {
                    item_sku = x.item_sku,
                    external_product_id_type = x.external_product_id_type,
                    external_product_id = x.external_product_id,
                    item_name = x.item_name,
                    brand_name = x.brand_name,
                    item_package_quantity = x.item_package_quantity,
                    product_description = x.product_description,
                    bullet_point1 = x.bullet_point1,
                    bullet_point2 = x.bullet_point2,
                    bullet_point3 = x.bullet_point3,
                    bullet_point4 = x.bullet_point4,
                    bullet_point5 = x.bullet_point5,
                    bullet_point6 = x.bullet_point6,
                    bullet_point7 = x.bullet_point7,
                    bullet_point8 = x.bullet_point8,
                    bullet_point9 = x.bullet_point9,
                    bullet_point10 = x.bullet_point10,
                    main_image_url = x.main_image_url,
                    other_image_url1 = x.other_image_url1,
                    other_image_url2 = x.other_image_url2,
                    other_image_url3 = x.other_image_url3,
                    other_image_url4 = x.other_image_url4,
                    other_image_url5 = x.other_image_url5,
                    other_image_url6 = x.other_image_url6,
                    other_image_url7 = x.other_image_url7,
                    other_image_url8 = x.other_image_url8,
                    california_proposition_65_chemical_names1 = x.california_proposition_65_chemical_names1,
                    california_proposition_65_chemical_names2 = x.california_proposition_65_chemical_names2,
                    california_proposition_65_chemical_names3 = x.california_proposition_65_chemical_names3,
                    california_proposition_65_chemical_names4 = x.california_proposition_65_chemical_names4,
                    california_proposition_65_chemical_names5 = x.california_proposition_65_chemical_names5,
                    map_price = x.map_price,
                    sale_from_date = x.sale_from_date,
                    sale_end_date = x.sale_end_date,
                    standard_price = x.standard_price,
                    website_shipping_weight_unit_of_measure = x.website_shipping_weight_unit_of_measure,
                    website_shipping_weight = x.website_shipping_weight,
                    cpsia_cautionary_statement1 = x.cpsia_cautionary_statement1,
                    target_gender = x.target_gender,
                    size_name = x.size_name,
                    department_name1 = x.department_name1,
                    manufacturer = x.manufacturer,
                    part_number = x.part_number,
                    model = x.model,
                    package_contains_quantity = x.package_contains_quantity,
                    color_name = x.color_name,
                    color_map = x.color_map,
                    item_type = x.item_type,
                    ring_size = x.ring_size,
                    gem_type1 = x.gem_type1,
                    gem_type2 = x.gem_type2,
                    gem_type3 = x.gem_type3,
                    number_of_stones = x.number_of_stones,
                    chain_type = x.chain_type,
                    chain_length_unit = x.chain_length_unit,
                    chain_length_derived = x.chain_length_derived,
                    clasp_type = x.clasp_type,
                    back_finding = x.back_finding,
                    setting_type = x.setting_type,
                    certificate_type1 = x.certificate_type1,
                    certificate_type2 = x.certificate_type2,
                    certificate_type3 = x.certificate_type3,
                    certificate_type4 = x.certificate_type4,
                    certificate_type5 = x.certificate_type5,
                    certificate_type6 = x.certificate_type6,
                    certificate_type7 = x.certificate_type7,
                    certificate_type8 = x.certificate_type8,
                    certificate_type9 = x.certificate_type9,
                    stone_clarity1 = x.stone_clarity1,
                    stone_color1 = x.stone_color1,
                    stone_cut1 = x.stone_cut1,
                    total_diamond_weight_unit_of_measure = x.total_diamond_weight_unit_of_measure,
                    stone_treatment_method1 = x.stone_treatment_method1,
                    stone_creation_method1 = x.stone_creation_method1,
                    stone_shape1 = x.stone_shape1,
                    pearl_type = x.pearl_type,
                    size_per_pearl = x.size_per_pearl,
                    is_resizable = x.is_resizable,
                    metal_type = x.metal_type,
                    metal_stamp = x.metal_stamp,
                    material_type1 = x.material_type1,
                    style_name = x.style_name,
                    item_shape = x.item_shape,
                    occasion_type1 = x.occasion_type1,
                    occasion_type2 = x.occasion_type2,
                    occasion_type3 = x.occasion_type3,
                    occasion_type4 = x.occasion_type4,
                    occasion_type5 = x.occasion_type5,
                    theme = x.theme,
                    item_display_diameter_unit_of_measure = x.item_display_diameter_unit_of_measure,
                    item_display_diameter = x.item_display_diameter,
                    initial_character = x.initial_character,
                    team_name = x.team_name,
                    league_name = x.league_name,
                    warranty_description = x.warranty_description,
                    collection_name = x.collection_name,
                }).AsNoTracking();


                var result = await PaginationUtility<AmazonJewelryDataToWalmart_Dto>.CreateCustomAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
                return result;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PaginationUtility<AmazonJewelryDataToAmazon_Dto>> GetDataForAmazon(PaginationParam pagination, string keyword, bool isPagging, bool? isGetAll = false)
        {
            var predicate = PredicateBuilder.New<AmazonJewelryDataFeedItemV3>(true);
            if (isGetAll == false)
            {
                predicate.And(x => x.IsDeleted != true);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                predicate.And(x => x.item_name.Contains(keyword) || x.standard_price.Contains(keyword)
                    || x.feed_product_type.Contains(keyword) || x.item_sku.Contains(keyword)
                    || x.item_type.Contains(keyword) || x.metal_type.Contains(keyword)
                    || x.model_name.Contains(keyword) || x.color_name.Contains(keyword)
                    || x.item_type_name.Contains(keyword));
            }

            try
            {
                var data = _amazonJewelryDataFeedItemRepository.FindAll(predicate).Select(x => new AmazonJewelryDataToAmazon_Dto()
                {
                    item_sku = x.item_sku,
                    standard_price = x.standard_price,
                    quantity= x.quantity,
                }).AsNoTracking();

                var result = await PaginationUtility<AmazonJewelryDataToAmazon_Dto>.CreateCustomAsync(data, pagination.PageNumber, pagination.PageSize, isPagging);
                return result;

            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}