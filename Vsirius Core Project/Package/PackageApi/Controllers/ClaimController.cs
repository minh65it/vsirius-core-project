using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PackageApi._Services.IServices;
using PackageApi.Dtos;
using PackageApi.Dtos.Claim;
using System.Data;

namespace PackageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }
        /// <summary>
        /// Lấy ra tất cả các claim
        /// </summary> 
        /// <remarks>
        /// Tham khảo quyền admin <br/>
        /// Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJBZG1pbiIsImV4cCI6MTcxMzA4MjgxNywiaXNzIjoiaHR0cHM6Ly92c2lyaXVzLmNvbS8iLCJhdWQiOiJodHRwczovL3ZzaXJpdXMuY29tLyJ9.zk6_lhlCT3n6x2ISmhiuyQgIBQnrEHeJeZXo-V6AxYk
        /// </remarks>
        /// <param name="paginationParam"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetClaim([FromQuery] PaginationParam paginationParam, [FromQuery] ClaimSearchParam search)
        {
            return Ok(await _claimService.GetClaim(paginationParam, search));
        }

        /// <summary>
        /// Submit 1 Claim
        /// </summary>
        /// <remarks>
        /// 0: ReturnPackage; 1: OrderDocuments; 3: ShippedPackage
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Submit([FromForm] ClaimParam model)
        {
            return Ok(await _claimService.Submit(model));
        }

        [HttpGet("detail")]
        public async Task<IActionResult> Detail(string id)
        {
            return Ok(await _claimService.GetDetail(id));
        }
        /// <summary>
        /// 0: ReturnPackage; 1: OrderDocuments; 3: ShippedPackage. KeyValuePair(int, string)
        /// </summary>
        /// <remarks>
        /// 0: ReturnPackage; 1: OrderDocuments; 3: ShippedPackage
        /// </remarks>
        /// <returns></returns>
        [HttpGet("DocTypes")]
        public IActionResult GetDocTypes()
        {
            return Ok(_claimService.GetDocTypes());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// Get all data with List RMA
        /// </remarks>
        /// <param name="rma"></param>
        /// <returns></returns>
        [HttpGet("GetAllByRMA")]
        public async Task<IActionResult> GetAllByRMA([FromHeader] List<string> rma)
        {
            return Ok(await _claimService.GetAllByRMA(rma));
        }
    }
}
