using EbayApi.JobServices;
using EbayApi.JobServices.Interfaces;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EbayApi.Controllers
{
    public class TriggerController : ApiController
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

        [HttpPost]
        [Route("api/trigger/UpdateTrackingOrder")]
        public async Task<HttpResponseMessage> UpdateTrackingOrder()
        {
            if (IsTrackingOrder)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Process is running.");
            }
            else
            {
                IsTrackingOrder = true;
                await _updateTrackingOrderService.UpdateTrackingOrder();
                IsTrackingOrder = false;
                return Request.CreateResponse(HttpStatusCode.OK, "Trigger success.");
            }
        }

        [HttpPost]
        [Route("api/trigger/SubmitOrderEbay")]
        public async Task<HttpResponseMessage> SubmitOrderEbay()
        {
            if (IsSubmitOrder)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Process is running.");
            }
            else
            {
                IsSubmitOrder = true;
                await _submitOrderService.SubmitOrderEbay();
                IsSubmitOrder = false;
                return Request.CreateResponse(HttpStatusCode.OK, "Trigger success.");
            }
        }

        [HttpPost]
        [Route("api/trigger/GetOrderEbay")]
        public async Task<HttpResponseMessage> GetOrderEbay()
        {
            if (IsGetOrderFromEbay)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Process is running.");
            }
            else
            {
                IsGetOrderFromEbay = true;
                await _getOrderService.GetOrderFromEbay();
                IsGetOrderFromEbay = false;
                return Request.CreateResponse(HttpStatusCode.OK, "Trigger success.");
            }
        }

        [HttpPost]
        [Route("api/trigger/UploadProductToEbayMarketplace")]
        public async Task<HttpResponseMessage> UploadProductToEbayMarketplace()
        {
            if (IsUploadProductToEbayMarketplace)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Process is running.");
            }
            else
            {
                IsUploadProductToEbayMarketplace = true;
                await _uploadService.UploadItemToEbayMarketplace();
                IsUploadProductToEbayMarketplace = false;
                return Request.CreateResponse(HttpStatusCode.OK, "Trigger success.");
            }
        }

        [HttpPost]
        [Route("api/trigger/MapPimToEbayProduct")]
        public async Task<HttpResponseMessage> MapPimToEbayProduct()
        {
            if (IsMapPimToEbayProduct)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Process is running.");
            }
            else
            {
                IsMapPimToEbayProduct = true;
                await _mapService.MapperPimProductToEbayProduct();
                IsMapPimToEbayProduct = false;
                return Request.CreateResponse(HttpStatusCode.OK, "Trigger success.");
            }
        }

        [HttpPost]
        [Route("api/trigger/UpdatePriceQuantiyEbayProduct")]
        public async Task<HttpResponseMessage> UpdatePriceQuantiyEbayProduct()
        {
            if (IsUpdateQuantityPrice)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Process is running.");
            }
            else
            {
                IsUpdateQuantityPrice = true;
                await _priceQuatityService.UpdatePriceAndQuantiyToMarketplace();
                IsUpdateQuantityPrice = false;
                return Request.CreateResponse(HttpStatusCode.OK, "Trigger success.");
            }
        }
    }
}
