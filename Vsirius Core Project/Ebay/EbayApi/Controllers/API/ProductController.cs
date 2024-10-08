using EbayApi.Dtos;
using EbayApi.Services.Interfaces;
using EbayApi.Utils;
using System.Threading.Tasks;
using System.Web.Http;

namespace EbayApi.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IEbayProductService _productService;

        public ProductController(IEbayProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<PaginationUtil<ProductDto>> SearchProducts(
           [FromUri] SearchParam search,
           [FromUri] PaginationParam pagination)
        {
            if (pagination == null)
            {
                pagination = new PaginationParam
                {
                    PageSize = 1,
                    PageNumber = 1
                };
            }
            else
            {
                // Xử lý nếu PageSize hoặc PageNumber nhỏ hơn 1
                if (pagination.PageSize < 1)
                {
                    pagination.PageSize = 1;
                }
                else if (pagination.PageSize > 1000)
                {
                    pagination.PageSize = 1000;
                }

                if (pagination.PageNumber < 1)
                {
                    pagination.PageNumber = 1;
                }
            }
            return await _productService.SearchProducts(pagination, search);
        }

        [HttpGet]
        [Route("api/product/{sku}")]
        public async Task<ProductDto> GetProduct(string sku)
        {
            return await _productService.GetProduct(sku);
        }

        [HttpPost]
        public async Task<ResultUtil<ProductDto>> AddProduct(ProductDto product)
        {
            return await _productService.AddProduct(product);
        }

        [HttpPut]
        public async Task<ResultUtil<ProductDto>> UpdateProduct(ProductDto product)
        {
            return await _productService.UpdateProduct(product);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <remark>
        /// có thể xoá nhiều sản phẩm với format sku1|sku2|sku3|
        /// </remark>
        /// <param name="skus"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ResultUtil<string>> DeleteProduct(string skus)
        {
            return await _productService.DeleteProduct(skus);
        }
    }
}
