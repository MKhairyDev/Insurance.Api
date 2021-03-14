using System;
using System.Collections.Generic;
using Insurance.Api.External.Models;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class InsuranceCalculator : IInsuranceCalculator
    {
        private readonly List<IInsuranceRule> _insuranceRules;

        public InsuranceCalculator()
        {
            _insuranceRules = new List<IInsuranceRule>()
            {
                //todo:those values should be configured somewhere!

                new ProductTypeNameRule(insuranceValueToAdd:500,lookupCollection: new List<string>() {"Laptops", "Smartphones"},
                    new SalesPriceAggregationRule()),
                new SalesPriceAggregationRule()
            };
        }

        public float CalculateProductInsurance(ProductDto product)
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