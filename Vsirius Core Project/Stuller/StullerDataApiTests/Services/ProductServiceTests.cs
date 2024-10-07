using StullerDataApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using StullerDataApi.Data;
using StullerDataApi.IRepository;
using StullerApiClient.Models;
using StullerDataApi.Repository;
using StullerDataApi.Params;
using StullerDataApi.Utilities;
using StullerDataApi.Services.IServices;
using StullerDataApi.Services.Services;

namespace StullerDataApi.Services.Tests
{
    [TestClass()]
    public class ProductServiceTests
    {
        private readonly IMongoDbSettings _connectionSetting;
        private readonly IMongoContext _context;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _connectionSetting = new MongoDbSettings()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "StullerApiClient",
            };
            _connectionSetting.DatabaseName = "StullerApiClient";
            _context = new MongoContext(_connectionSetting);
            _productRepository = new ProductRepository(_context);
            _productService = new ProductService(_productRepository);
        }

        //[TestMethod()]
        //public void GetProductTest()
        //{
        //    ProductsRequestPostFilter filter = new ProductsRequestPostFilter();

        //    filter.Page = 1;
        //    filter.PageSize = 500;

        //    var result = _productService.GetProduct(filter);
        //    Assert.IsNotNull(result);
        //}

        [TestMethod()]
        public async Task DetailProductTest()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                SKU = "SKU",
                Description = "Description",
                ShortDescription = "ShortDescription"
            };
            await _productRepository.Add(product);

            // Act
            var result = await _productService.DetailProduct("SKU");

            // Assert
            Assert.AreEqual(product.Id, result.Id);
            Assert.AreEqual(product.SKU, result.SKU);
            Assert.AreEqual(product.Description, result.Description);

            // Clean up
            await _productRepository.Remove("SKU");
        }

        [TestMethod()]
        public async Task UpdateProductTest()
        {
            // Arrange
            var product1 = new Product
            {
                Id = 2,
                SKU = "SKU2",
                Description = "Description",
                ShortDescription = "ShortDescription",
            };
            await _productRepository.Add(product1);

            // Act
            var productUpdate = new Product
            {
                Id = 2,
                SKU = "SKU2",
                Description = "Description Update",
                ShortDescription = "ShortDescription Update"
            };
            var result = await _productService.UpdateProduct("SKU2", productUpdate);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(productUpdate.Id, 2);
            Assert.AreEqual(productUpdate.SKU, "SKU2");
            Assert.AreEqual(productUpdate.Description, "Description Update");

            // Clean up
            await _productRepository.Remove("SKU2");
        }

        [TestMethod()]
        public async Task DeleteProductTest()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                SKU = "SKUDelete",
                Description = "Description",
                ShortDescription = "ShortDescription"
            };
            await _productRepository.Add(product);
            var result = await _productService.DeleteProduct("SKUDelete");
            Assert.IsTrue(result);

        }

        [TestMethod()]
        public async Task FilterTest()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                SKU = "FilterTest",
                Description = "Description",
                ShortDescription = "ShortDescription",
                ProductType = "abc",
            };
            var product2 = new Product
            {
                Id = 2,
                SKU = "FilterTest2",
                Description = "Description2",
                ShortDescription = "ShortDescription2",
                ProductType = "ProductType2"
            };
            await _productRepository.Add(product);
            await _productRepository.Add(product2);

            // Act
            ProductParams param = new ProductParams()
            {
                ProductType = "abc",
            };
            PaginationParam page = new PaginationParam()
            {
                PageNumber = 1,
                PageSize = 10,
            };
            var result = await _productService.Filter(param, page);

            // Assert
            Assert.IsTrue(result.TotalCount == 1);
            Assert.AreEqual(result.Data[0].Id, 1);

            // Clean up
            await _productRepository.Remove("FilterTest");
            await _productRepository.Remove("FilterTest2");
        }
    }
}