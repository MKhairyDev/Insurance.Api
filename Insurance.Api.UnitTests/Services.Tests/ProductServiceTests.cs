using System.Threading.Tasks;
using Insurance.Api.External;
using Insurance.Api.External.Models;
using Insurance.Api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Utilities.Polly.Exceptions;

namespace Insurance.Api.UnitTests.Services.Tests
{
    [TestFixture]
    internal class ProductServiceTests
    {
        [SetUp]
        public void TestInitialization()
        {
            _loggerMock = new Mock<ILogger<ProductService>>();
            _productApiClient = new Mock<IProductApiClient>();
            _productService = new ProductService(_productApiClient.Object, _loggerMock.Object);
        }

        private Mock<IProductApiClient> _productApiClient;
        private ProductService _productService;
        private Mock<ILogger<ProductService>> _loggerMock;

        [Test]
        public async Task WhenProductNotExist_ShouldReturn_Null()
        {
            //Arrange
            _productApiClient.Setup(x => x.GetProductAsync(It.IsAny<int>())).Throws(new ResourceNotFoundException());

            //Act
            var actualValue = await _productService.GetProductWithProductTypeAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(actualValue);
        }

        [Test]
        public async Task WhenProductTypeNotExist_ShouldReturn_Null()
        {
            //Arrange
            _productApiClient.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(new ProductDto());
            _productApiClient.Setup(x => x.GetProductTypeAsync(It.IsAny<int>()))
                .Throws(new ResourceNotFoundException());

            //Act
            var actualValue = await _productService.GetProductWithProductTypeAsync(It.IsAny<int>());

            //Assert
            Assert.IsNull(actualValue);
        }

        [Test]
        public async Task WhenProductWithTypeExist_ShouldReturn_ValidProductDtoInstance()
        {
            //Arrange
            var product = new ProductDto {Id = 1, Name = "Product_1", SalesPrice = 100};
            var productType = new ProductTypeDto {Id = 1, CanBeInsured = true, Name = "Product_type_1"};
            _productApiClient.Setup(x => x.GetProductAsync(It.IsAny<int>())).ReturnsAsync(product);
            _productApiClient.Setup(x => x.GetProductTypeAsync(It.IsAny<int>())).ReturnsAsync(productType);

            //Act
            var actualValue = await _productService.GetProductWithProductTypeAsync(It.IsAny<int>());

            //Assert
            product.ProductTypeDto = productType;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(product, actualValue);
        }
    }
}