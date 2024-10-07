using Microsoft.AspNetCore.Mvc;
using QgoldIntegrationDataApi._JobServices.Interfaces;

namespace QgoldIntegrationDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QgoldProcessTriggerController : ControllerBase
    {
        private readonly IPullQgoldTriggerService _servicePullQgold;
        private readonly IMappingQgoldToVsiriusService _serviceMapping;
        private static bool IsPullQgoldUsed = false;
        private static bool IsMappingUsed = false;

        public QgoldProcessTriggerController(
            IPullQgoldTriggerService servicePullQgold,
            IMappingQgoldToVsiriusService serviceMapping
            )
        {
            _servicePullQgold = servicePullQgold;
            _serviceMapping = serviceMapping;
        }

        [HttpPost("PullQgold")]
        public async Task<IActionResult> PullQgold()
        {
            if (IsPullQgoldUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsPullQgoldUsed = true;
                await _servicePullQgold.PullQgold();
                IsPullQgoldUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("MappingQgoldToVsirius")]
        public async Task<IActionResult> MappingQgoldToVsirius()
        {
            if (IsMappingUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsMappingUsed = true;
                await _serviceMapping.MappingQgoldToVsirius();
                IsMappingUsed = false;
                return Ok("Process run finish.");
            }
        }
    }
}
