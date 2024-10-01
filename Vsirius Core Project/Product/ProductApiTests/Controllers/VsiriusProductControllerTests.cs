using ProductApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductApi._Services.Interfaces;
using ProductApi._Services.Services.Tests;
using ProductApi.Dtos;
using Microsoft.AspNetCore.Http;
using ProductApi.Helpers.Utilities;
using System.Text;
using Azure.Core;
using System.Net.Mime;

namespace ProductApi.Controllers.Tests
{
    [TestClass()]
    public class VsiriusProductControllerTests
    {
        private readonly VsiriusProductController _controller;
        private readonly IVsiriusProductService _vsiriusProductService;
        public IFormFile _file;

        public VsiriusProductControllerTests()
        {
            _vsiriusProductService = new VsiriusProductServiceTests();
            _controller = new VsiriusProductController(_vsiriusProductService);
        }

        [TestMethod()]
        public void CreateProductTest()
        {
            VsiriusProductDto dto = new VsiriusProductDto();
            var okResult = _controller.CreateProduct(dto);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as OperationResult;
            Assert.AreEqual(result.Success, true);
        }

        [TestMethod()]
        public void ImportFileTest()
        {
            var okResult = _controller.ImportFile(_file);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as OperationResult;
            Assert.AreEqual(result.Success, false);
        }

        [TestMethod()]
        public void GetProductTest()
        {
            var okResult = _controller.GetProduct("S-123626:6024:P");
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as VsiriusProductDto;
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void RemoveProductTest()
        {
            var okResult = _controller.RemoveProduct("S-123626:6024:P");
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as OperationResult;
            Assert.AreEqual(result.Success, true);
        }

        [TestMethod()]
        public void UpdateProductTest()
        {
            VsiriusProductDto dto = new VsiriusProductDto();
            dto.item_sku = "S-123626:6024:P";
            var okResult = _controller.UpdateProduct(dto);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as OperationResult;
            Assert.AreEqual(result.Success, true);
        }

        [TestMethod()]
        public void GetFilterTest()
        {
            var okResult = _controller.GetFilter("ring");
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as List<VsiriusFilterParamDto>;
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod()]
        public void GetValueForAddTest()
        {
            var okResult = _controller.GetValueForAdd("ring");
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as List<VsiriusFilterParamDto>;
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod()]
        public void GetAllTest()
        {
            List<VsiriusFilterParamDto> filter = new List<VsiriusFilterParamDto>();
            PaginationParam pagination = new PaginationParam();

            var okResult = _controller.GetAll(filter, pagination, null);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as PaginationUtility<VsiriusProductDto>;
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.Count, 5);
        }

        [TestMethod()]
        public void Layer1Test()
        {
            List<VsiriusFilterParamDto> filter = new List<VsiriusFilterParamDto>();
            PaginationParam pagination = new PaginationParam();

            var okResult = _controller.Layer1("jewelryset");
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as PaginationUtility<VsiriusProductDto>;
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.Count, 5);
        }

        [TestMethod()]
        public void GetAllItemSKUTest()
        {
            var okResult = _controller.GetAllItemSKU("S");
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as List<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result[0], "S-123626:6021:P");
        }

        [TestMethod()]
        public void RemoveMultipleProductTest()
        {
            List<string> listTest = new List<string>()
            {
                "S-123626:6021:P", "S-123626:6022:P"
            };
            var okResult = _controller.RemoveMultipleProduct(listTest);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as OperationResult;
            Assert.AreEqual(result.Success, true);
        }
    }
}