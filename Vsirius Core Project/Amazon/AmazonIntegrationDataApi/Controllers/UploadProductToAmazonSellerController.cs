using AmazonIntegrationDataApi._Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmazonIntegrationDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadProductToAmazonSellerController : ControllerBase
    {
        private readonly IUploadProductToAmazonSeller _ploadProductToAmazonSeller;
        private static bool IsUploadProductToAmazonRunning = false;
        private static bool IsUpdateInventory = false;
        private static bool IsUpdatePrice = false;
        private static bool IsRemoveProduct = false;

        public UploadProductToAmazonSellerController(IUploadProductToAmazonSeller ploadProductToAmazonSeller)
        {
            _ploadProductToAmazonSeller = ploadProductToAmazonSeller;
        }

        /// <summary>
        /// Trigger upload product to Amazon Seller
        /// </summary>
        /// <returns></returns>
        [HttpPost("UploadProductToAmazon")]
        public async Task<IActionResult> UploadProductToAmazon()
        {
            if (IsUploadProductToAmazonRunning)
            {
                return BadRequest("UploadProductToAmazon is running");
            }
            else
            {
                IsUploadProductToAmazonRunning = true;
                await _ploadProductToAmazonSeller.UploadProductToAmazon();
                IsUploadProductToAmazonRunning = false;
                return Ok("UploadProductToAmazon is done");

            }
        }

        /// <summary>
        /// Trigger update inventory in Amazon Seller
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdateInventory")]
        public async Task<IActionResult> UpdateInventory()
        {
            if (IsUpdateInventory)
            {
                return BadRequest("UpdateInventory is running");
            }
            else
            {
                IsUpdateInventory = true;
                await _ploadProductToAmazonSeller.UpdateInventory();
                IsUpdateInventory = false;
                return Ok("UpdateInventory is done");

            }
        }

        /// <summary>
        /// Trigger update Price in Amazon Seller
        /// </summary>
        /// <returns></returns>
        [HttpPost("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice()
        {
            if (IsUpdatePrice)
            {
                return BadRequest("UpdatePrice is running");
            }
            else
            {
                IsUpdatePrice = true;
                await _ploadProductToAmazonSeller.UpdatePrice();
                IsUpdatePrice = false;
                return Ok("UpdatePrice is done");

            }
        }

        /// <summary>
        /// Remove product in Amazon Seller
        /// </summary>
        /// <remarks>
        /// xóa các sản phẩm vi phạm chính sách trên amazon
        /// </remarks>
        /// <returns></returns>
        [HttpPost("RemoveProduct")]
        public async Task<IActionResult> RemoveProduct()
        {
            if (IsRemoveProduct)
            {
                return BadRequest("RemoveProduct is running");
            }
            else
            {
                IsRemoveProduct = true;
                await _ploadProductToAmazonSeller.RemoveProduct();
                IsRemoveProduct = false;
                return Ok("RemoveProduct is done");

            }
        }
    }
}
