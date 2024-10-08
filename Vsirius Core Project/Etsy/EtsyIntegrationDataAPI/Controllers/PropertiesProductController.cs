using EtsyIntegrationDataAPI._Services.Interfaces;
using EtsyIntegrationDataAPI.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EtsyIntegrationDataAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesProductController : ControllerBase
    {
        private readonly IPropertiesProductService _propertiesProductService;

        public PropertiesProductController(IPropertiesProductService propertiesProductService)
        {
            _propertiesProductService = propertiesProductService;
        }

        /// <summary>
        /// Seed thuộc tính theo quy định của Etsy
        /// </summary>
        /// <remarks>
        ///     case "bracelet":
        ///         return 1193; <br/>
        ///     case "keychain":
        ///         return 164; <br/>
        ///     case "necklace":
        ///         return 1217; <br/>
        ///     case "earring":
        ///         return 1203; <br/>
        ///     case "watches":
        ///         return 1249; <br/>
        ///     case "ring":
        ///         return 1231; <br/>
        ///     case "charm":
        ///         return 1197; <br/>
        ///     default:
        ///         return 1179; <br/>
        /// </remarks>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SeedDataPropertiesByTaxonomy(int taxonomy_id)
        {
            var res = await _propertiesProductService.SeedDataPropertiesByTaxonomy(taxonomy_id);
            return Ok(res);
        }

        /// <summary>
        /// Cập nhật thêm các properties cho sản phẩm đã listing trên Etsy
        /// </summary>
        /// <returns></returns>
        [HttpPut()]
        [ProducesResponseType(typeof(OperationResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateListingPropertiesEtsy()
        {
            var res = await _propertiesProductService.UpdateListingPropertiesEtsy();
            return Ok(res);
        }
    }
}
