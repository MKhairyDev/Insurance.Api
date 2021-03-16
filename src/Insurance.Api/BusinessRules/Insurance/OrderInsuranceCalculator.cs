using System;
using System.Collections.Generic;
using Insurance.Api.Configuration;
using Insurance.Api.External.Models;
using Microsoft.Extensions.Options;

namespace Insurance.Api.BusinessRules.Insurance
{
    public class OrderInsuranceCalculator: IInsuranceCalculator<List<ProductDto>>
    {
        private readonly IInsuranceCalculator<ProductDto> _productCalculator;
        private readonly List<IInsuranceRule<List<ProductDto>>> _insuranceRules;

        public OrderInsuranceCalculator(IInsuranceCalculator<ProductDto> productCalculator, IOptionsMonitor<OrderWithSpecificProductTypeConfig> ordreWithSpecificProductTypeConfig)
        {
            _productCalculator = productCalculator ?? throw new ArgumentNullException(nameof(productCalculator));

            _insuranceRules = new List<IInsuranceRule<List<ProductDto>>>
            {
                new OrderContainsCertainProductTypeNumberRule(ordreWithSpecificProductTypeConfig)
            };
        }

        public float Calculate(List<ProductDto> products)
        {
            if (products == null ) 
                throw new ArgumentNullException(nameof(products)); 

            float insuranceValue = 0f;
            foreach (var item in products)
            {
                insuranceValue+= _productCalculator.Calculate(item);
            }
            foreach (var rule in _insuranceRules)
            {
                if (rule.Match(products))
                {
                    insuranceValue+= rule.Calculate();
                    return insuranceValue;
                }
            }

            return insuranceValue;
        }
    }
}