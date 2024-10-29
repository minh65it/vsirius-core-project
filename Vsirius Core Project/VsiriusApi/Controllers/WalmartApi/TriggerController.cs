using Microsoft.AspNetCore.Mvc;
using WalmartIntegrationDataApi._JobServices.Interfaces;

namespace VsiriusApi.Controllers.SystemApi
{
    [ApiExplorerSettings(GroupName = "Walmart")]
    [Route("api/[controller]")]
    [ApiController]
    public class TriggerController : ControllerBase
    {
        private static bool IsGetOrderWalmartUsed = false;
        private static bool IsGetReturnOrderWalmartUsed = false;
        private static bool IsUpdateTrackingWalmartOrderUsed = false;
        private static bool IsSendWalmartOrderToQgoldUsed = false;

        private static bool IsApplyShippingUsed = false;
        private static bool IsUpdatePriceUsed = false;
        private static bool IsUpdateInventoryUsed = false;
        private static bool IsUploadProductUsed = false;
        private static bool IsMappPrductUsed = false;

        private readonly IGetOrderWalmartService _getOrderService;
        private readonly IGetReturnOrderService _getReturnOrderService;
        private readonly ITrackingWalmartOrderService _trackingOrderService;
        private readonly ISubmitOrderToQgoldService _sendOrderService;

        private readonly IApplyShippingTemplateService _shippingService;
        private readonly IUpdateInventoryService _inventoryService;
        private readonly IUpdatePriceService _priceService;
        private readonly IUploadProductToSellerService _uploadProductService;
        private readonly IMapVsiriusToWalmartService _mapProductService;

        public TriggerController(
            IGetOrderWalmartService getOrderService,
            IGetReturnOrderService getReturnOrderService,
            ITrackingWalmartOrderService trackingOrderService,
            ISubmitOrderToQgoldService sendOrderService,
            IUploadProductToSellerService uploadProductService,
            IUpdatePriceService priceService,
            IUpdateInventoryService inventoryService,
            IApplyShippingTemplateService shippingService,
            IMapVsiriusToWalmartService mapProductService)
        {
            _getOrderService = getOrderService;
            _getReturnOrderService = getReturnOrderService;
            _trackingOrderService = trackingOrderService;
            _sendOrderService = sendOrderService;
            _uploadProductService = uploadProductService;
            _priceService = priceService;
            _inventoryService = inventoryService;
            _shippingService = shippingService;
            _mapProductService = mapProductService;
        }

        [HttpPost("GetOrderWalmart")]
        public async Task<IActionResult> GetOrderWalmart()
        {
            if (IsGetOrderWalmartUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsGetOrderWalmartUsed = true;
                await _getOrderService.GetOrderWalmart();
                IsGetOrderWalmartUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("GetReturnOrderWalmart")]
        public async Task<IActionResult> GetReturnOrderWalmart()
        {
            if (IsGetReturnOrderWalmartUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsGetReturnOrderWalmartUsed = true;
                await _getReturnOrderService.GetReturnOrder();
                IsGetReturnOrderWalmartUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("UpdateTrackingWalmartOrder")]
        public async Task<IActionResult> UpdateTrackingWalmartOrder()
        {
            if (IsUpdateTrackingWalmartOrderUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsUpdateTrackingWalmartOrderUsed = true;
                await _trackingOrderService.UpdateTrackingWalmartOrder();
                IsUpdateTrackingWalmartOrderUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("SubmitOrderToQgold")]
        public async Task<IActionResult> SendWalmartOrderToQgold()
        {
            if (IsSendWalmartOrderToQgoldUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsSendWalmartOrderToQgoldUsed = true;
                await _sendOrderService.SendWalmartOrderToQgold();
                IsSendWalmartOrderToQgoldUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("MapProduct")]
        public async Task<IActionResult> MapProduct()
        {
            if (IsMappPrductUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsMappPrductUsed = true;
                await _mapProductService.MapVsiriusToWalmart();
                IsMappPrductUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("ApplyShippingTemplate")]
        public async Task<IActionResult> ApplyShippingTemplate()
        {
            if (IsApplyShippingUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsApplyShippingUsed = true;
                await _shippingService.ApplyShippingTemplate();
                IsApplyShippingUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("UpdateInventory")]
        public async Task<IActionResult> UpdateInventory()
        {
            if (IsUpdateInventoryUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsUpdateInventoryUsed = true;
                await _inventoryService.UpdateInventory();
                IsUpdateInventoryUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice()
        {
            if (IsUpdatePriceUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsUpdatePriceUsed = true;
                await _priceService.UpdatePrice();
                IsUpdatePriceUsed = false;
                return Ok("Trigger success.");
            }
        }

        [HttpPost("UploadProductToSeller")]
        public async Task<IActionResult> UploadProductToSeller()
        {
            if (IsUploadProductUsed)
            {
                return BadRequest("Process is running.");
            }
            else
            {
                IsUploadProductUsed = true;
                await _uploadProductService.UploadProductToSeller();
                IsUploadProductUsed = false;
                return Ok("Trigger success.");
            }
        }
    }
}
