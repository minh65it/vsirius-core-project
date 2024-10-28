using Microsoft.AspNetCore.Mvc;
using QgoldIntegrationDataApi._Services.Interfaces;
using QgoldIntegrationDataApi.Dtos.Qgold;
using QgoldIntegrationDataApi.Helpers.Params;
using QgoldIntegrationDataApi.Helpers.Ultilities;

namespace QgoldIntegrationDataApi.Controllers
{
    [Route("api/qgold/[controller]")]
    [ApiController]
    public class QgoldCsvDataController : ControllerBase
    {
        private readonly IQgoldCsvDataService _service;
        private readonly IKeyService _keyService;

        public QgoldCsvDataController(IQgoldCsvDataService service, IKeyService keyService)
        {
            _service = service;
            _keyService = keyService;
        }
        //[HttpGet("GetBookList")]
        //public async Task<IActionResult> GetBookList([FromHeader] string clientId, [FromHeader] string clientSecret)
        //{
        //    if (_keyService.ValidateCredentials(clientId, clientSecret))
        //    {
        //        return Ok(await _service.GetBookList());
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

        //[DisableRequestSizeLimit]
        //[HttpPost("GenDataByExcel")]
        //public async Task<IActionResult> GenDataByExcel(IFormFile fileExcel, [FromHeader] string clientId, [FromHeader] string clientSecret)
        //{

        //    if (_keyService.ValidateCredentials(clientId, clientSecret))
        //    {
        //        var result = await _service.GenDataByExcel(fileExcel);
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

        /// <summary>
        /// Get data qgold
        /// </summary>
        /// <remarks>
        /// Lấy data của qgold
        /// QgoldApi: "90b34499-b58e-4f80-ab17-0c1da8a139d4-QgoldApi"
        /// </remarks>
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData([FromQuery] PaginationParam pagination,[FromQuery] QgoldCsvDataFillter dataFillter,
                    [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                var result = await _service.GetData(pagination, dataFillter);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        //[HttpPost("Add")]
        //public async Task<IActionResult> Add(QgoldCsvData_Dto dto, [FromHeader] string clientId, [FromHeader] string clientSecret)
        //{
        //    if (_keyService.ValidateCredentials(clientId, clientSecret))
        //    {
        //        dto.isDeleted = false;
        //        var result = await _service.Create(dto);
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

        //[HttpPut("Delete")]
        //public async Task<IActionResult> Delete(QgoldCsvData_Dto dto, [FromHeader] string clientId, [FromHeader] string clientSecret)
        //{
        //    if (_keyService.ValidateCredentials(clientId, clientSecret))
        //    {
        //        dto.isDeleted = true;
        //        return Ok(await _service.Delete(dto));
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

        //[HttpPut("Update")]
        //public async Task<IActionResult> Update(QgoldCsvData_Dto dto, [FromHeader] string clientId, [FromHeader] string clientSecret)
        //{
        //    if (_keyService.ValidateCredentials(clientId, clientSecret))
        //    {
        //        return Ok(await _service.Update(dto));
        //    }
        //    else
        //    {
        //        return Unauthorized();
        //    }
        //}

        /// <summary>
        /// Create and Update data Qgold
        /// </summary>
        /// <remarks>
        /// Thêm sửa data qgold, api này nhận list data qgold
        /// </remarks>
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [DisableRequestSizeLimit]
        [HttpPost("CreateAndUpdateDataQgold")]
        public async Task<IActionResult> CreateAndUpdateDataQgold(List<QgoldData> dto, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                return Ok(await _service.CreateAndUpdateDataQgold(dto));
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Get data item qgold 
        /// </summary>
        /// <remarks>
        /// Lấy dữ liệu item của qgold
        /// </remarks>
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [HttpGet("GetDataQgoldItem")]
        public async Task<IActionResult> GetDataQgoldItem([FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                var result = await _service.GetDataQgoldItem();
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Delete data qgold
        /// </summary>
        /// <remarks>
        /// Xóa data của qgold, api này nhận list item của qgold
        /// </remarks>   
        /// <param name="clientId">Nhập Client Id</param>
        /// <param name="clientSecret">Nhập Client Secret</param>
        /// <returns></returns>
        [HttpPost("DeleteDataQgold")]
        public async Task<IActionResult> DeleteDataQgold(List<string> dto, [FromHeader] string clientId, [FromHeader] string clientSecret)
        {
            if (_keyService.ValidateCredentials(clientId, clientSecret))
            {
                var result = await _service.DeleteDataQgold(dto);
                return Ok(result);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
