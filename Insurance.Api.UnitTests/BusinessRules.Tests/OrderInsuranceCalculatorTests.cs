using System.Collections.Generic;
using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.Configuration;
using Insurance.Api.External.Models;
using Insurance.Api.UnitTests.BusinessRules.Tests.TestData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.BusinessRules.Tests
{
    [TestFixture]
    public class OrderInsuranceCalculatorTests : InsuranceBase
    {

        private OrderInsuranceCalculator _orderInsuranceCalculator;
        // private Mock<IInsuranceCalculator<ProductDto>> _productCalculatorMock;
        private IInsuranceCalculator<ProductDto> _productInsuranceCalculator;
         [SetUp]
        public void PreTextInitialization()
        {
            //_productCalculatorMock = new Mock<IInsuranceCalculator<ProductDto>>();
            _productInsuranceCalculator = new ProductInsuranceCalculator(salesPriceConfig: SalesConfigMock.Object,
                productTypeConfig: ProductConfigMock.Object);
            _productInsuranceCalculator = new ProductInsuranceCalculator(salesPriceConfig: SalesConfigMock.Object,
                productTypeConfig: ProductConfigMock.Object);
            _orderInsuranceCalculator = new OrderInsuranceCalculator(_productInsuranceCalculator, OrderWithSpecificProductTypeConfigMock.Object);
        }
        [Test]
        [TestCaseSource(sourceType: typeof(OrderWithSpecificProductTypeTestCaseData),
            sourceName: nameof(OrderWithSpecificProductTypeTestCaseData.ParameterData))]
        public void GivenOrder_InsuranceValueShouldMatchBusinessRules(List<ProductDto> productList,
            float expectedValue)
        {
            //Act
            var actualValue = _orderInsuranceCalculator.Calculate(productList);

            //Assert
            Assert.AreEqual(expected: expectedValue, actual: actualValue);
        }


    }
}