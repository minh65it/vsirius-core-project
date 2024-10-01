using ProductApi._Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ProductApi._Services.Interfaces;

namespace ProductApi._Services.Services.Tests
{
    [TestClass()]
    public class VsiriusProductServiceTests : IVsiriusProductService
    {
        List<VsiriusProductDto> _vsiriusProductDto;
        List<VsiriusFilterParamDto> _vsiriusFilterParamDto;
        public VsiriusProductServiceTests()
        {
            _vsiriusProductDto = new List<VsiriusProductDto>()
            {
                new VsiriusProductDto() { item_sku = "S-123626:6021:P", feed_product_type ="jewelryset", brand_name ="VSIRIUS",
                    item_name = "14K Rose 1/5 CTW Natural Diamond Anniversary Band", main_image_url ="https://meteor.stullercloud.com/das/73364153?obj=metals&obj=stones/diamonds/g_accent&obj=metals&obj.recipe=white&$xlarge$",
                    target_gender ="Unisex", quantity ="1", standard_price ="340", item_type ="jewelry-sets"},
                new VsiriusProductDto() { item_sku = "S-123626:6022:P", feed_product_type ="jewelryset", brand_name ="VSIRIUS",
                    item_name = "14K Rose 5/8 CTW Natural Diamond Anniversary Band", main_image_url ="https://meteor.stullercloud.com/das/73364153?obj=metals&obj=stones/diamonds/g_accent&obj=metals&obj.recipe=white&$xlarge$",
                    target_gender ="Unisex", quantity ="1", standard_price ="340", item_type ="jewelry-sets"},
                new VsiriusProductDto() { item_sku = "S-123626:6023:P", feed_product_type ="jewelryset", brand_name ="VSIRIUS",
                    item_name = "Platinum 1/3 CTW Natural Diamond Anniversary Band", main_image_url ="https://meteor.stullercloud.com/das/73364153?obj=metals&obj=stones/diamonds/g_accent&obj=metals&obj.recipe=white&$xlarge$",
                    target_gender ="Unisex", quantity ="1", standard_price ="340", item_type ="jewelry-sets"},
                new VsiriusProductDto() { item_sku = "S-123626:6024:P", feed_product_type ="jewelryset", brand_name ="VSIRIUS",
                    item_name = "14K Rose 1/4 CTW Natural Diamond Anniversary Band", main_image_url ="https://meteor.stullercloud.com/das/73364153?obj=metals&obj=stones/diamonds/g_accent&obj=metals&obj.recipe=white&$xlarge$",
                    target_gender ="Unisex", quantity ="1", standard_price ="340", item_type ="jewelry-sets"},
                new VsiriusProductDto() { item_sku = "S-123626:6025:P", feed_product_type ="jewelryset", brand_name ="VSIRIUS",
                    item_name = "14K White 1 1/8 CTW Natural Diamond Anniversary Band", main_image_url ="https://meteor.stullercloud.com/das/73364153?obj=metals&obj=stones/diamonds/g_accent&obj=metals&obj.recipe=white&$xlarge$",
                    target_gender ="Unisex", quantity ="1", standard_price ="340", item_type ="jewelry-sets"},

            };
            _vsiriusFilterParamDto = new List<VsiriusFilterParamDto>()
            {
                new VsiriusFilterParamDto()  { FieldName = "feed_product_type", LabelName = "Product Type",
                        Value = new List<string>{ "bracelet", "earring", "necktie", "watches" }, ProductType="", GroupName = "Required"},
                new VsiriusFilterParamDto()  { FieldName = "color_name", LabelName = "Color",
                        Value = new List<string>{ "Black", "Blue"},ProductType="ring",  GroupName = "Required"},
            };
        }

        public async Task<OperationResult> CreateProduct(VsiriusProductDto model)
        {
            model = new VsiriusProductDto()
            {
                item_sku = "S-123626:6025:P",
                feed_product_type = "jewelryset",
                brand_name = "VSIRIUS",
                item_name = "14K White 1 1/8 CTW Natural Diamond Anniversary Band",
                main_image_url = "https://meteor.stullercloud.com/das/73364153?obj=metals&obj=stones/diamonds/g_accent&obj=metals&obj.recipe=white&$xlarge$",
                target_gender = "Unisex",
                quantity = "1",
                standard_price = "340",
                item_type = "jewelry-sets"
            };
            if (_vsiriusProductDto.Contains(model))
            {
                var result1 = new OperationResult { Success = false, Message = "Product already exists!" };
                return result1;
            }
            var result2 = new OperationResult { Success = true };
            return result2;
        }

        public async Task<OperationResult> ImportFile(IFormFile fileExcel)
        {
            var fileExtension = ".png";
            string?[] arrNameFile = new string?[] {
                ".xlsx", ".xls",".csv"
            };

            if (!arrNameFile.Contains(fileExtension))
            {
                return new OperationResult(false, "Not file excel");
            }
            return new OperationResult { Success = true };
        }

        public async Task<PaginationUtility<VsiriusProductDto>> GetAll(List<VsiriusFilterParamDto> filterParam, PaginationParam pagination, string fullTextSearch)
        {
            return PaginationUtility<VsiriusProductDto>.Create(_vsiriusProductDto, pagination.PageNumber, pagination.PageSize, true);
        }

        public async Task<PaginationUtility<VsiriusProductDto>> GetAllLayer1(string layer1)
        {
            return PaginationUtility<VsiriusProductDto>.Create(_vsiriusProductDto, 1, 10, true);
        }

        public async Task<VsiriusProductDto> GetProduct(string item_sku)
        {
            return _vsiriusProductDto.Where(x => x.item_sku == item_sku).First();
        }

        public async Task<OperationResult> RemoveProduct(string item_sku)
        {
            var data = _vsiriusProductDto.Where(x => x.item_sku == item_sku).First();
            if (data != null)
            {
                return new OperationResult { Success = true, Message = "Successful delete!!!" };
            }
            return new OperationResult { Success = false, Message = "Error" };
        }

        public async Task<OperationResult> UpdateProduct(VsiriusProductDto model)
        {
            var data = _vsiriusProductDto.Where(x => x.item_sku == model.item_sku).First();
            if (data == null)
            {
                return new OperationResult { Success = false, Message = "Error" };
            }
            return new OperationResult { Success = true, Message = "Successful update!!!" };
        }

        public async Task<List<VsiriusFilterParamDto>> GetFilter(string feed_product_type)
        {
            return _vsiriusFilterParamDto.Where(x => x.ProductType == feed_product_type).ToList();
        }

        public async Task<List<VsiriusFilterParamDto>> GetValueForAdd(string feed_product_type)
        {
            return _vsiriusFilterParamDto.Where(x => x.ProductType == feed_product_type).ToList();
        }

        public async Task<List<string>> GetAllItemSKU(string item_sku_category)
        {
            return _vsiriusProductDto.Where(x => x.item_sku.Contains("S-"))
                .Select(x => x.item_sku).ToList();
        }

        public async Task<OperationResult> RemoveMultipleProduct(List<string> listSKU)
        {
            if (listSKU.Any())
            {
                var data = _vsiriusProductDto.Where(x => x.item_sku == listSKU[0]).First();
                if (data != null)
                {
                    return new OperationResult { Success = true, Message = "Successful delete!!!" };
                }
                return new OperationResult { Success = false, Message = "Error" };
            }
            return new OperationResult { Success = false, Message = "Error" };
        }
    }
}