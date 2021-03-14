using Insurance.Api.BusinessRules.Insurance;
using Insurance.Api.External.Models;
using Insurance.Api.UnitTests.BusinessRules.Tests.TestData;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.BusinessRules.Tests
{
    [TestFixture]
    internal class InsuranceControllerTests
    {
        [SetUp]
        public void TestInitialization()
        {
            _insuranceCalculator = new InsuranceCalculator();
        }

        private InsuranceCalculator _insuranceCalculator;

        [Test]
        [TestCaseSource(typeof(SalesPriceTestCaseData), nameof(SalesPriceTestCaseData.ParameterData))]
        public void GivenSalesPrice_InsuranceValueShouldMatchBusinessRules(ProductDto product,
            float expectedValue)
        {
            //Act
            var actualValue = _insuranceCalculator.CalculateProductInsurance(product);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [Test]
        [TestCaseSource(typeof(ProductTypeTestCaseData), nameof(ProductTypeTestCaseData.ParameterData))]
        public void GivenSpecialProductType_ExtraInsuranceValueShouldBeAddedToOverallValue(ProductDto product,
            float expectedValue)
        {
            //Act
            var actualValue = _insuranceCalculator.CalculateProductInsurance(product);

            //Assert
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}