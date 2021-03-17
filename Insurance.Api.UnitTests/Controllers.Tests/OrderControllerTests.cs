using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Controllers;
using Insurance.Api.Models;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.Controllers.Tests
{
    [TestFixture]
    internal class OrderControllerTests
    {
        [SetUp]
        public void TestInitialization()
        {
            _productServiceMock = new Mock<IProductService>();
            _orderCalculatorMock = new Mock<IInsuranceCalculator<List<ProductDto>>>();
            _productController = new OrderController(productService: _productServiceMock.Object,
                insuranceCalculator: _orderCalculatorMock.Object);
        }

        private Mock<IProductService> _productServiceMock;
        private Mock<IInsuranceCalculator<List<ProductDto>>> _orderCalculatorMock;

        private OrderController _productController;

        [Test]
        public async Task CalculateInsurance_WhenProductIsdNotExist_ShouldReturn_HttpNotFound()
        {
            //Arrange
            _productServiceMock.Setup(x => x.GetProductsWithProductTypeAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(value: It.IsAny<List<ProductDto>>());

            //Act
            var actualResult = await _productController.CalculateInsurance(productsId: new List<int> {1});

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(actual: actualResult.Result);
        }

        [Test]
        public async Task CalculateInsurance_WhenProductIdsExist_ShouldReturn_OkObjectResult()
        {
            //Arrange
            const float expectedInsuranceValue = 10;
            _productServiceMock.Setup(x => x.GetProductsWithProductTypeAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(value: new List<ProductDto>());
            _orderCalculatorMock.Setup(x => x.Calculate(It.IsAny<List<ProductDto>>()))
                .Returns(value: expectedInsuranceValue);

            //Act
            var actualResult = await _productController.CalculateInsurance(productsId: new List<int> {1});

            //Assert
            Assert.IsInstanceOf<ActionResult<float>>(actual: actualResult);
            Assert.IsInstanceOf<OkObjectResult>(actual: actualResult.Result);
            var actualValue = ((OkObjectResult) actualResult.Result).Value;
            Assert.AreEqual(expected: expectedInsuranceValue, actual: (float) actualValue);
        }
    }
}