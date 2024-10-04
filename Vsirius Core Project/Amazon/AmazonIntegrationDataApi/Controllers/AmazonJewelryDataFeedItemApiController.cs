using AmazonIntegrationDataApi._Services.Interfaces;
using AmazonIntegrationDataApi.Dtos;
using AmazonIntegrationDataApi.Helpers.SignalRConfig;
using AmazonIntegrationDataApi.Helpers.Ultilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OfficeOpenXml;
using System.Data;

namespace AmazonIntegrationDataApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AmazonJewelryDataFeedItemApiController : ControllerBase
    {
        private readonly IAmazonJewelryDataFeedItemService _service;
        private readonly IHubContext<NotificationSignalRHub> _notificationHubContext;
        public AmazonJewelryDataFeedItemApiController(IAmazonJewelryDataFeedItemService service, IHubContext<NotificationSignalRHub> notificationHubContext)
        {
            _service = service;
            _notificationHubContext = notificationHubContext;
        }


        /// <summary>
        /// Get data 
        /// </summary>
        /// <remarks>
        /// 123
        /// </remarks>
        /// <param name="pagination"></param>
        /// <param name="keyword">123</param>
        /// <returns></returns>
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination, [FromQuery] string? keyword)
        {
            return Ok(await _service.GetData(pagination, keyword, true));
        }
        /// <summary>
        /// Tạo dữ liệu bằng file excel 
        /// </summary>
        /// <param name="fileExcel">
        /// 
        /// </param>
        /// <returns></returns>
        [DisableRequestSizeLimit]
        [HttpPost("ImportExcel")]
        public async Task<IActionResult> ImportExcel(IFormFile fileExcel)
        {
            var result = await _service.GenDataByExcel(fileExcel);
            return Ok(result);
        }
        /// <summary>
        /// Thêm dữ liệu 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add(AmazonJewelryDataFeedItemV3_Dto model)
        {
            var result = await _service.Add(model);
            await _notificationHubContext.Clients.All.SendAsync("DataAmazon", result.Data);
            return Ok(result);
        }
        /// <summary>
        /// Xóa dữ liệu 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Delete")]
        public async Task<IActionResult> Delete(AmazonJewelryDataFeedItemV3_Dto dto)
        {
            return Ok(await _service.Delete(dto));
        }
        /// <summary>
        /// Cập nhật dữ liệu
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        public async Task<IActionResult> Update(AmazonJewelryDataFeedItemV3_Dto model)
        {
            return Ok(await _service.Update(model));
        }
        /// <summary>
        /// Xuất dữ liệu dạng file excel csv
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel([FromQuery] PaginationParam pagination, [FromQuery] string keyword)
        {
            var listData = await _service.GetData(pagination, keyword, false);

            var fileName = Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Amazon_Jewelry_Data_Export_V3.xlsx");
            var package = new ExcelPackage(fileName);
            
            var ws = package.Workbook.Worksheets["Sheet1"];
            int rowIndex = 1;
            int columnIndex;
            var propertyNames = typeof(AmazonJewelryDataFeedItemV3_Dto)
                                .GetProperties()
                                .Select(p => p.Name)
                                .Where(name => name != "Id")
                                .ToArray();

            foreach (var data in listData.Result)
            {
                rowIndex++;
                columnIndex = 1;
                foreach (var propertyName in propertyNames)
                {
                    var value = typeof(AmazonJewelryDataFeedItemV3_Dto)
                                     .GetProperty(propertyName)
                                     .GetValue(data, null);
                    ws.Cells[rowIndex, columnIndex].Value = value;
                    columnIndex++;
                }
            }
            var stream = new MemoryStream(await package.GetAsByteArrayAsync());
            byte[] bytes = stream.ToArray();
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            return File(bytes, contentType, $"Amazon_Jewelry_Data_Export_{DateTime.Now}.csv");
        }


        /// <summary>
        /// lấy ra tất cả item_sku đang hiển thị theo Q- hoặc S-
        /// </summary>
        /// <remarks>
        /// tìm theo 'Q' hoặc 'S'
        /// </remarks>
        /// <param name="item_sku_category"></param>
        /// <returns></returns>
        [HttpGet("GetAllItemSKU")]
        public async Task<IActionResult> GetAllItemSKU(string item_sku_category)
        {
            var result = await _service.GetAllItemSKU(item_sku_category);
            return Ok(result);
        }
        /// <summary>
        /// Gửi lên 1 list các item_sku mà bạn muốn xóa
        /// </summary>
        /// <param name="listSKU"></param>
        /// <returns></returns>
        [HttpPut("RemoveMultipleProduct")]
        public async Task<IActionResult> RemoveMultipleProduct([FromBody] List<string> listSKU)
        {
            var result = await _service.RemoveMultipleProduct(listSKU);
            return Ok(result);
        }

        /// <summary>
        /// lấy ra 1 số trường cần thiết cho proccess Map to Walmart
        /// </summary>
        /// <remarks>
        /// item_sku
        /// external_product_id_type
        /// external_product_id
        ///item_name
        ///brand_name
        ///item_package_quantity
        ///product_description
        ///bullet_point1
        ///bullet_point2
        ///bullet_point3
        ///bullet_point4
        ///bullet_point5
        ///bullet_point6
        ///bullet_point7
        ///bullet_point8...
        /// </remarks>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("GetDataForWalmart")]
        public async Task<IActionResult> GetDataForWalmart([FromQuery] PaginationParam pagination, [FromQuery] string? keyword)
        {
            return Ok(await _service.GetDataForWalmart(pagination, keyword, true));
        }
        /// <summary>
        /// lấy ra Sku, price, quantity để update giá và số lượng của processor Mapping
        /// </summary>
        /// <remarks>
        /// lấy ra Sku, price, quantity để update giá và số lượng của processor Mapping
        /// </remarks>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet("GetDataForAmazon")]
        public async Task<IActionResult> GetDataForAmazon([FromQuery] PaginationParam pagination, [FromQuery] string? keyword)
        {
            return Ok(await _service.GetDataForAmazon(pagination, keyword, true, false));
        }
    }
}