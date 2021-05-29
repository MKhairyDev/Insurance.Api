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
    internal class ProductControllerTests
    {
        [SetUp]
        public void TestInitialization()
        {
            _productServiceMock = new Mock<IProductService>();
            _productCalculatorMock = new Mock<IInsuranceCalculator<ProductDto>>();
            _productController = new ProductController(productService: _productServiceMock.Object,
                productCalculator: _productCalculatorMock.Object);
        }

        private Mock<IProductService> _productServiceMock;
        private Mock<IInsuranceCalculator<ProductDto>> _productCalculatorMock;

        private ProductController _productController;

        [Test]
        public async Task CalculateInsurance_WhenProductIdNotExist_ShouldReturn_HttpNotFound()
        {
            //Arrange
            _productServiceMock.Setup(x => x.GetProductWithProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(value: (ProductDto) null);

            //Act
            var productId = 1;
            var actualResult = await _productController.CalculateInsurance(productId: productId);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(actual: actualResult.Result);
            Assert.AreEqual(expected: productId, actual: ((NotFoundObjectResult) actualResult.Result).Value);
        }

        [Test]
        public async Task CalculateInsurance_WhenProductIdExist_ShouldReturn_OkObjectResult()
        {
            //Arrange
            const float expectedInsuranceValue = 10;
            _productServiceMock.Setup(x => x.GetProductWithProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(value: new ProductDto());
            _productCalculatorMock.Setup(x => x.Calculate(It.IsAny<ProductDto>()))
                .Returns(value: expectedInsuranceValue);

            //Act
            var productId = 1;
            var actualResult = await _productController.CalculateInsurance(productId: productId);

            //Assert
            Assert.IsInstanceOf<ActionResult<float>>(actual: actualResult);
            Assert.IsInstanceOf<OkObjectResult>(actual: actualResult.Result);
            var actualValue = ((OkObjectResult) actualResult.Result).Value;
            Assert.AreEqual(expected: expectedInsuranceValue, actual: (float) actualValue);
        }
    }
}