using System.Collections.Generic;
using System.Threading.Tasks;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Configuration;
using Insurance.Api.Controllers;
using Insurance.Api.External.Models;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.Controllers.Tests
{
    [TestFixture]
    internal class OrderControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private Mock<IInsuranceCalculator<List<ProductDto>>> _orderCalculatorMock ;

        private OrderController _productController;

        [SetUp]
        public void TestInitialization()
        {
            _productServiceMock = new Mock<IProductService>();
            _orderCalculatorMock = new Mock<IInsuranceCalculator<List<ProductDto>>>();
            _productController = new OrderController(_productServiceMock.Object, _orderCalculatorMock.Object);
        }

        [Test]
        public async Task CalculateInsurance_WhenProductIsdNotExist_ShouldReturn_HttpNotFound()
        {
            //Arrange
            _productServiceMock.Setup(x => x.GetProductsWithProductTypeAsync(It.IsAny<List<int>>()))
                .ReturnsAsync((It.IsAny< List<ProductDto> >()));

            //Act
            var actualResult = await _productController.CalculateInsurance(new List<int>{ 1});

            //Assert
            Assert.IsInstanceOf<NotFoundResult>(actual:actualResult.Result);
        }

        [Test]
        public async Task CalculateInsurance_WhenProductIdsExist_ShouldReturn_OkObjectResult()
        {
            //Arrange
            const float expectedInsuranceValue = 10;
            _productServiceMock.Setup(x => x.GetProductsWithProductTypeAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<ProductDto> ());
            _orderCalculatorMock.Setup(x => x.Calculate(It.IsAny<List<ProductDto>>()))
                .Returns(expectedInsuranceValue);

            //Act
            var actualResult = await _productController.CalculateInsurance(new List<int> { 1 });

            //Assert
            Assert.IsInstanceOf<ActionResult<float>>(actualResult);
            Assert.IsInstanceOf<OkObjectResult>(actualResult.Result);
            var actualValue = ((OkObjectResult) actualResult.Result).Value;
            Assert.AreEqual(expected:expectedInsuranceValue,actual: (float) actualValue);
        }
    }
}