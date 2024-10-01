using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductApi._Services.Interfaces;
using ProductApi._Services.Services;
using ProductApi._Services.Services.Tests;
using ProductApi.Controllers;
using ProductApi.Dtos;
using ProductApi.Helpers.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Controllers.Tests
{
    [TestClass()]
    public class VsiriusHomeProductControllerTests
    {
        private readonly VsiriusHomeProductController _controller;
        private readonly IVsiriusHomeProductService _vsiriusHomeProductService;

        public VsiriusHomeProductControllerTests()
        {
            _vsiriusHomeProductService = new VsiriusHomeProductServiceTests();
            _controller = new VsiriusHomeProductController(_vsiriusHomeProductService);
        }

        [TestMethod()]
        public void GetAllHomeProductTest()
        {
            VsiriusHomeProductParams param = new VsiriusHomeProductParams();
            var okResult = _controller.GetAllHomeProduct(param);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as List<VsiriusProductDto>;
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod()]
        public void GetAllHomeProductBackendTest()
        {
            var okResult = _controller.GetAllHomeProductBackend();
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as List<VsiriusHomeProductDto>;
            Assert.AreEqual(result.Count, 1);
        }

        [TestMethod()]
        public void GetRelatedProductTest()
        {
            VsiriusHomeProductParams param = new VsiriusHomeProductParams();
            var okResult = _controller.GetRelatedProduct(param);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as List<VsiriusProductDto>;
            Assert.AreEqual(result.Count, 5);
        }

        [TestMethod()]
        public void CreateUpdateHomeProductTest()
        {
            VsiriusHomeProductDto dto = new VsiriusHomeProductDto();
            var okResult = _controller.CreateUpdateHomeProduct(dto);
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as OperationResult;
            Assert.AreEqual(result.Message, "Update successful");
        }

        [TestMethod()]
        public void RemoveHomeProductTest()
        {
            var okResult = _controller.RemoveHomeProduct("S-123626:6024:P");
            var okResultObject = okResult.Result as OkObjectResult;
            var result = okResultObject.Value as OperationResult;
            Assert.AreEqual(result.Message, "Successful delete!!!");
        }
    }
}