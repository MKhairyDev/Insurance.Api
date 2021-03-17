using System;
using System.Collections.Generic;
using Insurance.Api.Configuration;
using Insurance.Api.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class ProductInsuranceCalculator : IInsuranceCalculator<ProductDto>
    {
        private readonly List<IInsuranceRule<ProductDto>> _insuranceRules;

        public ProductInsuranceCalculator(IOptionsMonitor<SalesPriceConfig> salesPriceConfig, IOptionsMonitor<ProductTypeConfig> productTypeConfig)
        {
            _insuranceRules = new List<IInsuranceRule<ProductDto>>()
            {
                //todo:those values should be configured somewhere!

                new ProductTypeNameRule(productTypeConfig, new SalesPriceAggregationRule(salesPriceConfig)),
                new SalesPriceAggregationRule(salesPriceConfig)
            };
        }

        public float Calculate(ProductDto product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            foreach (var rule in _insuranceRules)
            {
                if (rule.Match(product))
                {
                    return rule.Calculate();
                }
            }

            return 0f;
        }

    }
}