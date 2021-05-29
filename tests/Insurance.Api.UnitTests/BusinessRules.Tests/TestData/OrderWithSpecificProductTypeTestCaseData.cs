using System.Collections;
using System.Collections.Generic;
using Insurance.Api.Models;
using NUnit.Framework;

namespace Insurance.Api.UnitTests.BusinessRules.Tests.TestData
{
    public static class OrderWithSpecificProductTypeTestCaseData
    {
        public static IEnumerable ParameterData
        {
            get
            {
                //Given order with multiple products Should calculate the overall insurance cost.

                yield return new TestCaseData(
                    new List<ProductDto>
                    {
                        new ProductDto
                        {
                            Name = "product_1", ProductTypeDto = new ProductTypeDto {CanBeInsured = true,Name = "Type_1"},
                            SalesPrice = 300
                        },
                        new ProductDto
                        {
                            Name = "product_1", ProductTypeDto = new ProductTypeDto {CanBeInsured = true,Name = "Type_1"},
                            SalesPrice = 600
                        },
                        new ProductDto
                        {
                            Name = "product_1", ProductTypeDto = new ProductTypeDto {CanBeInsured = true,Name = "Type_1"},
                            SalesPrice = 2100
                        }
                    },3000);

                //Given order with one or more Digital cameras Should Add 500 To insurance Cost

                yield return new TestCaseData(
                    new List<ProductDto>
                    {
                        new ProductDto
                        {
                            Name = "product_1", ProductTypeDto = new ProductTypeDto {CanBeInsured = false,Name = "Digital cameras"},// Add 0 to insurance cost
                            SalesPrice = 300
                        },
                        new ProductDto
                        {
                            Name = "product_1", ProductTypeDto = new ProductTypeDto {CanBeInsured = true,Name = "Digital cameras"},// Add 1000 to insurance cost
                            SalesPrice = 600
                        },
                        new ProductDto
                        {
                            Name = "product_1", ProductTypeDto = new ProductTypeDto {CanBeInsured = true,Name = "Smartphones"},// Add 2500 to insurance cost
                            SalesPrice = 2100
                        }
                        //Adding 500 Euro to the overall insurance cost because the order contains two Digital cameras.
                    }, 4000);

            }
        }
    }
}