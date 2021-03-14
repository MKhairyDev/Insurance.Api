using System.Threading.Tasks;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Controllers;
using Insurance.Api.External.Models;
using Insurance.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.Controllers.Tests
{
    [TestFixture]
    internal class InsuranceControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private Mock<IInsuranceCalculator> _insuranceCalculatorMock;
        private InsuranceController _insuranceController;
        [SetUp]
        public void TestInitialization()
        {
            _productServiceMock = new Mock<IProductService>();
            _insuranceCalculatorMock = new Mock<IInsuranceCalculator>();
            _insuranceController = new InsuranceController(_productServiceMock.Object, _insuranceCalculatorMock.Object);
        }

        [Test]
        public async Task CalculateInsurance_WhenProductIdNotExist_ShouldReturn_HttpNotFound()
        {
            //Arrange
            _productServiceMock.Setup(x => x.GetProductWithProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync((ProductDto) null);

            //Act
            const int productId = 1;
            var actualResult = await _insuranceController.CalculateInsurance(productId);

            //Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(actual:actualResult.Result);
            Assert.AreEqual(expected:productId, actual:((NotFoundObjectResult) actualResult.Result).Value);
        }

        [Test]
        public async Task CalculateInsurance_WhenProductIdExist_ShouldReturn_OkObjectResult()
        {
            //Arrange
            const float expectedInsuranceValue = 10;
            _productServiceMock.Setup(x => x.GetProductWithProductTypeAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductDto());
            _insuranceCalculatorMock.Setup(x => x.CalculateProductInsurance(It.IsAny<ProductDto>()))
                .Returns(expectedInsuranceValue);

            //Act
            var productId = 1;
            var actualResult = await _insuranceController.CalculateInsurance(productId);

            //Assert
            Assert.IsInstanceOf<ActionResult<float>>(actualResult);
            Assert.IsInstanceOf<OkObjectResult>(actualResult.Result);
            var actualValue = ((OkObjectResult) actualResult.Result).Value;
            Assert.AreEqual(expected:expectedInsuranceValue,actual: (float) actualValue);
        }
    }
}