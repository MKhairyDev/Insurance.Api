using System.Net.Http;
using System.Threading.Tasks;
using Insurance.Api.Configuration;
using Insurance.Api.External;
using Insurance.Api.Models;
using Insurance.Api.UnitTests.Handlers;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Utilities.Polly.Exceptions;

namespace Insurance.Api.UnitTests.External.Tests
{
    [TestFixture]
    public class ProductApiClientTests
    {
        [SetUp]
        public void TestInitialization()
        {
            _configurationMock = new Mock<IOptionsMonitor<ExternalServicesConfig>>();
            _configurationMock.Setup(x => x.Get(ExternalServicesConfig.ProductApi))
                .Returns(new ExternalServicesConfig {Url = "http://test.com"});
        }

        private Mock<IOptionsMonitor<ExternalServicesConfig>> _configurationMock;

        [Test]
        public void GetProduct_OnNotFoundResponse_ResourceNotFoundExceptionISThrown()
        {
            using var httpClient = new HttpClient(new ResourceNotFoundResponseHandler());
            var productApiClient = InitializeProductApiClient(httpClient);
            Assert.ThrowsAsync<ResourceNotFoundException>(async () =>
                await productApiClient.GetProductAsync(productId: 1000));
        }

        [Test]
        public void GetProduct_UnauthorizedResponse_UnauthorizedAccessExceptionISThrown()
        {
            using var httpClient = new HttpClient(new Unauthorized401ResponseHandler());
            var productApiClient = InitializeProductApiClient(httpClient);
            Assert.ThrowsAsync<UnauthorizedApiAccessException>(async () =>
                await productApiClient.GetProductAsync(productId: 100));
        }

        [Test]
        public async Task GetProductType_ValidResponse_ProductTypeDtoInstanceShouldBeReturned()
        {
            using var httpClient = new HttpClient(new OkResponseHandler<ProductTypeDto>());
            var productApiClient = InitializeProductApiClient(httpClient);

            var actualResult = await productApiClient.GetProductTypeAsync(productTypeId: 100);
            Assert.IsInstanceOf(expected:typeof(ProductTypeDto),actual: actualResult);
        }

        private ProductApiClient InitializeProductApiClient(HttpClient httpClient)
        {
            return
                new(httpClient, _configurationMock.Object);
        }
    }
}