﻿using Insurance.Api.BusinessRules.Insurance;
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
    public class ProductInsuranceCalculatorTests : InsuranceBase
    {

        private ProductInsuranceCalculator _insuranceCalculator;

        [SetUp]
        public void PreTextInitialization()
        {
            _insuranceCalculator = new ProductInsuranceCalculator(salesPriceConfig: SalesConfigMock.Object,
                productTypeConfig: ProductConfigMock.Object);
        }
        [Test]
        [TestCaseSource(sourceType: typeof(SalesPriceTestCaseData),
            sourceName: nameof(SalesPriceTestCaseData.ParameterData))]
        public void GivenSalesPrice_InsuranceValueShouldMatchBusinessRules(ProductDto product,
            float expectedValue)
        {
            //Act
            var actualValue = _insuranceCalculator.Calculate(product: product);

            //Assert
            Assert.AreEqual(expected: expectedValue, actual: actualValue);
        }

        [Test]
        [TestCaseSource(sourceType: typeof(ProductTypeTestCaseData),
            sourceName: nameof(ProductTypeTestCaseData.ParameterData))]
        public void GivenSpecialProductType_ExtraInsuranceValueShouldBeAddedToOverallValue(ProductDto product,
            float expectedValue)
        {
            //Act
            var actualValue = _insuranceCalculator.Calculate(product: product);

            //Assert
            Assert.AreEqual(expected: expectedValue, actual: actualValue);
        }

    }
}