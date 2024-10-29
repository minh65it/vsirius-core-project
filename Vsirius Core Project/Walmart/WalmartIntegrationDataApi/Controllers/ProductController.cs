using Microsoft.AspNetCore.Mvc;
using WalmartIntegrationDataApi.Common;
using WalmartIntegrationDataApi._Services.Interfaces;
using WalmartIntegrationDataApi.Dtos.Product;
using WalmartIntegrationDataApi.Common.Helpers;

namespace WalmartIntegrationDataApi.Controllers
{
    [Route("api/walmart/[controller]")]
    [ApiController]
    [CustomAuthen]
    public class ProductController : ControllerBase
    {
        private readonly IProductWalmartService _service;
        private OperationResult _result;

        public ProductController(IProductWalmartService service, OperationResult result)
        {
            _service = service;
            _result = result;
        }
        [HttpGet("GetAllId")]
        public async Task<IActionResult> GetAllId([FromHeader] string? Secret)
        {
            _result = await _service.GetAllId();
            return Ok(_result);
        }

        /// <summary>
        /// my summary
        /// </summary>
        /// <remarks>
        /// remark goes here.
        /// </remarks>
        /// <param name="param"></param>
        /// <param name="Secret">Token để truy cập API</param>
        /// <return>Returns comment</return>
        /// <response code="200">Ok</response>
        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam param, [FromHeader] string? Secret)
        {
            _result = await _service.GetAll(param);
            return Ok(_result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WalmartDTO data, [FromHeader] string? Secret)
        {
            _result = await _service.Create(data);
            //_result = await _service.GetAll();

            return Ok(_result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(WalmartDTO data, [FromHeader] string? Secret)
        {
            _result = await _service.Update(data);
            //_result = await _service.GetAll();

            return Ok(_result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(List<string> id, [FromHeader] string? Secret)
        {
            _result = await _service.Delete(id);
            //_result = await _service.GetAll();

            return Ok(_result);
        }

        /// <summary>
        /// Import file
        /// File xlsx,xlsm,csv mới được import
        /// dùng để update thông tin cơ bản của item theo bulkitem walmart template
        /// </summary>      
        [HttpPost("Import")]
        public async Task<IActionResult> Import(IFormFile file, [FromHeader] string? Secret)
        {
            _result = await _service.ImportFile(file);
            //_result = await _service.GetAll();

            return Ok(_result);
        }

        /// <summary>
        /// Export thông tin cơ bản của product để bulkupload item lên walmart
        /// </summary>
        /// <param name="param"></param>
        /// <param name="Secret"></param>
        /// <returns></returns>
        [HttpGet("Export")]
        public async Task<IActionResult> Export([FromQuery] PaginationParam param, [FromHeader] string? Secret)
        {
            _result = await _service.Export(param);
            HttpContext.Response.Headers.Add("TotalPage", _result.Message);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Data{DateTime.Now:yyyyMMdd_hhmmssfff}.xlsx";

            return File((byte[])_result.Data, contentType, fileName);

        }
        /// <summary>
        /// Endpoint dùng để cập nhật số lượng
        /// </summary>
        /// <param name="param"></param>
        /// <param name="Secret"></param>
        /// <returns></returns>
        [HttpGet("ExportQuantity")]
        public async Task<IActionResult> ExportQuantity([FromQuery] PaginationParam param, [FromHeader] string? Secret)
        {
            _result = await _service.ExportQuantity(param);
            HttpContext.Response.Headers.Add("TotalPage", _result.Message);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Quantity{DateTime.Now:yyyyMMdd_hhmmssfff}.xlsx";

            return File((byte[])_result.Data, contentType, fileName);

        }

        /// <summary>
        /// Endpoint dùng để cập nhật giá
        /// </summary>
        /// <param name="param"></param>
        /// <param name="Secret"></param>
        /// <returns></returns>
        [HttpGet("ExportPrice")]
        public async Task<IActionResult> ExportPrice([FromQuery] PaginationParam param, [FromHeader] string? Secret)
        { 

            _result = await _service.ExportPrice(param);
            HttpContext.Response.Headers.Add("TotalPage", _result.Message);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = $"Price{DateTime.Now.ToString("yyyyMMdd_hhmmssfff")}.xlsm";

            return File((byte[])_result.Data, contentType, fileName);

        }

        [HttpPost("BulkInsertUpdate")]
        public async Task<IActionResult> BulkInsertUpdate([FromBody] List<WalmartDTO> lstData, [FromHeader] string? Secret)
        {
            _result = await _service.BulkInsertUpdate(lstData);
            //_result = await _service.GetAll();

            return Ok(_result);
        }

        /// <summary>
        /// Get inventory and price
        /// </summary>
        /// <remarks>
        /// fecb6608f0f3c8d512ee61adf930793435ac2926e2e5b4edc297ccbd6be5c500a7345628691eb2f7c41f8c378d14031dd772d69f32fcac50fe90897653998b39
        /// </remarks>
        /// <param name="param"></param>
        /// <param name="Secret"></param>
        /// <returns></returns>
        [HttpGet("GetPriceInventory")]
        public async Task<IActionResult> GetPriceInventory([FromQuery] PaginationParam param, [FromHeader] string? Secret)
        {
            var  _result = await _service.GetPriceInventory(param);
            //_result = await _service.GetAll();

            return Ok(_result);
        }

    }
}
