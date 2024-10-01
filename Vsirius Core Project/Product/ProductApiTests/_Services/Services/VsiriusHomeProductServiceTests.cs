using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductApi._Services.Interfaces;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;

namespace ProductApi._Services.Services.Tests
{
    [TestClass()]
    public class VsiriusHomeProductServiceTests : IVsiriusHomeProductService
    {
        List<VsiriusProductDto> _vsiriusProductDto;
        List<VsiriusHomeProductDto> _vsiriusHomeProductDto;
        public VsiriusHomeProductServiceTests()
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
            _vsiriusHomeProductDto = new List<VsiriusHomeProductDto>()
            {
                new VsiriusHomeProductDto()  { item_sku = "S-123626:6031:P", IsPopular = true, IsLatest = true,
                    IsActive = true, IsDelete = false, CreatedBy="haphan"},
                new VsiriusHomeProductDto()  { item_sku = "S-123626:6036:P", IsPopular = true, IsLatest = true,
                    IsActive = true, IsDelete = false, CreatedBy="haphan"},
                new VsiriusHomeProductDto()  { item_sku = "S-123626:6045:P", IsPopular = false, IsLatest = true,
                    IsActive = true, IsDelete = false, CreatedBy="haphan"},
                new VsiriusHomeProductDto()  { item_sku = "S-123626:6024:P", IsPopular = false, IsLatest = true,
                    IsActive = true, IsDelete = false, CreatedBy="haphan"},
            };
        }

        public async Task<OperationResult> CreateUpdateHomeProduct(VsiriusHomeProductDto dto)
        {
            dto = new VsiriusHomeProductDto()
            {
                item_sku = "S-123626:6045:P",
                IsPopular = false,
                IsLatest = true,
                IsActive = true,
                IsDelete = false,
                CreatedBy = "haphan"
            };
            // mong muốn trả ra mess = "Update successful" vì item_sku đã tồn tại
            var result = new OperationResult { Success = true, Message = "Update successful" };
            return result;
        }

        public async Task<List<VsiriusProductDto>> GetAllHomeProduct(VsiriusHomeProductParams param)
        {
            param.Filter = "popular";
            param.TakeProduct = 10;
            //trả rá count vì trong data mock chỉ có 1 S-123626:6024:P thỏa mãn
            return _vsiriusProductDto.Where(x => x.item_sku == "S-123626:6024:P").ToList();
        }

        public async Task<List<VsiriusHomeProductDto>> GetAllHomeProductBackend()
        {
            //trả rá count vì trong data mock chỉ có 1 S-123626:6024:P thỏa mãn
            return _vsiriusHomeProductDto.Where(x => x.item_sku == "S-123626:6024:P").ToList();
        }

        public async Task<List<VsiriusProductDto>> GetRelatedProduct(VsiriusHomeProductParams param)
        {
            param.Filter = "jewelry-sets";
            param.TakeProduct = 10;
            //lấy hết, vì đều liên quan tới "jewelry-sets"
            return _vsiriusProductDto.ToList();
        }

        public async Task<OperationResult> RemoveHomeProduct(string item_sku)
        {
            var data = _vsiriusHomeProductDto.Where(x => x.item_sku == item_sku).First();
            if (data != null)
            {
                return new OperationResult { Success = true, Message = "Successful delete!!!" };
            }
            return new OperationResult { Success = false, Message = "Error" };
        }
    }
}