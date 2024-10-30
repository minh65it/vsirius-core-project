using EbayCoreApi.JobServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace VsiriusApi.Controllers.EbayApi
{
    [ApiExplorerSettings(GroupName = "EbayCore")]
    [Route("api/[controller]")]
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private static bool IsUploadProductToEbayMarketplace = false;
        private static bool IsMapPimToEbayProduct = false;
        private static bool IsUpdateQuantityPrice = false;
        private static bool IsGetOrderFromEbay = false;
        private static bool IsSubmitOrder = false;
        private static bool IsTrackingOrder = false;

        private readonly IUpdatePriceAndQuantiyToMarketplaceService _priceQuatityService;
        private readonly IUploadItemToEbayMarketplaceService _uploadService;
        private readonly IMapPimToEbayProductService _mapService;
        private readonly IGetOrderEbayService _getOrderService;
        private readonly ISubmitOrderService _submitOrderService;
        private readonly IUpdateTrackingOrderService _updateTrackingOrderService;

        public TriggerController(
            IUploadItemToEbayMarketplaceService uploadService,
            IMapPimToEbayProductService mapService,
            IUpdatePriceAndQuantiyToMarketplaceService priceQuatityService,
            IGetOrderEbayService getOrderService,
            ISubmitOrderService submitOrderService,
            IUpdateTrackingOrderService updateTrackingOrderService
            )
        {
            _uploadService = uploadService;
            _mapService = mapService;
            _priceQuatityService = priceQuatityService;
            _getOrderService = getOrderService;
            _submitOrderService = submitOrderService;
            _updateTrackingOrderService = updateTrackingOrderService;
        }

        [HttpPost("UpdateTrackingOrder")]
        public async Task<IActionResult> UpdateTrackingOrder()
        {
            if (IsTrackingOrder)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsTrackingOrder = true;
                await _updateTrackingOrderService.UpdateTrackingOrder();
                IsTrackingOrder = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("SubmitOrderEbay")]
        public async Task<IActionResult> SubmitOrderEbay()
        {
            if (IsSubmitOrder)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsSubmitOrder = true;
                await _submitOrderService.SubmitOrderEbay();
                IsSubmitOrder = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("GetOrderEbay")]
        public async Task<IActionResult> GetOrderEbay()
        {
            if (IsGetOrderFromEbay)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsGetOrderFromEbay = true;
                await _getOrderService.GetOrderFromEbay();
                IsGetOrderFromEbay = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("UploadProductToEbayMarketplace")]
        public async Task<IActionResult> UploadProductToEbayMarketplace()
        {
            if (IsUploadProductToEbayMarketplace)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsUploadProductToEbayMarketplace = true;
                await _uploadService.UploadItemToEbayMarketplace();
                IsUploadProductToEbayMarketplace = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("MapPimToEbayProduct")]
        public async Task<IActionResult> MapPimToEbayProduct()
        {
            if (IsMapPimToEbayProduct)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsMapPimToEbayProduct = true;
                await _mapService.MapperPimProductToEbayProduct();
                IsMapPimToEbayProduct = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("UpdatePriceQuantiyEbayProduct")]
        public async Task<IActionResult> UpdatePriceQuantiyEbayProduct()
        {
            if (IsUpdateQuantityPrice)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsUpdateQuantityPrice = true;
                await _priceQuatityService.UpdatePriceAndQuantiyToMarketplace();
                IsUpdateQuantityPrice = false;
                return Ok("Trigger success.");
            }
        }
    }
}
