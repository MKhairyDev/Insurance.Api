using System.Collections;
using Insurance.Api.External.Models;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.BusinessRules.Tests.TestData
{
    public static class SalesPriceTestCaseData
    {
        public static IEnumerable ParameterData
        {
            get
            {

                //Given Sales Price Between less that 500 Euros Should Add zero ToInsurance Cost
                yield return new TestCaseData(
                    new ProductDto {Name = "product_1",ProductTypeDto = new ProductTypeDto {CanBeInsured = true}, SalesPrice = 300}, 0);

                // Given Sales Price Between 500 And 2000 Euros Should Add Thousand Euros To Insurance Cost
                yield return new TestCaseData(
                    new ProductDto { Name = "product_2", ProductTypeDto = new ProductTypeDto {CanBeInsured = true}, SalesPrice = 600},
                    1000);

                // Given Sales Price bigger that 2000 Euros Should Add Two Thousand Euros To Insurance Cost
                yield return new TestCaseData(
                    new ProductDto { Name = "product_3", ProductTypeDto = new ProductTypeDto {CanBeInsured = true}, SalesPrice = 3000},
                    2000);
            }
        }
    }
}