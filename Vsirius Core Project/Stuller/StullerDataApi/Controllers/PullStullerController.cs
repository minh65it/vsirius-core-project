using Microsoft.AspNetCore.Mvc;
using StullerDataApi.Services.IServices;
using StullerDataApi.Services.Services;

namespace StullerDataApi.Controllers
{
    [Route("api/stuller/[controller]")]
    [ApiController]
    public class PullStullerController : ControllerBase
    {
        private readonly IPullStuller _pullStuller;
        private static bool IsRunning = false;

        public PullStullerController(IPullStuller pullStuller)
        {
            _pullStuller = pullStuller;
        }


        /// <summary>
        /// Pull new product from Stuller
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PullNewProduct()
        {
            if (IsRunning)
            {
                return BadRequest("PullNewProduct is running");
            }
            else
            {
                IsRunning = true;
                await _pullStuller.PullNewProduct();
                IsRunning = false;
                return Ok("PullNewProduct is done");

            }
        }
    }
}
