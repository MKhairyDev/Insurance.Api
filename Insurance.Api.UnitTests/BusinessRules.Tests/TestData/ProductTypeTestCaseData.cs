using System.Collections;
using Insurance.Api.External.Models;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.BusinessRules.Tests.TestData
{
    public static class ProductTypeTestCaseData
    {
        public static IEnumerable ParameterData
        {
            get
            {

                //Given Product of type 'Laptop' Should Add 500 More To Insurance Cost
                yield return new TestCaseData(
                    new ProductDto { Name = "product_1", SalesPrice = 300, ProductTypeDto = new ProductTypeDto { Name = "Laptops", CanBeInsured = true } }, 500);

                //Given Product of type 'Smartphones' Should Add 500 More To Insurance Cost
                yield return new TestCaseData(
                    new ProductDto { Name = "product_2", SalesPrice = 600, ProductTypeDto = new ProductTypeDto { Name = "Smartphones", CanBeInsured = true } },
                    1500);

                // Given Product of type 'Smartphones' Should Add 500 More To Insurance Cost 
                yield return new TestCaseData(
                    new ProductDto { Name = "product_3", SalesPrice = 3000, ProductTypeDto = new ProductTypeDto { Name = "Laptops", CanBeInsured = true } },
                    2500);
            }
        }
    }
}